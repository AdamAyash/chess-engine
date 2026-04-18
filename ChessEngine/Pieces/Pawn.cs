using ChessEngine.Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ChessEngine.Pieces
{
    internal class Pawn : BasePiece
    {
        public Pawn(PlayerTypes playerType, Texture2D texture)
            : base(playerType, texture)
        {
        }
        public override int[] Directions()
        {
            return new int[] { };
        }

        public override PieceTypes GetPieceType() => PieceTypes.Pawn;

        public override List<Move> GenerateLegalMoves(IPiece[] boardRepresentation)
        {
            bool isFirstMove = false;
            if (PlayerType == PlayerTypes.Black && CurrentPosition > 7 && CurrentPosition < 16)
                isFirstMove = true;
            else if (PlayerType == PlayerTypes.White && CurrentPosition > 46 &&  CurrentPosition < 56)
                isFirstMove = true;

            var moves = new List<Move>();

            int step = PlayerType == PlayerTypes.White ? (8 * (-1)) : 8;

            int enPassantStep1 = PlayerType == PlayerTypes.White ? (7 * (-1)) : 7;
            int enPassantStep2 = PlayerType == PlayerTypes.White ? (9 * (-1)) : 9;

            int newEndPosition = -1;

            newEndPosition = CurrentPosition + step;

            if (newEndPosition > 0 && newEndPosition < 64)
             {
                if (boardRepresentation[newEndPosition] == null)
                {
                    moves.Add(new Move(CurrentPosition, newEndPosition));

                    if (isFirstMove)
                        moves.Add(new Move(CurrentPosition, newEndPosition + step));
                }

                newEndPosition = CurrentPosition + enPassantStep1;
                if (boardRepresentation[newEndPosition] != null && boardRepresentation[newEndPosition].PlayerType != PlayerType)
                    moves.Add(new Move(CurrentPosition, newEndPosition));

                newEndPosition = CurrentPosition + enPassantStep2;
                if (boardRepresentation[newEndPosition] != null && boardRepresentation[newEndPosition].PlayerType != PlayerType)
                    moves.Add(new Move(CurrentPosition, newEndPosition));
            }

            return moves;
        }
    }
}
