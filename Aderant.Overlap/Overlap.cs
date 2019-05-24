using System;
using System.Collections.Generic;
using System.Linq;

namespace Aderant.Overlap
{
    public class Overlap : IDisposable
    {
        private List<string> fragmentStrings = null;
        private bool disposed = false;

        #region Constructors
        /// <summary>
        /// construct a object by a string
        /// </summary>
        /// <param name="fragments"></param>
        public Overlap(string fragments)
        {
            fragmentStrings = fragments == null? null : fragments.Split('\n').ToList();
        }
        /// <summary>
        /// construct a object by an array of string
        /// </summary>
        /// <param name="fragments"></param>
        public Overlap(List<string> fragments)
        {
            fragmentStrings = fragments;
        }
        #endregion

        #region functions
        /// <summary>
        /// find out the overlap of between s1 and s2 by the length. If they are overlaped, empty one of them.
        /// </summary>
        /// <param name="index1">position of string1 in fragments</param>
        /// <param name="index2">position of string2 in fragments</param>
        /// <param name="length">detect depth</param>
        public void FindAndMergeOverlap(List<string> fragments, int index1, int index2, int length)
        {
            var s1 = fragments[index1];
            var s2 = fragments[index2];
            if (s1.Contains(s2))
            {
                fragmentStrings[index2] = string.Empty;
                return;
            }
            if (s2.Contains(s1))
            {
                fragmentStrings[index1] = string.Empty;
                return;
            }

            // s1.begin == s2.end
            if(s1.Substring(0, length).Equals(s2.Substring(s2.Length - length)))
            {
                fragmentStrings[index2] += s1.Substring(length);
                fragmentStrings[index1] = string.Empty;
                return;
            }
            // s1.end == s2.begin
            if(s1.Substring(s1.Length - length).Equals(s2.Substring(0, length)))
            {
                fragmentStrings[index1] += s2.Substring(length);
                fragmentStrings[index2] = fragmentStrings[index1];
                fragmentStrings[index1] = string.Empty;
                return;
            }
        }

        /// <summary>
        /// For given length, Loop through each possible string pair
        /// </summary>
        /// <param name="tryLength">the length that is going to test</param>
        private void FindOverlap(int tryLength)
        {
            // index1: from 0 to (n-1)
            for(int index1 = 0; index1 < fragmentStrings.Count - 1; index1++)   //O(n)
            {
                if(fragmentStrings[index1].Length < tryLength)
                {
                    continue;
                }

                // index2: from 1 to n
                for (int index2 = index1 + 1; index2 < fragmentStrings.Count; index2++)  //O(n)
                {
                    if(fragmentStrings[index2].Length < tryLength)
                    {
                        continue;
                    }
                    FindAndMergeOverlap(fragmentStrings, index1, index2, tryLength);
                }
            }
        }

        /// <summary>
        /// Sort Fragment array by element's length descendly
        /// </summary>
        private void SortFragmentsByFragmentLength()
        {
            var fragments = fragmentStrings.OrderByDescending(x => x.Length).ToList();
            fragmentStrings = fragments;
        }
        /// <summary>
        /// the entrance of program
        /// </summary>
        /// <returns></returns>
        public string MergeOverlap()
        {
            if(fragmentStrings == null || fragmentStrings.Count == 0)
            {
                return string.Empty;
            }

            if(fragmentStrings.Count == 1)
            {
                return fragmentStrings[0];
            }
            SortFragmentsByFragmentLength();
            var tryLength = fragmentStrings[0].Length;

            // Decreasing from the longest possible value
            for (tryLength = fragmentStrings[1].Length; tryLength > 0; tryLength--) //O(T)
            {
                FindOverlap(tryLength);
            }

            return string.Join("", fragmentStrings);
        } 
        #endregion

        /// <summary>
        /// Implementation for interface of Disposable
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if(disposing)
            {
                if(fragmentStrings != null)
                {
                    fragmentStrings = null;
                }
            }
            disposed = true;
            return;
        }
    }
}
