using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SCNET_Restart_Tool;

namespace SCNET_Restart_Tool.Uno.Controls;

/// <summary>
/// 文件夹条目模型
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
/// 普通文件夹浏览控件（替代 NormalFolderTabCreator）
/// </summary>
public sealed partial class FolderBrowserControl : UserControl
{
    private readonly string _rootPath;
    private string _currentPath;
    private readonly string _tabName;
    private readonly ObservableCollection<FolderEntry> _entries = new();

    public FolderBrowserControl(string tabName, string rootPath)
    {
        this.InitializeComponent();
        _tabName = tabName;
        _rootPath = rootPath;
        _currentPath = rootPath;
        fileListView.ItemsSource = _entries;
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

    private async void FileListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is not FolderEntry entry || !entry.IsFolder) return;

        LoadFiles(entry.FullPath);
        await Task.CompletedTask;
    }

    private void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        LoadFiles(_currentPath);
    }

    private async void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        var parent = Directory.GetParent(_currentPath);
        if (parent != null)
        {
            LoadFiles(parent.FullName);
        }
        else
        {
            var root = XamlRoot.Content as FrameworkElement;
            if (root != null)
            {
                await ShowDialogAsync("提示", "已到根目录");
            }
        }
    }

    private void BtnOpen_Click(object sender, RoutedEventArgs e)
    {
        _ = FolderOperations.OpenFolderAsync(_currentPath, _tabName);
    }

    private async void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        await FolderOperations.AddFileToFolderAsync(_currentPath, _tabName);
        LoadFiles(_currentPath);
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (fileListView.SelectedItem is not FolderEntry entry || string.IsNullOrEmpty(entry.Name))
        {
            await ShowDialogAsync("提示", "请选中文件或文件夹");
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

    private async Task ShowDialogAsync(string title, string content)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = content,
            CloseButtonText = "确定",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();
    }
}
