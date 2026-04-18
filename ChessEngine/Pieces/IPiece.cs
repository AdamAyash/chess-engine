using ChessEngine.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    internal interface IPiece
    {
        public Vector2 WindowPosition { get; set; }
        public PlayerTypes PlayerType { get; set; }
        public int CurrentPosition { get; set; }
        public Texture2D Texture { get; set; }
        public PieceTypes GetPieceType();
        public List<Move> GenerateLegalMoves(IPiece[] boardRepresentation);
    }
}
