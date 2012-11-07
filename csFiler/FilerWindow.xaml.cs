using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace csFiler
{
    /// <summary>
    /// ファイラ画面
    /// </summary>
    public partial class FilerWindow : Window
    {
        /// <summary>
        /// FilerWindow の新しいインスタンスを初期化します。
        /// </summary>
        public FilerWindow()
        {
            InitializeComponent();

            var processTextBoxShortcutKey = new ShortcutKeyInvoker(this.processTextBox);
            processTextBoxShortcutKey.Add(() =>
            {
                if (this.intellisenceListBox.Items.Count == 0)
                {
                    this.intellisence.IsOpen = false;
                }
                else
                {
                    this.intellisence.IsOpen = true;
                }
                return true;
            }, Key.Space, ModifierKeys.Control);

            processTextBoxShortcutKey.Add(() =>
            {
                MessageBox.Show("action");
                return true;
            }, Key.Space, ModifierKeys.Control | ModifierKeys.Alt);
            processTextBoxShortcutKey.Add(() =>
                {
                    if (this.intellisence.IsOpen == false)
                    {
                        return false;
                    }

                    this.intellisenceListBox.SelectedIndex = 0;
                    this.intellisenceListBox.FocusSelectedItem();
                    return true;
                }, Key.Down, ModifierKeys.None);
            processTextBoxShortcutKey.Add(() =>
                {
                    var process = new ProcessStartInfo();
                    process.BindFromCommandLine(this.processTextBox.Text);

                    using (Process.Start(process)) { }

                    this.Close();
                    return true;
                }, Key.Enter, ModifierKeys.None);
            processTextBoxShortcutKey.Add(() =>
                {
                    if (this.intellisence.IsOpen == false)
                    {
                        return false;
                    }

                    if (this.intellisenceListBox.Items.Count != 1)
                    {
                        return false;
                    }

                    this.processTextBox.Text = Convert.ToString(this.intellisenceListBox.Items[0]);
                    this.processTextBox.Select(this.processTextBox.Text.Length, 0);
                    return true;
                }, Key.Tab, ModifierKeys.None);
            var intellisenceListBoxShortcutKey = new ShortcutKeyInvoker(this.intellisenceListBox);
            intellisenceListBoxShortcutKey.Add(() => 
                {
                    if (this.intellisenceListBox.SelectedIndex != 0)
                    {
                        return false;
                    }

                    this.intellisenceListBox.SelectedIndex = -1;

                    this.processTextBox.Focus();
                    return true;
                }
            ,Key.Up, ModifierKeys.None);
            var intellisenceDecide = new Func<bool>(() =>
            {
                this.processTextBox.Text = this.intellisenceListBox.SelectedItem as string;
                this.processTextBox.Select(this.processTextBox.Text.Length, 0);

                this.processTextBox.Focus();
                return true;
            });

            intellisenceListBoxShortcutKey.Add(intellisenceDecide, Key.Tab, ModifierKeys.None);
            intellisenceListBoxShortcutKey.Add(intellisenceDecide, Key.Enter, ModifierKeys.None);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.processTextBox.Focus();

            this.intellisenceListBox.ItemsSource = 
                new string[] {
                    "control",
                    "notepad",
                };
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.intellisence.IsOpen = false;
        }

        private void processTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var inputText = this.processTextBox.Text.Trim();

            this.intellisenceListBox.Items.Filter = (x) =>
            {
                var listItem = x as string;

                if (listItem == null)
                {
                    return false;
                }

                if (listItem == inputText)
                {
                    return false;
                }

                return listItem.Contains(inputText);
            };

            bool isAnyIntellisence = this.intellisenceListBox.Items.Cast<object>().Any();

            this.intellisence.IsOpen = isAnyIntellisence;
            e.Handled = true;
            return;
        }
    }
}
