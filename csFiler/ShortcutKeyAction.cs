using System;
using System.Windows.Input;

namespace csFiler
{
    /// <summary>
    /// ショートカットキーの動作を定義したクラスです。
    /// </summary>
    public class ShortcutKeyAction
    {
        /// <summary>
        /// ショートカットキーの識別子キーを取得・設定します。
        /// </summary>
        public Key Key { get; set; }
        /// <summary>
        /// ショートカットキーの修飾子キーを取得・設定します。
        /// </summary>
        public ModifierKeys ModifierKeys { get; set; }
        /// <summary>
        /// ModifierKeys プロパティと Key プロパティで指定したショートカットキーに紐付ける動作を取得・設定します。
        /// </summary>
        public Func<bool> Action { get; set; }
    }
}
