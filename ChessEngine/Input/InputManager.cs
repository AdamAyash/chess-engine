using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessEngine.Input
{
    internal static class InputManager
    {
        private static MouseState _previousMouseState;
        private static MouseState _nextMouseState;

        public static bool IsLeftButtonPressed()
        {
            return _previousMouseState.LeftButton == ButtonState.Released && _nextMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsRightButtonPressed()
        {
            return _previousMouseState.RightButton == ButtonState.Released && _nextMouseState.RightButton == ButtonState.Pressed;
        }

        public static Point MousePosition { get { return _nextMouseState.Position; } }

        public static void UpdateCurrentState()
        {
            _nextMouseState = Mouse.GetState();
        }

        public static void UpdatePreviousState()
        {
            _previousMouseState = _nextMouseState;
        }
    }
}
