using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace csFiler.Native
{
    /// <summary>
    /// ホットキーを処理するメッセージループです。
    /// </summary>
    public class HotKeyMessageLoop : NativeWindow, IDisposable
    {
        /// <summary>
        /// HotKeyMessageLoop クラスの新しいインスタンスを初期化します。
        /// </summary>
        public HotKeyMessageLoop()
        {
            this.HotKies = new Dictionary<short, HotKey>();
            this.CreateHandle(new CreateParams());
        }

        /// <summary>
        /// ATOM 値とホットキーの配列を取得・設定します。
        /// </summary>
        private Dictionary<short, HotKey> HotKies { get; set; }

        /// <summary>
        /// ホットキーを追加します。
        /// </summary>
        public void Add(HotKey hotKey)
        {
            var atom = Kernel32.GlobalAddAtom(hotKey.ToString());
            if (atom == (short)IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var model = new
            {
                ID = HotKey.GetIDFromAtom(atom),
                Modifiers = (uint)hotKey.ModiferKeys,
                Key = (uint)KeyInterop.VirtualKeyFromKey(hotKey.Key),
            };

            if (User32.RegisterHotKey(this.Handle, model.ID, model.Modifiers, model.Key) == false)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            this.HotKies.Add(atom, hotKey);
        }

        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        public void Dispose()
        {
            foreach (short atom in this.HotKies.Keys)
            {
                if (User32.UnregisterHotKey(this.Handle, HotKey.GetIDFromAtom(atom)) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                if (Kernel32.GlobalDeleteAtom(atom) != (short)IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            this.ReleaseHandle();
        }

        /// <summary>
        /// HotKey の Windows メッセージを処理します。
        /// </summary>
        /// <param name="message">Windows メッセージ</param>
        protected override void WndProc(ref Message message)
        {
            var isCompleted = false;

            try
            {
                if (message.Msg != HotKey.WindowsMessageID)
                {
                    return;
                }

                var windowsParameter = message.WParam;

                short? atom = this.HotKies.Keys
                    .Where(m => (int)windowsParameter == HotKey.GetIDFromAtom(m))
                    .FirstOrDefault();

                if (atom == null)
                {
                    return;
                }

                this.HotKies[(short)atom].Action.Invoke();
                isCompleted = true;
            }
            finally
            {
                if (isCompleted == false)
                {
                    base.WndProc(ref message);
                }
            }
        }
    }
}