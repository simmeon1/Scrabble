using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Classes
{
    /// <summary>
    /// Used for HashSet. Objects are the same if they start with the same letter.
    /// </summary>
    public class DawgEdgeEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (x[0] == y[0]) return true;
            return false;
        }

        public int GetHashCode(string obj)
        {
            if (obj == "") return 0;
            return obj[0];
        }
    }
}
