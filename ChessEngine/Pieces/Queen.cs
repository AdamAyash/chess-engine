using ChessEngine.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    internal class Queen : BasePiece
    {
        public Queen(PlayerTypes playerType, Texture2D texture)
            : base(playerType, texture) 
        {
        }

        public override int[] Directions()
        {
            return new int[] { -9, -7, 7, 9, -8, 8, -1, 1 };
        }

        public override PieceTypes GetPieceType() => PieceTypes.Queen;

        public override List<Move> GenerateLegalMoves(IPiece[] boardRepresentation)
        {
            var moves = new List<Move>();

            int row = CurrentPosition / 8;
            int col = CurrentPosition % 8;

            int[] maxSteps =
            {
                Math.Min(row, col),
                Math.Min(row, 7 - col),
                Math.Min(7 - row, col),
                Math.Min(7 - row, 7 - col),
                row,
                7 - row,
                col,
                7 - col,
            };

            for (int directionIndex = 0; directionIndex < Directions().Length; directionIndex++)
            {
                int direction = Directions()[directionIndex];
                for (int stepIndex = 1; stepIndex <= maxSteps[directionIndex]; stepIndex++)
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
