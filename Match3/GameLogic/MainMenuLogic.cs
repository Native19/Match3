using System.Linq;
using Match3.GameLogic.Initialize;
using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework.Input;
using Match3.Screens;
using Microsoft.Xna.Framework;


namespace Match3.GameLogic
{
    static class MainMenuLogic
    {
        static bool isInitialized = false;
        static ActiveElement playButton = new ActiveElement();

        public static void MainLogic(MouseState lastMouseState, GameTime gameTime)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                MainScreen.UpdateStaticImageList(
                    MainMenu.InitializeMainMenu(ref playButton)
                    .Select(item => new Image(item.Texture, new Point(item.Position_X, item.Position_Y)))
                    .ToList());
            }
            if (playButton.IsPresed(lastMouseState))
            {
                SelectedController.UnselectElements();
                MoveController.movingElementsList.Clear();
                ScreenMeneger.ScreenName = "GameMain";
                isInitialized = false;
                MainScreen.Clear();
                return;
            }
        }
    }
}
