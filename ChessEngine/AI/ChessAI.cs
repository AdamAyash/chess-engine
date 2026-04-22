using ChessEngine.Common;
using ChessEngine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.AI
{
    internal class ChessAI
    {
        public PlayerTypes OponentType { get; set; }

        private Stack<IPiece[]> _boardSnapshot = new Stack<IPiece[]>();

        public ChessAI(PlayerTypes _oponentType)
        {
            this.OponentType = _oponentType;
        }

        private int HeuristicEval(IPiece[] board, PlayerTypes playerType)
        {
            var whiteMaterialScore = board.Where(p => p is not null && p.PlayerType == PlayerTypes.White).Sum(p => (int)p.GetPieceType());
            var blackMaterialScore = board.Where(p => p is not null && p.PlayerType == PlayerTypes.Black).Sum(p => (int)p.GetPieceType());

            if (playerType == PlayerTypes.White)
                return whiteMaterialScore - blackMaterialScore;

            return blackMaterialScore - whiteMaterialScore;
        }
        private bool IsKingInCheck(PlayerTypes playerType, IPiece[] board)
        {
            var enemyPieces = board.Where(p => p is not null && p.PlayerType != playerType);
            var enemyMoves = new List<Move>();

            foreach (var piece in enemyPieces)
                enemyMoves.AddRange(piece.GenerateLegalMoves(board));

            IPiece king = board.Where(p => p is not null &&
            p.PlayerType == playerType && p.GetPieceType() == PieceTypes.King).FirstOrDefault();

            return enemyMoves.Any(m => m.EndPosition == king.CurrentPosition);
        }

        private IPiece[] MakeSnapshot(IPiece[] board)
        {
            var boardSnapshot = new IPiece[64];

            for (int index = 0; index < board.Length; index++)
            {
                if (board[index] != null)
                    boardSnapshot[index] = (IPiece)board[index].Clone();
            }

            return boardSnapshot;
        }

        private void SimulateMove(Move move, IPiece[] boardRepresentation)
        {
            IPiece piece = boardRepresentation[move.StartPosition];
            boardRepresentation[move.EndPosition] = piece;
            boardRepresentation[move.StartPosition] = null;
            piece.CurrentPosition = move.EndPosition;
        }

        private bool HasAnyLegalMoves(PlayerTypes playerType, IPiece[] boardRepresentation)
        {
            var pieces = boardRepresentation.Where(p => p is not null && p.PlayerType == playerType);
            var moves = new List<Move>();

            foreach (var piece in pieces)
                moves.AddRange(piece.GenerateLegalMoves(boardRepresentation));

            foreach (var move in moves)
            {
                var boardSapshot = MakeSnapshot(boardRepresentation);
                _boardSnapshot.Push(boardSapshot);
                SimulateMove(move, boardSapshot);
                if (!IsKingInCheck(playerType, boardSapshot))
                {
                    return true;
                }
            }

            return false;
        }

        private int MiniMax(int depth, IPiece[] boardRepresentation, int alpha, int beta, bool isMaximizer, PlayerTypes playerType)
        {
            if (depth == 0 || IsKingInCheck(playerType, boardRepresentation) && !HasAnyLegalMoves(playerType, boardRepresentation))
                return HeuristicEval(boardRepresentation, OponentType);

            PlayerTypes nextPlayer = playerType == PlayerTypes.White ? PlayerTypes.Black : PlayerTypes.White;

            if (isMaximizer)
            {
                int maxEvaluation = int.MinValue;

                var pieces = boardRepresentation.Where(p => p is not null && p.PlayerType == nextPlayer);
                var moves = new List<Move>();

                foreach (var piece in pieces)
                    moves.AddRange(piece.GenerateLegalMoves(boardRepresentation));

                foreach (var currentMove in moves)
                {
                    var boardSapshot = MakeSnapshot(boardRepresentation);
                    _boardSnapshot.Push(boardSapshot);
                    SimulateMove(currentMove, boardSapshot);
                    maxEvaluation = Math.Max(maxEvaluation, MiniMax(depth - 1, boardRepresentation, alpha, beta, false, nextPlayer));
                    alpha = Math.Max(alpha, maxEvaluation);
                    if (beta <= alpha)
                        break;
                }

                return maxEvaluation;
            }
            else
            {
                int minEvaluation = int.MaxValue;
                var pieces = boardRepresentation.Where(p => p is not null && p.PlayerType == nextPlayer);
                var moves = new List<Move>();

                foreach (var piece in pieces)
                    moves.AddRange(piece.GenerateLegalMoves(boardRepresentation));

                foreach (var currentMove in moves)
                {
                    var boardSapshot = MakeSnapshot(boardRepresentation);
                    _boardSnapshot.Push(boardSapshot);
                    SimulateMove(currentMove, boardSapshot);
                    minEvaluation = Math.Min(minEvaluation, MiniMax(depth - 1, boardRepresentation, alpha, beta, true, nextPlayer));
                    beta = Math.Min(beta, minEvaluation);
                    if (beta <= alpha)
                        break;
                }
                return minEvaluation;
            }
        }

        public Move GenerateBestMove(IPiece[] boardRepresentation)
        {
            var pieces = boardRepresentation.Where(p => p is not null && p.PlayerType == OponentType);
            var moves = new List<Move>();

            foreach (var piece in pieces)
                moves.AddRange(piece.GenerateLegalMoves(boardRepresentation));

            int bestEvaluation = int.MinValue;
            Move bestMove = null;

            foreach (var move in moves)
            {
                var boardSapshot = MakeSnapshot(boardRepresentation);
                _boardSnapshot.Push(boardSapshot);
                SimulateMove(move, boardSapshot);
                int currentEvaluation = MiniMax(4, boardSapshot, int.MinValue, int.MaxValue, false, OponentType);
                if (currentEvaluation > bestEvaluation)
                {
                    bestEvaluation = currentEvaluation;
                    bestMove = move;
                }
            }

            return bestMove;
        }
    }
}
