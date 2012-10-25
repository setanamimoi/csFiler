using System;
using Wpf = System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace csFiler
{
    /// <summary>
    /// エントリポイントを定義するクラス
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// エントリポイント
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var iconPath = Path.Combine(Application.StartupPath, "csFiler.ico");
            using (var icon = new Icon(iconPath))
            using (var commandMenu = new ToolStripMenuItem()
                {
                    Text = "コマンド入力",
                })
            using (var exitMenuItem = new ToolStripMenuItem()
                {
                    Text = "終了",
                })
            using (var contextMenu = new ContextMenuStrip())
            using (var taskTray = new NotifyIcon()
                {
                    Icon = icon,
                    Text = "csFiler",
                    ContextMenuStrip = contextMenu,
                })
            {
                contextMenu.Items.AddRange(
                    new ToolStripItem [] { 
                        commandMenu,
                        exitMenuItem,
                    });
                contextMenu.Click += new EventHandler((sender, e) =>
                {
                    new FilerWindow().Show();
                });
                exitMenuItem.Click += new EventHandler((sender, e) =>
                {
                    Wpf.Application.Current.Shutdown();
                });

                var application = new Wpf.Application()
                {
                    ShutdownMode = Wpf.ShutdownMode.OnExplicitShutdown,
                };
                application.DispatcherUnhandledException += new Wpf.Threading.DispatcherUnhandledExceptionEventHandler((sender, e) =>
                {
                    MessageBox.Show(
                        string.Concat("想定外のエラーが発生しました。",
                        Environment.NewLine, e.Exception.Message), "csFiler");
                    e.Handled = true;
                });
                application.Exit += new Wpf.ExitEventHandler((sender, e) =>
                {
                    taskTray.Dispose();
                    contextMenu.Dispose();
                    exitMenuItem.Dispose();
                    commandMenu.Dispose();
                    icon.Dispose();
                });

                taskTray.Visible = true;
                application.Run();
            }
        }
    }
}
