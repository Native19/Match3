using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Screens
{
    class StringForDraw
    {
        private Vector2 position;
        private Color color = Color.White;
        private SpriteFont font = GameContext.ContentManager.Load<SpriteFont>("GameOver");
        private string drowingString;

        public StringForDraw(Vector2 position, string drowingString)
        {
            this.position = position;
            this.drowingString = drowingString;
        }

        public int X
        {
            get { return (int)position.X; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return (int)position.Y; }
            set { position.Y = value; }
        }

        public void DrawString(SpriteBatch batch)
        {
            batch.DrawString(this.font, this.drowingString, this.position, this.color);
        }
    }
}
