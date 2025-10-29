using System;
using System.Drawing;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    internal static class ControlFactory
    {
        // 创建标准按钮
        public static Button CreateButton(FolderManagerForm form, string text, Color backColor, int width)
        {
            return new Button
            {
                Text = text,
                Width = width,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = backColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Font = new Font("微软雅黑", 9F)
            };
        }

        // 创建小尺寸按钮（用于分页）
        public static Button CreateSmallButton(FolderManagerForm form, string text, Color backColor, int width)
        {
            var btn = CreateButton(form, text, backColor, width);
            btn.Height = 25;
            btn.Font = new Font("微软雅黑", 8F);
            return btn;
        }

        // 创建带自动换行的按钮面板
        public static Panel CreateButtonPanel(FolderManagerForm form, params Button[] buttons)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70, // 足够高度容纳换行
                BackColor = Color.White,
                Padding = new Padding(10, 5, 10, 5)
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true, // 自动换行
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            foreach (var btn in buttons)
            {
                btn.Margin = new Padding(0, 0, 10, 5); // 按钮间距
                flowPanel.Controls.Add(btn);
            }

            panel.Controls.Add(flowPanel);
            return panel;
        }

        // 创建路径显示文本框
        public static TextBox CreatePathTextBox(FolderManagerForm form, string path, bool exists)
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Text = path,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.None,
                BackColor = form.LightGray,
                Font = new Font("微软雅黑", 9F),
                ForeColor = exists ? form.DarkGray : form.DangerColor
            };
        }

        // 创建标准文件列表表格
        public static DataGridView CreateFileGridView(FolderManagerForm form)
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.LightGray,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = { BackColor = form.LightGray },
                RowTemplate = { Height = 28 },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("微软雅黑", 9F, FontStyle.Bold),
                    ForeColor = form.DarkGray,
                    BackColor = form.LightGray,
                    Padding = new Padding(5, 0, 0, 0)
                }
            };
        }
    }
}