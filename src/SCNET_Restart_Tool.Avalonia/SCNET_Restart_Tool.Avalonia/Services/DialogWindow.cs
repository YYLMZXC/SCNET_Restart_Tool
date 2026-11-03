using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace SCNET_Restart_Tool.Avalonia;

/// <summary>
/// 自定义对话框窗口（替代 Uno 的 ContentDialog）：
/// - isConfirm = false：信息提示，仅「确定」按钮；
/// - isConfirm = true ：确认询问，「是 / 否」按钮，点击「是」返回 true。
/// 窗口宽度固定、高度随内容自适应，内容过长时内部滚动，避免帮助文本撑爆窗口。
/// </summary>
internal sealed class DialogWindow : Window
{
    private static readonly IBrush TitleBrush = new SolidColorBrush(Color.Parse("#2C3E50"));
    private static readonly IBrush ContentBrush = new SolidColorBrush(Color.Parse("#4A5560"));

    public DialogWindow(string title, string content, bool isConfirm)
    {
        Title = string.IsNullOrWhiteSpace(title) ? "提示" : title;
        Width = 440;
        SizeToContent = SizeToContent.Height;
        CanResize = false;
        ShowInTaskbar = false;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Background = Brushes.White;

        var root = new StackPanel { Spacing = 12 };

        root.Children.Add(new TextBlock
        {
            Text = Title,
            FontSize = 16,
            FontWeight = FontWeight.SemiBold,
            Foreground = TitleBrush
        });

        root.Children.Add(new ScrollViewer
        {
            MaxHeight = 380,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = new TextBlock
            {
                Text = content,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 21,
                Foreground = ContentBrush
            }
        });

        var buttons = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 8
        };

        if (isConfirm)
        {
            var noButton = CreateButton("否", "#95A5A6");
            noButton.Click += (_, _) => Close(false);
            buttons.Children.Add(noButton);

            var yesButton = CreateButton("是", "#3498DB");
            yesButton.Click += (_, _) => Close(true);
            buttons.Children.Add(yesButton);
        }
        else
        {
            var okButton = CreateButton("确定", "#3498DB");
            okButton.Click += (_, _) => Close(true);
            buttons.Children.Add(okButton);
        }

        root.Children.Add(buttons);

        Content = new Border
        {
            Padding = new Thickness(22, 20, 22, 18),
            Child = root
        };

        // Esc 视为取消/关闭
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape) return;
        e.Handled = true;
        Close(false);
    }

    /// <summary>创建对话框按钮，复用 App.axaml 中的彩色按钮主题</summary>
    private Button CreateButton(string text, string background)
    {
        var button = new Button
        {
            Content = text,
            Background = new SolidColorBrush(Color.Parse(background)),
            Foreground = Brushes.White,
            Padding = new Thickness(22, 8),
            MinWidth = 88,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };

        if (this.TryFindResource("ColorButton", out var theme) && theme is ControlTheme controlTheme)
        {
            button.Theme = controlTheme;
        }

        return button;
    }
}
