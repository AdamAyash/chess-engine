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
            int newEndPosition = -1;

            for(int index = 1; index <= 8; index++)
            {
                newEndPosition = CurrentPosition + index;
                if (newEndPosition % 8 == 0)
                    break;

                if (boardRepresentation[newEndPosition] == null)
                    moves.Add(new Move(CurrentPosition, newEndPosition));

                newEndPosition = CurrentPosition - index;
                if (newEndPosition % 8 == 0)
                    break;

                if (boardRepresentation[newEndPosition] == null)
                    moves.Add(new Move(CurrentPosition, newEndPosition));
            }

            return moves;
        }
    }
}
