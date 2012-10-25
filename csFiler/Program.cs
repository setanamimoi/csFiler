using System;
using System.Windows;

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
            new Application().Run(new FilerWindow());
        }
    }
}
