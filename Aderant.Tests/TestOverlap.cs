using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aderant.Overlap;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Aderant.Overlap.Tests
{
    [TestClass]
    public class OverlapTests
    {
        #region TestMethod

        [TestMethod]
        public void TestWholeProcessByNullArray()
        {
            var overlap = new Overlap((List<string>) null);
            Assert.AreEqual(overlap.MergeOverlap(), string.Empty);
            overlap.Dispose();
        }

        [TestMethod]
        public void TestWholeProcessByNullString()
        {
            var overlap = new Overlap((string)null);
            Assert.AreEqual(overlap.MergeOverlap(), string.Empty);
            overlap.Dispose();
        }

        [TestMethod]
        public void TestWholeProcessByString()
        {
            var fragments = new StringBuilder();
            fragments.Append("all is well\n");
            fragments.Append("ell that en\n");
            fragments.Append("hat end\n");
            fragments.Append("t ends well\n");
            var overlap = new Overlap(fragments.ToString());

            Assert.AreEqual(overlap.MergeOverlap(), "all is well that ends well");
            overlap.Dispose();
        }

        [TestMethod]
        public void TestWholeProcessByRandomArray()
        {
            var s = @"Other collaborative online encyclopedias were attempted before Wikipedia, but none were as successful.[24] Wikipedia began as a complementary project for Nupedia, a free online English-language encyclopedia project whose articles were written by experts and reviewed under a formal process.[25] It was founded on March 9, 2000, under the ownership of Bomis, a web portal company. Its main figures were Bomis CEO Jimmy Wales and Larry Sanger, editor-in-chief for Nupedia and later Wikipedia.[26][27] Nupedia was initially licensed under its own Nupedia Open Content License, but even before Wikipedia was founded, Nupedia switched to the GNU Free Documentation License at the urging of Richard Stallman.[28] Wales is credited with defining the goal of making a publicly editable encyclopedia,[29][30] while Sanger is credited with the strategy of using a wiki to reach that goal.[31] On January 10, 2001, Sanger proposed on the Nupedia mailing list to create a wiki as a feeder project for Nupedia.[32]";
            var l = ProduceTestCase(s);
            l = Shuffle(l);
            var overlap = new Overlap(l);

            Assert.AreEqual(overlap.MergeOverlap(), s);
            overlap.Dispose();
        }

        [TestMethod]
        public void TestFindAndMergeOverlap()
        {
            var fragments = new List<string>()
            {
                "all is well",
                "ell that en",
                "hat end",
                "t ends well",
                "hat",
                "end"
            };

            var overlap = new Overlap(fragments);

            // to test 2 strings which are not relevant
            overlap.FindAndMergeOverlap(fragments, 2, 3, 2);
            Assert.AreEqual(fragments[2], "hat end");
            Assert.AreEqual(fragments[3], "t ends well");

            // to test 2 strings which are overlaped 
            overlap.FindAndMergeOverlap(fragments, 0, 1, 3);
            Assert.AreEqual(fragments[1], "all is well that en");
            Assert.AreEqual(fragments[0], "");

            // to test a string contains another
            overlap.FindAndMergeOverlap(fragments, 2, 4, 3);
            Assert.AreEqual(fragments[2], "hat end");
            Assert.AreEqual(fragments[4], "");

            // to test a string is contained by another
            overlap.FindAndMergeOverlap(fragments, 5, 2, 3);
            Assert.AreEqual(fragments[2], "hat end");
            Assert.AreEqual(fragments[5], "");
        }

        #endregion

        #region private methods

        /// <summary>
        /// Shuffle a array randomly
        /// </summary>
        /// <param name="originalList"></param>
        /// <returns>Shuffled Array</returns>
        private List<string> Shuffle(List<string> originalList)
        {
            List<string> shuffledList = new List<string>();
            Random random = new Random();

            int Length = originalList.Count;

            for(int i = 0; i < Length; i++)
            {
                int r = random.Next(0, originalList.Count - 1);
                shuffledList.Add(originalList[r]);
                originalList.RemoveAt(r);
            }

            return shuffledList;
        }

        /// <summary>
        /// Produce an array by a string to help test.
        /// </summary>
        /// <param name="s">A string</param>
        /// <returns>A string array whose elements get overlaped</returns>
        private List<string> ProduceTestCase(string s)
        {
            var l = new List<string>();
            var random = new Random();
            int pos = 0;

            while(true)
            {
                int r1 = random.Next(20, 30);
                if(pos+r1 >= s.Length)
                {
                    l.Add(s.Substring(pos));
                    break;
                }
                l.Add(s.Substring(pos, r1));
                pos += r1;

                int r2 = random.Next(10, r1);
                pos -= r2;
            }
            return l;
        }
        #endregion
    }
}
