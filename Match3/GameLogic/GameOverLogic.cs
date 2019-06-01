using System.Linq;
using Match3.GameLogic.Initialize;
using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework.Input;
using Match3.Screens;
using Microsoft.Xna.Framework;


namespace Match3.GameLogic
{
    static class GameOverLogic
    {
        static bool isInitialized = false;
        static ActiveElement okButton = new ActiveElement();

        public static void OverLogic(MouseState lastMouseState, GameTime gameTime, int score)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                MainScreen.UpdateStaticImageList(GameOver.InitializeGameOver(ref okButton)
                    .Select(item => new Image(item.Texture, new Point(item.Position.X, item.Position.Y)))
                    .ToList<Image>());
                MainScreen.UpdateDrawListOfStrings(GameOver.InitializeGameOverStrings(score));
            }
            if (okButton.IsPresed(lastMouseState))
            {
                SelectedController.UnselectElements();
                MoveController.movingElementsList.Clear();
                ScreenMenager.ScreenName = "MainMenu";
                isInitialized = false;
                MainScreen.Clear();
                return;
            }
        }
    }
}
