using Scrabble.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Classes
{
    public class GeneratedMoveEqualityComparer : IEqualityComparer<GeneratedMove>
    {
        public bool Equals(GeneratedMove x, GeneratedMove y)
        {
            if (x.Word.Equals(y.Word) && x.StartIndex == y.StartIndex && x.EndIndex == y.EndIndex
                && x.Anchor == y.Anchor && x.IsHorizontal == y.IsHorizontal
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
