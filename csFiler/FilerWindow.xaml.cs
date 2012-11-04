using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Windows.Controls;

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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.processTextBox.Focus();

            var intellisenceItemsSource = new string[] {
                "control",
                "notepad",
            };
            this.intellisenceListBox.ItemsSource = intellisenceItemsSource;
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.intellisence.IsOpen = false;
        }

        private void processTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Space) == true && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                this.intellisence.IsOpen = true;
                e.Handled = true;
            }
            else if (Keyboard.IsKeyDown(Key.Down) == true)
            {
                if (this.intellisence.IsOpen == false)
                {
                    return;
                }

                this.intellisenceListBox.Focus();
                this.intellisenceListBox.SelectedIndex = 0;
                var selectedItem = this.intellisenceListBox.ItemContainerGenerator.ContainerFromIndex(this.intellisenceListBox.SelectedIndex) as ListBoxItem;
                selectedItem.Focus();
                e.Handled = true;
            }
            else if (Keyboard.IsKeyDown(Key.Enter) == true)
            {
                var process = new ProcessStartInfo();
                process.BindFromCommandLine(this.processTextBox.Text);

                using (Process.Start(process)) { }

                this.Close();
                e.Handled = true;
            }
        }

        private void intellisenceListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Up) == true)
            {
                if (this.intellisenceListBox.SelectedIndex != 0)
                {
                    return;
                }

                this.intellisenceListBox.SelectedIndex = -1;
                this.processTextBox.Focus();
                e.Handled = true;
            }
        }
    }
}
