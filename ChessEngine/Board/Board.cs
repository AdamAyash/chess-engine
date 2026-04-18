using ChessEngine.Audio;
using ChessEngine.Common;
using ChessEngine.Input;
using ChessEngine.Pieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    enum GameStates
    {
        Normal,
        InCheck,
        Over
    }

    internal class Board
    {
        private const int _boardSize = 8;
        private const int _squareSize = 100;

        private int _centerBoardOffset;
        private PlayerTypes _playerTurn;
        private Vector2 _centerScreenPosition;

        private List<IPiece> _pieces = new List<IPiece>();
        private IPiece[] _boardInternalRepresentation;
        private List<Move> _moves = new List<Move>();

        private IPiece _selectedPiece = null;

        private GameStates _currentGameSate = GameStates.Normal;
        private IPiece _endPositionPiecePrevious;
        private IPiece _startPositionPiecePrevious;

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public Texture2D DarkSquareTexture { get; set; }
        public Texture2D LightSquareTexture { get; set; }

        public Texture2D WhitePawnTexture { get; set; }
        public Texture2D BlackPawnTexture { get; set; }
        public Texture2D WhiteRookTexture { get; set; }
        public Texture2D BlackRookTexture { get; set; }
        public Texture2D WhiteKnightTexture { get; set; }
        public Texture2D BlackKnightTexture { get; set; }
        public Texture2D WhiteBishopTexture { get; set; }
        public Texture2D BlackBishopTexture { get; set; }
        public Texture2D WhiteKingTexture { get; set; }
        public Texture2D BlackKingTexture { get; set; }
        public Texture2D WhiteQueenTexture { get; set; }
        public Texture2D BlackQueenTexture { get; set; }

        public Board()
        {
            this._playerTurn = PlayerTypes.White;
            this._centerScreenPosition = Vector2.Zero;
            this._boardInternalRepresentation = new IPiece[_boardSize * _boardSize];
            this._pieces = new();
            this._moves = new List<Move>();
        }
        private void ChangePlayerTurn()
        {
            if (_playerTurn == PlayerTypes.White)
                _playerTurn = PlayerTypes.Black;
            else
                _playerTurn = PlayerTypes.White;
        }

        private void Unselect()
        {
            _selectedPiece = null;
            _moves.Clear();
        }
        private bool MovePiece(Move currentMove)
        {
            var previous = _boardInternalRepresentation[currentMove.EndPosition];

            _boardInternalRepresentation[currentMove.EndPosition] = _selectedPiece;
            _boardInternalRepresentation[currentMove.StartPosition] = null;
            _selectedPiece.CurrentPosition = currentMove.EndPosition;

            AudioManager.PlaySoundEffect("movePiece");

            return true;
        }

        private bool IsKingInCheck()
        {
            var enemyPieces = _boardInternalRepresentation.Where(p => p is not null && p.PlayerType != _playerTurn);
            var enemyMoves = new List<Move>();

            foreach (var piece in enemyPieces)
                enemyMoves.AddRange(piece.GenerateLegalMoves(_boardInternalRepresentation));

            IPiece king = _boardInternalRepresentation.Where(p => p is not null &&
            p.PlayerType == _playerTurn && p.GetPieceType() == PieceTypes.King).FirstOrDefault();

            return enemyMoves.Any(m => m.EndPosition == king.CurrentPosition);
        }

        private void SimulateMove(Move currentMove)
        {
            _selectedPiece = _boardInternalRepresentation[currentMove.StartPosition];

            _endPositionPiecePrevious = _boardInternalRepresentation[currentMove.EndPosition];
            _startPositionPiecePrevious = _boardInternalRepresentation[currentMove.StartPosition];
            //var previousSelectedPiece = _selectedPiece.CurrentPosition = currentMove.EndPosition;

            _boardInternalRepresentation[currentMove.EndPosition] = _selectedPiece;
            _boardInternalRepresentation[currentMove.StartPosition] = null;

            if (_selectedPiece is not null)
                _selectedPiece.CurrentPosition = currentMove.EndPosition;
            //_selectedPiece.CurrentPosition = currentMove.EndPosition;
        }

        private void UndoSimulatedMove(Move currentMove)
        {
            if (_selectedPiece is not null)
                _selectedPiece.CurrentPosition = currentMove.StartPosition;

            _boardInternalRepresentation[currentMove.EndPosition] = _endPositionPiecePrevious;
            _boardInternalRepresentation[currentMove.StartPosition] = _selectedPiece;

            _startPositionPiecePrevious = null;
            _endPositionPiecePrevious = null;
            _selectedPiece = null;
        }

        private bool HasAnyLegalMoves()
        {
            var pieces = _boardInternalRepresentation.Where(p => p is not null && p.PlayerType == _playerTurn);
            var moves = new List<Move>();

            foreach (var piece in pieces)
                moves.AddRange(piece.GenerateLegalMoves(_boardInternalRepresentation));

            foreach (var move in moves)
            {
                SimulateMove(move);
                if (!IsKingInCheck())
                {
                    UndoSimulatedMove(move);
                    return true;
                }

                UndoSimulatedMove(move);
            }

            return false;
        }

        public void Update()
        {
            if (_currentGameSate == GameStates.Over)
                return;

            InputManager.UpdateCurrentState();

            this._centerScreenPosition = new Vector2((WindowWidth / 2), (WindowHeight / 2));
            this._centerBoardOffset = (_squareSize * _boardSize) / 2;

            if (InputManager.IsRightButtonPressed())
                Unselect();

            int currentSquareIndex = 0;
            if (InputManager.IsLeftButtonPressed())
            {
                for (int columns = 0; columns < _boardSize; columns++)
                {
                    for (int rows = 0; rows < _boardSize; rows++)
                    {
                        int xPosition = (rows * _squareSize) + ((int)_centerScreenPosition.X - _centerBoardOffset);
                        int yPosition = columns * _squareSize + ((int)_centerScreenPosition.Y - _centerBoardOffset);

                        var squareRectangle = new Rectangle(xPosition, yPosition, _squareSize, _squareSize);
                        if (squareRectangle.Contains(InputManager.MousePosition))
                        {
                            var currentPiece = _boardInternalRepresentation[currentSquareIndex];
                            if (currentPiece is not null && currentPiece.PlayerType == _playerTurn)
                            {
                                _selectedPiece = currentPiece;
                                _moves = _selectedPiece.GenerateLegalMoves(_boardInternalRepresentation);
                                break;
                            }

                            Move currentMove = _moves.Find(move => move.EndPosition == currentSquareIndex);
                            if (_selectedPiece != null && currentMove != null)
                            {
                                if (!this.MovePiece(currentMove))
                                    break;

                                this.ChangePlayerTurn();
                                this.Unselect();
                                break;
                            }
                        }
                        currentSquareIndex++;
                    }
                }
            }

            if (IsKingInCheck() && HasAnyLegalMoves())
            {
                if (_currentGameSate == GameStates.Normal)
                    AudioManager.PlaySoundEffect("check");

                _currentGameSate = GameStates.InCheck;
            }
            if (IsKingInCheck() && !HasAnyLegalMoves())
                _currentGameSate = GameStates.Over;
            if (!IsKingInCheck())
                _currentGameSate = GameStates.Normal;

            InputManager.UpdatePreviousState();
        }

        public void PopulateBoard()
        {
            int boardSquareIndex = 0;
            //const string FENStarting = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
            const string FENStarting = "6k1/5ppp/8/8/8/8/5PPP/4R1K1";

            var characters = FENStarting.ToCharArray();
            foreach (var symbol in characters)
            {
                IPiece piece = null;

                bool isPiece = char.IsLetter(symbol);
                if (isPiece)
                {
                    if (symbol.Equals('P'))
                        piece = new Pawn(PlayerTypes.White, WhitePawnTexture);
                    else if (symbol.Equals('p'))
                        piece = new Pawn(PlayerTypes.Black, BlackPawnTexture);

                    if (symbol.Equals('R'))
                        piece = new Rook(PlayerTypes.White, WhiteRookTexture);
                    else if (symbol.Equals('r'))
                        piece = new Rook(PlayerTypes.Black, BlackRookTexture);

                    if (symbol.Equals('B'))
                        piece = new Bishop(PlayerTypes.White, WhiteBishopTexture);
                    else if (symbol.Equals('b'))
                        piece = new Bishop(PlayerTypes.Black, BlackBishopTexture);

                    if (symbol.Equals('N'))
                        piece = new Knight(PlayerTypes.White, WhiteKnightTexture);
                    else if (symbol.Equals('n'))
                        piece = new Knight(PlayerTypes.Black, BlackKnightTexture);

                    if (symbol.Equals('Q'))
                        piece = new Queen(PlayerTypes.White, WhiteQueenTexture);
                    else if (symbol.Equals('q'))
                        piece = new Queen(PlayerTypes.Black, BlackQueenTexture);

                    if (symbol.Equals('K'))
                        piece = new King(PlayerTypes.White, WhiteKingTexture);
                    else if (symbol.Equals('k'))
                        piece = new King(PlayerTypes.Black, BlackKingTexture);

                }
                else if (char.IsDigit(symbol))
                {
                    int emptySpaceOffset = int.Parse(symbol.ToString());
                    boardSquareIndex += emptySpaceOffset;
                    continue;
                }
                else if (symbol.Equals('/'))
                    continue;

                if (piece != null)
                {
                    _pieces.Add(piece);
                    piece.CurrentPosition = boardSquareIndex;
                    _boardInternalRepresentation[boardSquareIndex] = piece;
                }

                boardSquareIndex++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var boardOffset = (_squareSize * _boardSize) / 2;
            Texture2D currentSquareTexture = null;

            int counter = 0;
            for (int columns = 0; columns < _boardSize; columns++)
            {
                for (int rows = 0; rows < _boardSize; rows++)
                {
                    if ((rows + columns + 1) % 2 == 0)
                        currentSquareTexture = DarkSquareTexture;
                    else
                        currentSquareTexture = LightSquareTexture;

                    int xPosition = (rows * _squareSize) + ((int)_centerScreenPosition.X - boardOffset);
                    int yPosition = columns * _squareSize + ((int)_centerScreenPosition.Y - boardOffset);

                    Color color = Color.White;

                    var piece = _boardInternalRepresentation[counter];

                    if (piece != null)
                    {
                        if (piece == _selectedPiece)
                            color = Color.LightGoldenrodYellow;
                    }

                    var squareRectangle = new Rectangle(xPosition, yPosition, _squareSize, _squareSize);

                    if (_moves.Count > 0)
                    {
                        Move currentMove = _moves.Find(move => move.EndPosition == counter);
                        if (currentMove != null)
                        {
                            if (_boardInternalRepresentation[currentMove.EndPosition] != null)
                                color = Color.Red;
                            else
                                color = Color.LightSkyBlue;
                        }
                    }

                    spriteBatch.Draw(currentSquareTexture, squareRectangle, color);

                    if (piece != null)
                        spriteBatch.Draw(piece.Texture, squareRectangle, Color.White);

                    counter++;
                }
            }
        }
    }
}
