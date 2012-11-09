using System;
using System.Linq;
using csFiler;
using NUnit.Framework;

namespace _Tests._String
{
    [TestFixture]
    public class IndexOfAll
    {
        [TestCase("a b c", ' ',new int[] { 1, 3 })]
        [TestCase("a", ' ', new int[] { })]
        public void 全てのインデックスが取得できる(string target, char separator, int[] expected)
        {
            var actuals = target.IndexOfAll(separator);

            Assert.AreEqual(expected.Length, actuals.Count());

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actuals[i]);
            }
        }

        [ExpectedException(typeof(ArgumentNullException))]
        public void selfがnullの場合例外がスローされる()
        {
            ApplicationExtention.IndexOfAll(null, 'a');
        }
    }
}
