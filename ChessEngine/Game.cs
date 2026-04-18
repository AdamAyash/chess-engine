using ChessEngine.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ChessEngine
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Board _chessBoard;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.AllowUserResizing = true;

            this._chessBoard = new Board();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _chessBoard.LightSquareTexture = Content.Load<Texture2D>("Board/LightSquareTexture");
            _chessBoard.DarkSquareTexture = Content.Load<Texture2D>("Board/DarkSquareTexture");

            _chessBoard.WhitePawnTexture = Content.Load<Texture2D>("Pieces/WhitePawn");
            _chessBoard.BlackPawnTexture = Content.Load<Texture2D>("Pieces/BlackPawn");

            _chessBoard.WhiteRookTexture = Content.Load<Texture2D>("Pieces/WhiteRook");
            _chessBoard.BlackRookTexture = Content.Load<Texture2D>("Pieces/BlackRook");

            _chessBoard.WhiteKnightTexture = Content.Load<Texture2D>("Pieces/WhiteKnight");
            _chessBoard.BlackKnightTexture = Content.Load<Texture2D>("Pieces/BlackKnight");

            _chessBoard.WhiteBishopTexture = Content.Load<Texture2D>("Pieces/WhiteBishop");
            _chessBoard.BlackBishopTexture = Content.Load<Texture2D>("Pieces/BlackBishop");

            _chessBoard.WhiteKingTexture = Content.Load<Texture2D>("Pieces/WhiteKing");
            _chessBoard.BlackKingTexture = Content.Load<Texture2D>("Pieces/BlackKing");

            _chessBoard.WhiteQueenTexture = Content.Load<Texture2D>("Pieces/WhiteQueen");
            _chessBoard.BlackQueenTexture = Content.Load<Texture2D>("Pieces/BlackQueen");

            AudioManager.AddSoundEffect("movePiece", Content.Load<SoundEffect>("Audio/move"));
            AudioManager.AddSoundEffect("gameStart", Content.Load<SoundEffect>("Audio/gameStart"));
            AudioManager.AddSoundEffect("check", Content.Load<SoundEffect>("Audio/check"));

            _chessBoard.PopulateBoard();
            AudioManager.PlaySoundEffect("gameStart");
        }

        protected override void Update(GameTime gameTime)
        {
            _chessBoard.WindowWidth = GraphicsDevice.Viewport.Width;
            _chessBoard.WindowHeight = GraphicsDevice.Viewport.Height;

            _chessBoard.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin();

            _chessBoard.Draw(_spriteBatch);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
