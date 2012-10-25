using System.Runtime.InteropServices;
using BOOL = System.Boolean;
using HWND = System.IntPtr;
using UINT = System.UInt32;

namespace csFiler.Native
{
    /// <summary>
    /// User32.lib で定義した関数をまとめたクラスです。
    /// </summary>
    internal static class User32
    {
        /// <summary>
        /// ホットキーのメッセージ ID
        /// </summary>
        internal const int WM_HOTKEY = 0x312;

        /// <summary>
        /// インポートするDLLのファイル名
        /// </summary>
        private const string ImportDll = "User32.dll";

        /// <summary>
        /// システムワイド（ システム全体に適用される）のホットキーを定義します。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル
        /// ［入力］ホットキーによって生成された WM_HOTKEY メッセージを受け取るウインドウのハンドルを指定します。
        /// このパラメータに NULL を指定したときは、WM_HOTKEY メッセージは呼び出し側のスレッドのメッセージキューにポストされるので、
        /// そのメッセージループで処理しなければなりません。
        /// </param>
        /// <param name="id">ホットキーの識別子
        /// ［入力］ホットキーの識別子を指定します。現在のスレッド内の他のホットキーは、同じ識別子を使うべきではありません。
        /// アプリケーションは、0x0000～0xBFFF の範囲の値を指定しなければなりません。
        /// 共有ダイナミックリンクライブラリ（DLL）は、0xC000～0xFFFF（ 関数が返す範囲）の値を指定しなくてはなりません。
        /// 他の共有 DLL との競合を避けるために、各 DLL は GlobalAddAtom 関数を使ってホットキーの識別子を取得するべきです。
        /// </param>
        /// <param name="fsModifiers">キー修飾子フラグ
        /// ［入力］WM_HOTKEY メッセージを生成するために、nVirtKey パラメータで指定されたキーとともに押されるキーを指定します。
        /// 次の値の任意の組み合わせを指定します。
        /// </param>
        /// <param name="vk">仮想キーコード
        /// ［入力］ホットキーの仮想キーコードを指定します。
        /// </param>
        /// <returns>
        /// 関数が成功すると、0 以外の値が返ります。
        /// 関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、 関数を使います。
        /// </returns>
        /// <remarks>
        /// あるキーが押されると、システムはすべてのホットキーの中からそれに一致するものを探します。
        /// 一致するものが見つかると、システムはそのホットキーを登録したスレッドのメッセージキューに WM_HOTKEY メッセージをポストします。
        /// このメッセージは、キューの先頭にポストされ、メッセージループの次の反復で削除されます。
        /// この関数は、他のスレッドが生成したウィンドウにホットキーを関連付けることはできません。
        /// ホットキーに割り当てようとしたキーストロークが、他のホットキーによって既に登録されている場合、RegisterHotKey 関数は失敗します。
        /// hWnd パラメータで指定されたウィンドウが、id パラメータで指定したのと同じ ID を持つホットキーを既に登録していた場合、
        /// fsModifiers パラメータと vk パラメータの値は、新しいものに置き換えられます。
        /// </remarks>
        [DllImport(ImportDll, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern BOOL RegisterHotKey(HWND hWnd, int id, UINT fsModifiers, UINT vk);

        /// <summary>
        /// 呼び出し側スレッドが既に定義したホットキーを破棄します。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル
        /// ［入力］破棄したいホットキーに関連付けられているウィンドウのハンドルを指定します。
        /// ホットキーがどのウィンドウにも関連付けられていない場合、このパラメータには NULL を指定します。
        /// </param>
        /// <param name="id">ホットキーの識別子
        /// ［入力］破棄したいホットキーの識別子を指定します。
        /// </param>
        /// <returns>
        /// 関数が成功すると、0 以外の値が返ります。
        /// 関数が失敗すると、0 が返ります。
        /// 拡張エラー情報を取得するには、 関数を使います。
        /// </returns>
        [DllImport(ImportDll, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern BOOL UnregisterHotKey(HWND hWnd, int id);
    }
}
