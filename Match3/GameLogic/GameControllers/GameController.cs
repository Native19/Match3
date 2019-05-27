using System.Collections.Generic;
using System.Linq;
using Match3.GameLogic.Initialize;
using Match3.GameObjects;
using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework.Input;
using Match3.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.GameLogic
{
    static class GameController // Основная логика игры
    {
        static private Point gridSize = new Point(8, 8);
        static private Point gridStartPos = new Point(60, 150);
        static private Point gridElementSize = new Point(60, 60);
        static private Point gridObjectSize = new Point(46, 46);
        static private int numberOfColors = 5;
        static private float gameTimeCounter = 60f;
        static private int score = 0;
        static public GameTime nowGameTime;

        static public int Score
        {
            get => score;
            set => score = value;
        }

        static public Point GridSize
        {
            get => gridSize;
            set => gridSize = value;
        }

        static public Point GridStartPos
        {
            get => gridStartPos;
        }

        static public Point GridElementSize
        {
            get => gridElementSize;
        }

        static public Point GridObjectSize
        {
            get => gridObjectSize;
        }

        static public int NumberOfColors
        {
            get => numberOfColors;
        }

        static bool isInitialized = false;
        static List<GameElement> gameOjects = new List<GameElement>();
        static List<ActiveElement> gameOjectsActive = new List<ActiveElement>();
        static List<Destroyer> gameDestroersList = new List<Destroyer>();
        static List<Image> destroersImage = new List<Image>();

        public static void GameMainLogic(MouseState lastMouseState, GameTime gameTime)
        {
            nowGameTime = gameTime;
            gameTimeCounter -= (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            if (gameTimeCounter <= 0)
            {
                MoveController.swap = false;
                MoveController.movingDestroyersList.Clear();
                gameTimeCounter = 60f;
                GameGrid.GameStart = false;
                SelectedController.UnselectElements();
                MoveController.movingElementsList.Clear();
                MoveController.movingDestroyersList.Clear();
                ScreenMeneger.ScreenName = "GameOver";
                isInitialized = false;
                MainScreen.Clear();
                return;
            }

            if (!isInitialized)
            {
                Score = 0;
                isInitialized = true;
                MainScreen.UpdateStaticImageList(GameMain.InitializeGameMain()
                    .Select(item => new Image(item.Texture, new Point(item.Position.X, item.Position.Y)))
                    .ToList<Image>());
                MainScreen.UpdateDrawListOfStrings(GameMain.InitializeGameMainStrings());
                return;
            }
            MoveController.ElementsMove(gameTime);
            gameOjectsActive = GameGrid.GridToGameObjects();
            destroersImage.Clear();

            GameGrid.ElementSelected(lastMouseState);
            foreach (Destroyer item in GameGrid.DestroyersToGameObjects())
            {
                switch (item.Direction) // вращение пакмена
                {
                    case "Up":
                        destroersImage.Add(new Image(item.Texture, new Point(item.PositionNow.X + 60, item.PositionNow_Y), null, MathHelper.PiOver2, Vector2.Zero, SpriteEffects.FlipVertically));
                        break;
                    case "Down":
                        destroersImage.Add(new Image(item.Texture, new Point(item.PositionNow.X + 60, item.PositionNow_Y), null, MathHelper.PiOver2, Vector2.Zero, SpriteEffects.FlipHorizontally));
                        break;
                    case "Left":
                        destroersImage.Add(new Image(item.Texture, new Point(item.PositionNow.X, item.PositionNow_Y), null, 0,  Vector2.Zero , SpriteEffects.None));
                        break;
                    case "Right":
                        destroersImage.Add(new Image(item.Texture, new Point(item.PositionNow.X, item.PositionNow_Y), null,  0, Vector2.Zero, SpriteEffects.FlipHorizontally));
                        break;
                }
            };

            MainScreen.UpdateActiveImageList(gameOjectsActive
                    .Select(item => new Image(item.Texture, new Point(item.PositionNow.X, item.PositionNow_Y)))
                    .ToList<Image>());

            MainScreen.UpdateDestroyersImageList(new List<Image>(destroersImage));

            MainScreen.UpdateBombImageList(GameGrid.BombToGameObjects()
                    .Select(item =>
                    new Image(item.Texture, new Point(item.PositionNow.X, item.PositionNow_Y), item.GetSourceRectangle(), 0, Vector2.Zero, SpriteEffects.None))
                    .ToList());
            
            MainScreen.UpdateDrawListOfStrings(
                            new List<StringForDraw>(){
                    new StringForDraw(new Vector2(80, 100), "Time:" + (int)gameTimeCounter),
                    new StringForDraw(new Vector2(330, 100), "Score:" + score)}
                );
        }
    }
}
