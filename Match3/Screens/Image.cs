using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Screens
{
    class Image
    {
        private Texture2D texture { get; set; }
        private Rectangle position;
        private Rectangle? sourceRectangle = null;
        private Color color { get; set; }
        private float rotation = 0;
        private Vector2 origin = Vector2.Zero;
        SpriteEffects effects = SpriteEffects.None;

        public Image(Texture2D texture, Point startPos)
        { 
            this.texture = texture;
            position.X = startPos.X;
            position.Y = startPos.Y;
            position.Width = texture.Width;
            position.Height = texture.Height;
            sourceRectangle = null;
            color = Color.White;
        }

        public Image(Texture2D texture, Point startPos, Rectangle? sourceRectangle, float rotation, Vector2 origin, SpriteEffects effects)
        {
            this.texture = texture;
            position.X = startPos.X;
            position.Y = startPos.Y;
            position.Width = texture.Width;
            position.Height = texture.Height;
            this.sourceRectangle = sourceRectangle;
            color = Color.White;
            this.rotation = rotation;
            this.origin = origin;
            this.effects = effects;
        }
    
        public int X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public int Width
        {
            get => position.Width;
            set => position.Width = value;
        }

        public int Height
        {
            get => position.Height;
            set => position.Height = value;
        }

        public Rectangle Position
        {
            get => position;
            set => position= value;
        }
        public Point takeTextureSize ()
        {
            return new Point(texture.Width, texture.Height);
        }

        public virtual void DrawImage(SpriteBatch batch)
        {
            batch.Draw(this.texture, new Vector2(this.position.X, this.position.Y), null, this.color, rotation, origin, 1f, effects, 1);
        }
    }
}
