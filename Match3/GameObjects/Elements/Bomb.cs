using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Match3.GameLogic;

namespace Match3.GameObjects.Elements
{
    class Bomb : ActiveElement
    {
        private List<Ghost> deliteList = new List<Ghost>();
        private float bombTimer = 250f;
        public Bomb(string textureName, string elementType, Point startPos, List<Ghost> deliteList) : base(textureName, elementType, startPos)
        {
            this.deliteList = deliteList;
        }

        public List<Ghost> DeliteList
        {
            get { return deliteList; }
            set { deliteList = value; }
        }

        public float BombTimer
        {
            get { return bombTimer; }
            set { bombTimer = value; }
        }

        public Rectangle GetSourceRectangle ()
        {
            Rectangle returnSourceRectangle = new Rectangle(this.Position.X, this.Position.Y, this.Position.Width, this.Position.Height) ;
            if (GameController.GridStartPos.X > this.Position.X)
                returnSourceRectangle.X = GameController.GridStartPos.X;
            if (GameController.GridStartPos.Y > this.Position.Y)
                returnSourceRectangle.Y = GameController.GridStartPos.Y;
            if (GameController.GridStartPos.X + GameController.GridSize.X * GameController.GridElementSize.X < this.Position.X + this.Position.Width)
                returnSourceRectangle.Width = GameController.GridStartPos.X + GameController.GridSize.X * GameController.GridElementSize.X - this.Position.X;
            if (GameController.GridStartPos.Y + GameController.GridSize.Y * GameController.GridElementSize.Y < this.Position.Y + this.Position.Height)
                returnSourceRectangle.Height = GameController.GridStartPos.Y + GameController.GridSize.Y * GameController.GridElementSize.Y - this.Position.Y;
            return returnSourceRectangle;
        }
    }
}
