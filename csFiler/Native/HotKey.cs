using System;
using System.Windows.Input;

namespace csFiler.Native
{
    /// <summary>
    /// ホットキーを定義したクラスです。
    /// </summary>
    public class HotKey
    {
        /// <summary>
        /// デフォルトコンストラクタによるインスタンスを許可していません。
        /// </summary>
        private HotKey()
        {
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="action">ホットキーを押した時に実行する処理</param>
        /// <param name="key">識別子</param>
        /// <param name="modifierKeys">修飾子</param>
        public HotKey(Action action, Key key, ModifierKeys modifierKeys = ModifierKeys.None)
        {
            this.Action = action;
            this.Key = key;
            this.ModiferKeys = modifierKeys;
        }
        /// <summary>
        /// ホットキーを処理する Windows メッセージ ID
        /// </summary>
        public const int WindowsMessageID = 0x312;
        /// <summary>
        /// 修飾子を取得・設定します。
        /// </summary>
        public ModifierKeys ModiferKeys { get; private set; }
        /// <summary>
        /// 識別子を取得・設定します。
        /// </summary>
        public Key Key { get; private set; }
        /// <summary>
        /// ホットキーを押した時に実行する処理を取得・設定します。
        /// </summary>
        public Action Action { get; private set; }

        /// <summary>
        /// ホットキーの ATOM 値を生成する元の文字列を返します。
        /// </summary>
        /// <returns>ホットキーのATOM値を生成する元の文字列</returns>
        public string AtomSourceString
        {
            get
            {
                return string.Format("{0}, {1}", this.ModiferKeys, this.Key);
            }
        }

        /// <summary>
        /// ATOM 値からホットキーのメッセージ処理で使用するホットキー ID を取得します。
        /// </summary>
        /// <param name="atom">ATOM値</param>
        /// <returns>ホットキーのメッセージ処理で使用するホットキー ID</returns>
        public static int GetIDFromAtom(short atom)
        {
            return atom & 0x0000ffff;
        }
    }
}
