using ChessEngine.Common;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    internal class Bishop : BasePiece
    {
        public Bishop(PlayerTypes playerType, Texture2D texture)
            : base(playerType, texture)
        {
        }

        public override int[] Directions()
        {
            return new int[] { -9, -7, 7, 9 };
        }

        public override PieceTypes GetPieceType() => PieceTypes.Bishop;

        public override List<Move> GenerateLegalMoves(IPiece[] boardRepresentation)
        {
            var moves = new List<Move>();

            int row = CurrentPosition / 8;
            int col = CurrentPosition % 8;

            int[] maxSteps = {
                Math.Min(row, col),        // NW
                Math.Min(row, 7 - col),    // NE
                Math.Min(7 - row, col),    // SW
                Math.Min(7 - row, 7 - col) // SE
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
