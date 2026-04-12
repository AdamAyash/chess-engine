namespace ChessEngine.Entities.Pieces
{
    internal struct Move
    {
        public int StartPosition { get; set; }

        public int EndPosition { get; set; } = -1;

        public bool TakePiece { get; set; }

        public Move(int startPosition, int endPosition, bool takePiece = false)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            TakePiece = takePiece;
        }
    }
}
