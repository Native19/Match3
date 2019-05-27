using System.Collections.Generic;
using Match3.GameObjects.Elements;

namespace Match3.GameLogic.Initialize
{
    static class MainMenu
    {
        public static List<GameElement> InitializeMainMenu(ref ActiveElement playButton)
        {
            List<GameElement> mainMenuOjects = new List<GameElement>();
            playButton = new ActiveElement("PlayButton", "button");
            playButton.Position_X = GameContext.Graphics.PreferredBackBufferWidth / 2 - playButton.Position.Width / 2;
            playButton.Position_Y = GameContext.Graphics.PreferredBackBufferHeight / 2 - playButton.Position.Height / 2;
            GameElement backGround = new GameElement ("BackGround", "staticElement");
            mainMenuOjects.Add(backGround);
            mainMenuOjects.Add(playButton);
            return mainMenuOjects;
        }
    }
}
