using ChessEngine.Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.Pieces
{
    internal class King : BasePiece
    {
        public King(PlayerTypes playerType, Texture2D texture)
            : base(playerType , texture)
        {
        }

        public override PieceTypes GetPieceType() => PieceTypes.King;
        public override int[] Directions() { return new int[] { -9, -7, 7, 9, -8, 8, -1, 1 }; }

        public override List<Move> GenerateLegalMoves(IPiece[] boardRepresentation)
        {
            List<IPiece> enemyPieces = boardRepresentation.Where(p => p is not null && p.PlayerType != PlayerType).ToList();
            var enemyMoves = new List<Move>();

            foreach (var piece in enemyPieces)
            {
                if (piece.GetPieceType() == PieceTypes.King)
                    continue;

                enemyMoves.AddRange(piece.GenerateLegalMoves(boardRepresentation));
            }

            var moves = new List<Move>();

            int row = CurrentPosition / 8;
            int col = CurrentPosition % 8;

            for (int index = 0; index < Directions().Length; index++)
            {
                int newPosition = CurrentPosition + Directions()[index];

                Move move = new Move(CurrentPosition, newPosition);
                if (enemyMoves.Where(m => m.EndPosition == move.EndPosition).FirstOrDefault() != null)
                    continue;

                if (newPosition >= 0 && newPosition <= 63)
                {
                    if (boardRepresentation[newPosition] == null)
                        moves.Add(move);

                    if (boardRepresentation[newPosition] != null && boardRepresentation[newPosition].PlayerType != PlayerType)
                        moves.Add(move);
                }
            }

            return moves;
        }
    }
}
