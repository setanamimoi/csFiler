using System.Diagnostics;
using System.Windows;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.processTextBox.Focus();

            var intellisenceItemsSource = new string[] {
                "control",
                "notepad",
            };
            this.intellisenceListBox.ItemsSource = intellisenceItemsSource;
            this.intellisence.IsOpen = true;
        }

        private void processTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter) == true)
            {
                var process = new ProcessStartInfo();
                process.BindFromCommandLine(this.processTextBox.Text);

                using (Process.Start(process)) { }

                this.Close();
            }
        }
    }
}
