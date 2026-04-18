namespace ChessEngine.Common
{
    internal class Move
    {
        public int StartPosition { get; set; }

        public int EndPosition { get; set; } = -1;

        public Move()
        {
            StartPosition = -1;
            EndPosition = -1;
        }

        public Move(int startPosition, int endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}
