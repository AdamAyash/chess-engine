using ChessEngine.Entities.Board;
using Microsoft.Xna.Framework;
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


            _chessBoard.PopulateBoard();
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
