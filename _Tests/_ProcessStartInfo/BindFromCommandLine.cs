using System;
using System.Diagnostics;
using csFiler;
using NUnit.Framework;

namespace _Tests._ProcessStartInfo
{
    [TestFixture]
    public class BindFromCommandLine
    {
        [Test]
        [TestCase(@"""C:\Program Files\test.txt"" -f -t", @"C:\Program Files\test.txt", "-f -t")]
        [TestCase(@"""C:\Program Files\test.txt""", @"C:\Program Files\test.txt", "")]
        [TestCase(@" ""C:\Program Files\test.txt"" -f -t", @"C:\Program Files\test.txt", "-f -t")]
        [TestCase(@" ""C:\Program Files\test.txt""", @"C:\Program Files\test.txt", "")]
        [TestCase(@"""C:\ProgramFiles\test.txt"" -f -t", @"C:\ProgramFiles\test.txt", "-f -t")]
        [TestCase(@"""C:\ProgramFiles\test.txt""", @"C:\ProgramFiles\test.txt", "")]
        [TestCase(@" ""C:\ProgramFiles\test.txt"" -f -t", @"C:\ProgramFiles\test.txt", "-f -t")]
        [TestCase(@" ""C:\ProgramFiles\test.txt""", @"C:\ProgramFiles\test.txt", "")]
        [TestCase("notepad -f -t", "notepad", "-f -t")]
        [TestCase("notepad", "notepad", "")]
        [TestCase(" notepad -f -t", "notepad", "-f -t")]
        [TestCase(" notepad", "notepad", "")]
        [TestCase(@"TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt -f -t", @"TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt", "-f -t")]
        [TestCase(@"TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt", @"TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt", "")]
        [TestCase(@" TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt -f -t", @"TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt", "-f -t")]
        [TestCase(@" TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt", @"TestData\_ProcessStartInfo\BindFromCommandLine\Test Data\notepad.txt", "")]
        public void コマンドラインがFileNameとArgumentsプロパティに反映される(
            string commandLine, string expectedFileName, string expectedArguments)
        {
            var actual = new ProcessStartInfo();

            actual.BindFromCommandLine(commandLine);

            Assert.AreEqual(actual.FileName, expectedFileName);
            Assert.AreEqual(actual.Arguments, expectedArguments);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void selfがnullの場合例外がスローされる()
        {
            ApplicationExtention.BindFromCommandLine(null, "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void commandLineがnullの場合例外がスローされる()
        {
            new ProcessStartInfo().BindFromCommandLine(null);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void ダブルクォーテーションが閉じていない場合例外がスローされる()
        {
            new ProcessStartInfo().BindFromCommandLine(@"""AAAA");
        }
    }
}
