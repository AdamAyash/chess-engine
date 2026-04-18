using ChessEngine.Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    internal class Knight : BasePiece
    {
        public Knight(PlayerTypes playerType, Texture2D texture)
            : base(playerType, texture)
        {
        }
        public override int[] Directions()
        {
            return new int[] { };
        }

        public override PieceTypes GetPieceType() => PieceTypes.Knight;

        public override List<Move> GenerateLegalMoves(IPiece[] boardRepresentation)
        {
           var moves = new List<Move>();

            int[][] pos =
            {
                new int[]{- 16 , 1 },
                new int[]{- 16 , -1 },
                new int[]{-8 , 2},
                new int[] {-8, -2},
                new int []{8, -2},
                new int []{8, 2},
                new int []{16, 1},
                new int []{16, -1}
            };

            int row = CurrentPosition / 8;
            int col = CurrentPosition  % 8;

            for(int index = 0; index < pos.Length; index++)
            {
                int newRow = row + (pos[index][0] / 8);
                int newCol = col + pos[index][1];

                if (newCol < 0 || newCol > 7)
                    continue;

                if (newRow < 0 || newRow > 7)
                    continue;

                int newPosition = CurrentPosition + pos[index][0] + pos[index][1];
                Move move = new Move(CurrentPosition, newPosition);

                if (newPosition >= 0 && newPosition <= 63)
                {
                    if (boardRepresentation[newPosition] == null)
                        moves.Add(move);

                    if(boardRepresentation[newPosition] != null && boardRepresentation[newPosition].PlayerType != PlayerType)
                        moves.Add(move);
                }
            }

            return moves;
        }
    }
}