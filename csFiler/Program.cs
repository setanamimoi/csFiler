using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using csFiler.Native;
using Wpf = System.Windows;

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
            using (var commandMenu = new ToolStripMenuItem("コマンド入力"))
            using (var exitMenuItem = new ToolStripMenuItem("終了"))
            using (var contextMenu = new ContextMenuStrip())
            using (var taskTray = new NotifyIcon())
            using (var hotKeyLoop = new HotKeyMessageLoop())
            {
                taskTray.Icon = icon;
                taskTray.Text = "csFiler";
                taskTray.ContextMenuStrip = contextMenu;

                var openCommand = new HotKey(
                    new Action(() => new FilerWindow().Show()), 
                    Key.V, ModifierKeys.Windows);

                hotKeyLoop.Add(openCommand);

                contextMenu.Items.AddRange(
                    new ToolStripItem []
                    { 
                        commandMenu,
                        exitMenuItem,
                    });
                contextMenu.Click += (sender, e) => openCommand.Action.Invoke();
                exitMenuItem.Click += (sender, e) => Wpf.Application.Current.Shutdown();

                var application = new Wpf.Application()
                {
                    ShutdownMode = Wpf.ShutdownMode.OnExplicitShutdown,
                };

                application.DispatcherUnhandledException += (sender, e) =>
                {
                    MessageBox.Show(
                        string.Concat("想定外のエラーが発生しました。",
                        Environment.NewLine, e.Exception.Message), "csFiler");
                    e.Handled = true;
                };

                application.Exit += (sender, e) =>
                {
                    hotKeyLoop.Dispose();
                    taskTray.Dispose();
                    contextMenu.Dispose();
                    exitMenuItem.Dispose();
                    commandMenu.Dispose();
                    icon.Dispose();
                };

                taskTray.Visible = true;
                application.Run();
            }
        }
    }
}
