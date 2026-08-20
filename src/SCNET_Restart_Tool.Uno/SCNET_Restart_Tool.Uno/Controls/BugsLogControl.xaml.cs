using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SCNET_Restart_Tool;

namespace SCNET_Restart_Tool.Uno.Controls;

/// <summary>
/// Bugs 日志标签页控件（替代 BugsLogTabCreator）
/// - Game.log 分页预览
/// - 其他文件列表
/// - 清理/删除/备份操作
/// </summary>
public sealed partial class BugsLogControl : UserControl
{
    private const string GameLogFileName = "Game.log";
    private const int PageLineCount = 1000;

    private readonly string _folderPath;
    private readonly ServerConfig _server;
    private readonly List<string> _logLinesCache = new();
    private int _currentPage = 1;
    private int _totalPages = 1;

    private class FileEntry
    {
        public string Name { get; set; } = "";
        public string Size { get; set; } = "";
        public string LastWriteTime { get; set; } = "";
    }

    public BugsLogControl(ServerConfig server, string folderPath)
    {
        this.InitializeComponent();
        _server = server;
        _folderPath = folderPath;
        pathText.Text = folderPath;

        fileListView.ItemsSource = new ObservableCollection<FileEntry>();
        LoadOtherFiles();

        var logPath = Path.Combine(folderPath, GameLogFileName);
        if (File.Exists(logPath))
        {
            _ = LoadLogFileAsync(logPath);
        }
        else
        {
            logPreview.Text = $"未找到 {GameLogFileName} 或文件被占用";
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
        }
    }

    private async Task LoadLogFileAsync(string path)
    {
        try
        {
            logPreview.Text = "正在加载日志...";
            _logLinesCache.Clear();

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fileStream))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    _logLinesCache.Add(line);
                }
            }

            _totalPages = (int)Math.Ceiling(_logLinesCache.Count / (double)PageLineCount);
            _currentPage = 1;
            UpdateLogPage();
        }
        catch (Exception ex)
        {
            logPreview.Text = $"加载失败：{ex.Message}（文件可能被占用或无权限）";
        }
    }

    private void UpdateLogPage()
    {
        var start = (_currentPage - 1) * PageLineCount;
        var end = Math.Min(start + PageLineCount, _logLinesCache.Count);
        var pageLines = new string[end - start];
        _logLinesCache.CopyTo(start, pageLines, 0, end - start);

        logPreview.Text = string.Join(Environment.NewLine, pageLines);
        pageLabel.Text = $"第 {_currentPage}/{_totalPages} 页（共 {_logLinesCache.Count} 行）";
    }

    private void LoadOtherFiles()
    {
        var list = (ObservableCollection<FileEntry>)fileListView.ItemsSource;
        list.Clear();

        if (!Directory.Exists(_folderPath)) return;

        try
        {
            foreach (var filePath in Directory.GetFiles(_folderPath))
            {
                var fileName = Path.GetFileName(filePath);
                if (fileName.Equals(GameLogFileName, StringComparison.OrdinalIgnoreCase))
                    continue;

                var fileInfo = new FileInfo(filePath);
                list.Add(new FileEntry
                {
                    Name = fileName,
                    Size = (fileInfo.Length / 1024.0).ToString("F2"),
                    LastWriteTime = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                });
            }
        }
        catch (Exception ex)
        {
            logPreview.Text += Environment.NewLine + $"加载文件列表失败：{ex.Message}";
        }
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e)
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            UpdateLogPage();
        }
    }

    private void BtnNext_Click(object sender, RoutedEventArgs e)
    {
        if (_currentPage < _totalPages)
        {
            _currentPage++;
            UpdateLogPage();
        }
    }

    private void BtnRefresh_Click(object sender, RoutedEventArgs e)
    {
        LoadOtherFiles();
    }

    private void BtnOpen_Click(object sender, RoutedEventArgs e)
    {
        _ = FolderOperations.OpenFolderAsync(_folderPath, "Bugs");
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (fileListView.SelectedItem is not FileEntry entry)
        {
            await ShowDialogAsync("提示", "请先选中要删除的文件！");
            return;
        }

        await FolderOperations.DeleteFileAsync(_folderPath, entry.Name, "Bugs");
        LoadOtherFiles();
    }

    private async void BtnClean_Click(object sender, RoutedEventArgs e)
    {
        await FolderOperations.CleanBugsLogAsync(_folderPath, GameLogFileName);
        LoadOtherFiles();
    }

    private void BtnBatchBackup_Click(object sender, RoutedEventArgs e)
    {
        _ = FolderOperations.BatchBackupAllFoldersAsync(_server);
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
