using Scrabble.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Classes
{
    public class BoardTileCrossCheckEqualityComparer : IEqualityComparer<BoardTileCrossCheck>
    {
        public bool Equals(BoardTileCrossCheck x, BoardTileCrossCheck y)
        {
            if (x.Tile == y.Tile && x.WordAttachedTo == y.WordAttachedTo)
            {
                return true;
            }
            else return false;
        }

        public int GetHashCode(BoardTileCrossCheck obj)
        {
            return obj.Tile.GetHashCode() + obj.WordAttachedTo.GetHashCode();
        }
    }
}
