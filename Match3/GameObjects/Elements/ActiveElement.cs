using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Match3.GameObjects.Elements
{
    class ActiveElement : GameElement
    {
        private Rectangle positionNow;
        private bool isMoving = false;

        public ActiveElement(string textureName, string elementType, Point startPos) : base (textureName, elementType, startPos)
        {
            PositionNow = Position;
        }

        public ActiveElement(string textureName, string elementType) : base(textureName, elementType)
        {
            PositionNow = Position;
        }

        public Rectangle PositionNow
        {
            get { return positionNow; }
            set { positionNow = value; }
        }

        public int PositionNow_X
        {
            get { return positionNow.X; }
            set { positionNow.X = value; }
        }

        public int PositionNow_Y
        {
            get { return positionNow.Y; }
            set { positionNow.Y = value; }
        }

        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }

        public ActiveElement()
        {
        }

        public bool IsPresed(MouseState lastMouseState)
        {
            if (lastMouseState.LeftButton == ButtonState.Released &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Position.Contains(Mouse.GetState().Position) && 
                !IsMoving
                )      
            {
                return true;
            }
            return false;
        }

        public bool Move(int step)
        {
            int x = this.Position_X - PositionNow.X;
            int y = this.Position_Y - PositionNow_Y;
            double vectorLenght = Math.Sqrt(x * x + y * y);
            if (vectorLenght == 0 || vectorLenght <= step)
            {
                PositionNow_X = Position.X;
                PositionNow_Y = Position.Y;
                this.IsMoving = false;
                return true;
            }
            int dx = (int)(step * x / vectorLenght);
            int dy = (int)(step * y / vectorLenght);
            PositionNow_X = PositionNow.X + dx;
            PositionNow_Y = PositionNow.Y + dy;
            return false;
        }
    }
}
