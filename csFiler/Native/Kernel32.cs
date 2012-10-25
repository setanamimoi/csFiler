using System.Runtime.InteropServices;
using ATOM = System.Int16;
using LPCTSTR = System.String;

namespace csFiler.Native
{
    /// <summary>
    /// User32.lib で定義した関数をまとめたクラスです。
    /// </summary>
    internal static class Kernel32
    {
        /// <summary>
        /// インポートするDLLのファイル名
        /// </summary>
        private const string ImportDll = "Kernel32.dll";

        /// <summary>
        /// 文字列をグローバルアトムテーブルに格納し、その文字列を識別する一意の値（ アトム）を返します。
        /// </summary>
        /// <param name="lpString">追加したい文字列
        /// ［入力］グローバルアトムテーブルに格納したい、NULL で終わる文字列へのポインタを指定します。文字列の最大の長さは 255 バイトです。追加した文字列の大文字と小文字はそのまま格納されますが、文字列を比較する際に大文字と小文字を区別することはありません。格納した文字列を取得するには、GlobalGetAtomName 関数を使います。代わりに、 マクロを使って整数アトムを変換し、得られた文字列を使うこともできます。詳細については、この関数の「解説」を参照してください。
        /// </param>
        /// <returns>
        /// 関数が成功すると、新しく作成されたアトムが返ります。
        /// 関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、 関数を使います。
        /// </returns>
        /// <remarks>
        /// 指定された文字列が既にテーブル内に存在していた場合は、既存のアトムを返し、そのアトムの参照カウントをインクリメントします。
        /// 参照カウントが 0 になるまでは、アトムに関連付けられた文字列がメモリから削除されることはありません。
        /// 詳細については、GlobalDeleteAtom 関数を参照してください。
        /// アプリケーションが終了する際に、グローバルアトムが自動的に削除されることはありません。
        /// GlobalAddAtom を呼び出すたびに、最終的にはそれに対応する GlobalDeleteAtom を呼び出すべきです。
        /// "#1234" の形式で lpString を指定すると、GlobalAddAtom は整数アトムを返します。
        /// これは、文字列で指定された 10 進数を 16 ビット値で表現するものです（ この場合は、10 進の 1234 に相当する 0x04D2 になります）。
        /// 0x0000 に相当する 10 進数や、0xC000 以上の 10 進数を指定した場合は、0 が返り、エラーが発生したことを示します。
        /// マクロを使って lpString を作成した場合は、下位ワード（low-order word）は必ず 0x0001～0xBFFF の範囲にあります。
        /// 下位ワードがこの範囲にない場合、この関数は失敗します。
        /// これ以外の形式で lpString を指定すると、GlobalAddAtom は文字列アトムを返します。
        /// </remarks>
        [DllImport(ImportDll, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern ATOM GlobalAddAtom(LPCTSTR lpString);
        /// <summary>
        /// グローバル文字列アトムの参照カウントをデクリメントします。参照カウントが 0 になったときは、アトムに関連付けられている文字列を、グローバルアトムテーブルから削除します。
        /// </summary>
        /// <param name="nAtom">削除したいアトム
        /// ［入力］削除したい文字列アトムを指定します。
        /// </param>
        /// <returns>
        /// 関数が成功すると、0 が返ります。
        /// 関数が失敗すると、nAtom パラメータで指定した値が返ります。拡張エラー情報を取得するには、 関数を使います。
        /// </returns>
        /// <remarks>
        /// 文字列アトムの参照カウントは、そのアトムがアトムテーブルに追加された回数を表します。
        /// 指定された文字列が既にテーブル内に存在していた場合、GlobalAddAtom 関数を呼び出すたびに、この関数は参照カウントをインクリメントします。
        /// GlobalAddAtom を呼び出すたびに、最終的にはそれに対応する GlobalDeleteAtom を呼び出すべきです。
        /// GlobalAddAtom を呼び出した回数を超えて GlobalDeleteAtom を呼び出すことは避けてください。
        /// もしそのような呼び出しを行うと、他のクライアントがまだアトムを使っている最中に、そのアトムを削除してしまう可能性があります。
        /// DDE を利用するアプリケーションは、メモリリークや予期しない削除を防止するために、グローバルアトムの管理に関するルールに従うべきです。
        /// GlobalDeleteAtom 関数は、整数アトム（0x0001～0xBFFF の範囲の値を取るアトム）に対しては機能しません。常に 0 を返します。
        /// </remarks>
        [DllImport(ImportDll, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern ATOM GlobalDeleteAtom(ATOM nAtom);
    }
}
