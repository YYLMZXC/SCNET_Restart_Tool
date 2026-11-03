using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SCNET_Restart_Tool;

namespace SCNET_Restart_Tool.Avalonia.Controls;

/// <summary>
/// 文件夹条目模型（列表绑定用）
/// </summary>
public class FolderEntry
{
    public string Type { get; set; } = "";
    public string Name { get; set; } = "";
    public string Size { get; set; } = "";
    public string LastWriteTime { get; set; } = "";
    public string FullPath { get; set; } = "";
    public bool IsFolder { get; set; }
}

/// <summary>
/// 普通文件夹浏览控件（对应原 Uno 版 FolderBrowserControl / WinForms 版 NormalFolderTabCreator）。
/// 列表单击文件夹即进入该目录，文件需先选中后再执行删除等操作。
/// </summary>
public partial class FolderBrowserControl : UserControl
{
    private readonly string _tabName;
    private readonly ObservableCollection<FolderEntry> _entries = new();
    private string _currentPath;

    public FolderBrowserControl(string tabName, string rootPath)
    {
        InitializeComponent();

        _tabName = tabName;
        _currentPath = rootPath;

        fileListBox.ItemsSource = _entries;
        LoadFiles(rootPath);
    }

    /// <summary>刷新当前目录</summary>
    public void Refresh()
    {
        LoadFiles(_currentPath);
    }

    private void LoadFiles(string targetPath)
    {
        _currentPath = targetPath;
        pathText.Text = targetPath;
        _entries.Clear();

        if (!Directory.Exists(targetPath))
        {
            _entries.Add(new FolderEntry { Type = "", Name = "当前目录不存在", Size = "-", LastWriteTime = "-" });
            return;
        }

        try
        {
            // 文件夹
            foreach (var dirPath in Directory.GetDirectories(targetPath))
            {
                var dirInfo = new DirectoryInfo(dirPath);
                _entries.Add(new FolderEntry
                {
                    Type = "文件夹",
                    Name = dirInfo.Name,
                    Size = "-",
                    LastWriteTime = dirInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm"),
                    FullPath = dirInfo.FullName,
                    IsFolder = true
                });
            }

            // 文件
            foreach (var filePath in Directory.GetFiles(targetPath))
            {
                var fileInfo = new FileInfo(filePath);
                _entries.Add(new FolderEntry
                {
                    Type = "文件",
                    Name = fileInfo.Name,
                    Size = (fileInfo.Length / 1024.0).ToString("F2"),
                    LastWriteTime = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm"),
                    FullPath = fileInfo.FullName,
                    IsFolder = false
                });
            }
        }
        catch (Exception ex)
        {
            _entries.Add(new FolderEntry { Type = "", Name = $"加载失败：{ex.Message}", Size = "-", LastWriteTime = "-" });
        }
    }

    /// <summary>单击文件夹即进入该目录（与 Uno 版的 ItemClick 行为一致）</summary>
    private void FileListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (fileListBox.SelectedItem is not FolderEntry entry || !entry.IsFolder) return;
        if (string.IsNullOrEmpty(entry.FullPath)) return;

        LoadFiles(entry.FullPath);
    }

    private void BtnRefresh_Click(object? sender, RoutedEventArgs e)
    {
        LoadFiles(_currentPath);
    }

    private async void BtnBack_Click(object? sender, RoutedEventArgs e)
    {
        var parent = Directory.GetParent(_currentPath);
        if (parent != null)
        {
            LoadFiles(parent.FullName);
            return;
        }

        // 已经位于磁盘根目录（如 C:\），无法继续向上
        await DialogService.ShowAsync("提示", "已到根目录");
    }

    private void BtnOpen_Click(object? sender, RoutedEventArgs e)
    {
        _ = FolderOperations.OpenFolderAsync(_currentPath, _tabName);
    }

    private async void BtnAdd_Click(object? sender, RoutedEventArgs e)
    {
        await FolderOperations.AddFileToFolderAsync(_currentPath, _tabName);
        LoadFiles(_currentPath);
    }

    private async void BtnDelete_Click(object? sender, RoutedEventArgs e)
    {
        if (fileListBox.SelectedItem is not FolderEntry entry || string.IsNullOrEmpty(entry.Name))
        {
            await DialogService.ShowAsync("提示", "请选中文件或文件夹");
            return;
        }

        if (entry.IsFolder)
        {
            await FolderOperations.DeleteFolderAsync(_currentPath, entry.Name, _tabName);
        }
        else
        {
            await FolderOperations.DeleteFileAsync(_currentPath, entry.Name, _tabName);
        }

        LoadFiles(_currentPath);
    }
}
