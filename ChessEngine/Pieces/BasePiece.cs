namespace ChessEngine.Pieces
{
    using ChessEngine.Common;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    internal abstract class BasePiece : IPiece
    {
        public PlayerTypes PlayerType { get; set; }
        public Texture2D Texture { get; set; }
        public int CurrentPosition { get; set; }

        protected BasePiece(PlayerTypes playerType, Texture2D texture)
        {
            PlayerType = playerType;
            this.Texture = texture;
        }

        public abstract int[] Directions();

        public abstract PieceTypes GetPieceType();
        public abstract List<Move> GenerateLegalMoves(IPiece[] boardRepresentation);

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
