using ChessEngine.Entities.Pieces;
using ChessEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine.Entities.Board
{
    public enum PlayerTypes
    {
        Black = 0,
        White = 1,
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

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public Texture2D DarkSquareTexture { get; set; }
        public Texture2D LightSquareTexture { get; set; }

        public Texture2D WhitePawnTexture { get; set; }
        public Texture2D BlackPawnTexture { get; set; }

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

        private void MovePiece(Move currentMove)
        {
            _boardInternalRepresentation[currentMove.EndPosition] = _selectedPiece;
            _boardInternalRepresentation[currentMove.StartPosition] = null;
            _selectedPiece.CurrentPosition = currentMove.EndPosition;
        }

        public void Update()
        {
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

                            Move currentMove = _moves.Where(move => move.EndPosition == currentSquareIndex).FirstOrDefault();
                            if (_selectedPiece != null && currentMove.EndPosition > 0)
                            {
                                this.MovePiece(currentMove);
                                this.ChangePlayerTurn();
                                this.Unselect();
                                break;
                            }
                        }
                        currentSquareIndex++;
                    }
                }
            }

            InputManager.UpdatePreviousState();
        }

        public void PopulateBoard()
        {
            int boardSquareIndex = 0;
            const string FENStarting = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

            var characters = FENStarting.ToCharArray();
            foreach (var symbol in characters)
            {
                IPiece piece = null;

                if (symbol.Equals('P'))
                    piece = new Pawn(PlayerTypes.White, WhitePawnTexture);
                else if (symbol.Equals('p'))
                    piece = new Pawn(PlayerTypes.Black, BlackPawnTexture);
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
                        Move currentMove = _moves.Where(move => move.EndPosition == counter).FirstOrDefault();
                        if (currentMove.EndPosition > 0)
                            color = Color.LightSkyBlue;
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
