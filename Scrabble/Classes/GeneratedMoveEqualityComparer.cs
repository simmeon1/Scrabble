using Scrabble.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Classes
{
    /// <summary>
    /// Used for HashSet. Generated moves are the same if all their details are the same as an existing one.
    /// </summary>
    public class GeneratedMoveEqualityComparer : IEqualityComparer<GeneratedMove>
    {
        public bool Equals(GeneratedMove x, GeneratedMove y)
        {
            if (x.Word.Equals(y.Word) && x.StartIndex == y.StartIndex && x.EndIndex == y.EndIndex
                && x.Anchor[0] == y.Anchor[0] && x.Anchor[1] == y.Anchor[1] && x.IsHorizontal == y.IsHorizontal
                && x.Score == y.Score)
            {
                return true;
            }
            else return false;
        }

        public int GetHashCode(GeneratedMove obj)
        {
            return obj.StartIndex * 1337 + obj.EndIndex * 337 + obj.EndIndex * 37 + obj.Score * 7;
        }
    }
}
