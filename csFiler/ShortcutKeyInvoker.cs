using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace csFiler
{
    /// <summary>
    /// ショートカットキーを処理するクラスです。
    /// </summary>
    public class ShortcutKeyInvoker
    {
        /// <summary>
        /// ShortcutKeyInvoker クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="inputElement">ショートカットキーを紐付けるコントロール</param>
        public ShortcutKeyInvoker(IInputElement inputElement)
        {
            this.ShortcutKeys = new List<ShortcutKeyAction>();
            inputElement.PreviewKeyDown += (sender, e) => e.Handled = this.Invoke();
        }

        /// <summary>
        /// ショートカットキーの一覧を取得・設定します。
        /// </summary>
        private List<ShortcutKeyAction> ShortcutKeys
        {
            get;
            set;
        }

        /// <summary>
        /// 紐付けるショートカットキーを追加します。
        /// </summary>
        /// <param name="action">ショートカットキーの動作</param>
        /// <param name="key">ショートカット識別子キー</param>
        /// <param name="modiferKey">ショートカット修飾子キー</param>
        public void Add(Func<bool> action, Key key, ModifierKeys modiferKey)
        {
            this.ShortcutKeys.Add(new ShortcutKeyAction()
            {
                Action = action,
                Key = key,
                ModifierKeys = modiferKey,
            });
        }

        /// <summary>
        /// ショートカットキーを処理します。
        /// </summary>
        /// <remarks>ショートカットキーに紐付けられた処理が正しく処理されたかどうか 処理された場合, true 、そうでなければ　false 。</remarks>
        public bool Invoke()
        {
            var shortcutActions = this.ShortcutKeys.Select(model =>
            {
                List<ModifierKeys> modiList = new List<ModifierKeys>();

                foreach (ModifierKeys modifierKey in Enum.GetValues(typeof(ModifierKeys)))
                {
                    if ((model.ModifierKeys & modifierKey) == modifierKey)
                    {
                        modiList.Add(modifierKey);
                    }
                }

                return new
                {
                    Key = model.Key,
                    ModifierKeys = modiList.ToArray(),
                    Action = model.Action,
                };
            })
            .OrderByDescending(x => x.ModifierKeys.Count())
            .ToArray();

            foreach (var model in shortcutActions)
            {
                bool isError = false;
                foreach (ModifierKeys m in model.ModifierKeys)
                {
                    if ((Keyboard.Modifiers & m) != m)
                    {
                        isError = true;
                        break;
                    }
                }
                if (isError == true)
                {
                    continue;
                }

                if (Keyboard.IsKeyDown(model.Key) == true)
                {
                    return model.Action.Invoke();
                }
            }

            return false;
        }
    }
}
