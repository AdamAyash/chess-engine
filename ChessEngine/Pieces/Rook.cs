using ChessEngine.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    internal class Rook : IPiece
    {
        public bool IsHeld { get; set; }
        public Vector2 WindowPosition { get; set; }
        public PlayerTypes PlayerType { get; set; }
        public Texture2D Texture { get; set; }
        public int CurrentPosition { get; set; }
        public Rectangle Collider { get; set; }

        public Rook(PlayerTypes playerType, Texture2D texture)
        {
            PlayerType = playerType;
            this.Texture = texture;
        }

        public PieceTypes GetPieceType() => PieceTypes.Pawn;

        public List<Move> GenerateLegalMoves(IPiece[] boardRepresentation)
        {
            var moves = new List<Move>();

            for(int direction = 0; direction < 8; direction++)
            {

            }

            return moves;
        }
    }
}
