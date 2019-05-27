using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.GameObjects.Elements
{
    class GameElement
    {
        private Texture2D texture;
        private string elementType;
        private Rectangle position;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public void TextureSet (string textureName)
        {
            Texture = GameContext.ContentManager.Load<Texture2D>(textureName);
        }

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Position_X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Position_Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public string ElementType
        {
            get { return elementType; }
            set { elementType = value; }
        }

        public GameElement(string textureName, string elementType)
        {
            this.elementType = elementType;
            TextureSet(textureName);
            Position = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }

        public GameElement(string textureName, string elementType, Point startPos)
        {
            this.elementType = elementType;
            TextureSet(textureName);
            Position = new Rectangle(startPos.X, startPos.Y, Texture.Width, Texture.Height);
        }

        public GameElement()
        {

        }
    }
}
