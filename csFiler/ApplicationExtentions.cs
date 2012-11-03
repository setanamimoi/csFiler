using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace csFiler
{
    /// <summary>
    /// アプリケーション内で拡張するメソッドを定義したクラスです。
    /// </summary>
    public static class ApplicationExtention
    {
        /// <summary>
        /// 指定した Unicode 文字がこの文字列内で見つかった位置のインデックスをレポートします。
        /// </summary>
        /// <param name="self">拡張する System.String</param>
        /// <param name="seek">シークする Unicode 文字</param>
        /// <returns>その文字が見つかった場合は、value の 0 から始まるインデックスでの位置。見つからなかった場合は 空の配列。</returns>
        public static int[] IndexOfAll(this string self, char seek)
        {
            if (self == null)
            {
                throw new ArgumentNullException("self");
            }

            List<int> ret = new List<int>();

            var currentIndex = self.IndexOf(seek);
            while (true)
            {
                if (currentIndex == -1)
                {
                    break;
                }
                ret.Add(currentIndex);
                currentIndex++;
                currentIndex = self.IndexOf(seek, currentIndex);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// コマンドラインからプロセス開始情報を読み取ります。
        /// </summary>
        /// <param name="self">拡張元インスタンス</param>
        /// <param name="commandLine">コマンドライン文字列</param>
        /// <remarks>
        /// この関数は以下の３種類の形式を認識します。
        /// "フルパス"(空白を含む 可) コマンドライン引数
        /// 実在するパス(空白を含む 可) コマンドライン引数
        /// ファイル名(空白を含む 不可) コマンドライン引数
        /// </remarks>
        /// <example>
        /// var p1 = new ProcessStartInfo().BindFromCommandLine(@"""C:\Program Files\Test.exe"" -s -f -t 0");
        /// Console.WriteLine(p1.FileName);  //出力：C:\Program Files\Test.exe
        /// Console.WriteLine(p1.Arguments); //出力：-s -f -t 0
        /// 
        /// var p2 = new ProcessStartInfo().BindFromCommandLine(@"C:\Program Files\Test.exe -s -f -t 0");
        /// Console.WriteLine(p2.FileName);  //出力：C:\Program Files\Test.exe
        /// Console.WriteLine(p2.Arguments); //出力：-s -f -t 0
        /// 
        /// var p3 = new ProcessStartInfo().BindFromCommandLine(@"Test -s -f -t 0");
        /// Console.WriteLine(p3.FileName);  //出力：Test
        /// Console.WriteLine(p3.Arguments); //出力：-s -f -t 0
        /// </example>
        public static void BindFromCommandLine(this ProcessStartInfo self, string commandLine)
        {
            if (self == null)
            {
                throw new ArgumentNullException("self");
            }
            if (commandLine == null)
            {
                throw new ArgumentNullException("commandLine");
            }

            var executeCommand = commandLine.Trim();
            var splitIndex = executeCommand.IndexOf(" ");

            //スペースが含まれる事を想定して条件分岐する
            if (executeCommand.IndexOf('"') == 0)
            {
                //ダブルクォーテーションから始まるトークンは次のダブルクォーテーションまでを一つのトークンと認識する
                //ファイルパスにダブルクォーテーションは含める事ができない為、エスケープされたダブルクォーテーションの可能性を無視する
                var doubleQuotationIndex = executeCommand.IndexOf('"', 1);

                if (doubleQuotationIndex == -1)
                {
                    throw new FormatException("コマンドラインに含まれるダブルクォーテーションの終端が閉じていません。トークンの終端としてダブルクォーテーションを含めてください。");
                }

                splitIndex = doubleQuotationIndex + 1;
            }
            else
            {
                //一番長い文字列がパスである事を想定しパスが実在するか確認し、実在する場合は一つのトークンとして認識する
                if (File.Exists(executeCommand) == true || Directory.Exists(executeCommand) == true)
                {
                    splitIndex = executeCommand.Length;
                }
                else
                {
                    var indexes = executeCommand.IndexOfAll(' ');
                    indexes.Reverse();

                    foreach (var index in indexes)
                    {
                        var length = index + 1;

                        var commandPart = string.Concat(executeCommand.Take(length));
                        if (File.Exists(commandPart) == true || Directory.Exists(commandPart) == true)
                        {
                            splitIndex = index;
                            break;
                        }
                    }
                }
            }

            self.FileName = executeCommand;
            if (splitIndex != -1)
            {
                self.FileName = string.Concat(executeCommand.Take(splitIndex));
            }
            self.FileName = self.FileName.Trim('"');

            self.Arguments = string.Empty;
            if (splitIndex != -1)
            {
                self.Arguments = string.Concat(executeCommand.Skip(splitIndex)).Trim();
            }

            return;
        }
    }
}
