using ChessEngine.Common;
using ChessEngine.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.AI
{
    internal class ChessAI
    {
        public PlayerTypes OponentType { get; set; }

        public ChessAI(PlayerTypes _oponentType)
        {
            this.OponentType = _oponentType;
        }

        public void GenerateBestMove(IPiece[] boardRepresentation)
        {

        }
    }
}
