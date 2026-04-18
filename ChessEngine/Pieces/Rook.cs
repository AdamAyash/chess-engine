using ChessEngine.Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    internal class Rook : BasePiece
    {
        public Rook(PlayerTypes playerType, Texture2D texture)
            : base(playerType, texture)
        {
        }

        public override PieceTypes GetPieceType() => PieceTypes.Rook;

        public override int[] Directions()
        {
            return new int[] { -8, 8, -1, 1 };
        }

        public override List<Move> GenerateLegalMoves(IPiece[] boardRepresentation)
        {
            var moves = new List<Move>();

            int row = CurrentPosition / 8;
            int col = CurrentPosition % 8;

            int[] maxSteps =
            {
                row,
                7 - row,
                col,
                7 - col
            };

            for(int directionIndex = 0; directionIndex < Directions().Length; directionIndex++)
            {
                int direction = Directions()[directionIndex];
                for(int stepIndex = 1;  stepIndex <= maxSteps[directionIndex]; stepIndex++)
                {
                    int newPosiition = CurrentPosition + (direction * stepIndex);
                    IPiece targetPiece = boardRepresentation[newPosiition];

                    if (targetPiece == null)
                    {
                        moves.Add(new Move(CurrentPosition, newPosiition));
                    }
                    else if (targetPiece.PlayerType != this.PlayerType)
                    {
                        moves.Add(new Move(CurrentPosition, newPosiition));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return moves;
        }
    }
}
