using Microsoft.Xna.Framework;

namespace Match3.GameObjects.Elements
{
    class Destroyer : ActiveElement
    {
        string direction;
        string textureName;
        string[] textureNameMap = new string[] { "Destroyer1", "Destroyer2" };

        public Destroyer(string textureName, string elementType, Point startPos, Point positionNow, string direction) : base(textureName, elementType, startPos)
        {
            this.textureName = textureName;
            this.direction = direction;
            PositionNow = new Rectangle(positionNow.X, positionNow.Y, Position.Width, Position.Height);
        }

        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }
    
        public void TextureUpdate ()
        {
            if (textureName == textureNameMap[0])
                textureName = textureNameMap[1];
            else
                textureName = textureNameMap[0];
            this.TextureSet(textureName);
        }
    }
}
