using ChessEngine.Entities.Board;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace ChessEngine.Entities.Pieces
{
    internal interface IPiece
    {
        public PlayerTypes PlayerType { get; set; }
        public int CurrentPosition { get; set; }
        public Texture2D Texture { get; set; }
        public PieceTypes GetPieceType();
        public List<Move> GenerateLegalMoves(IPiece[] boardRepresentation);
    }
}
