using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Match3.GameLogic
{
    static class ScreenMenager
    {
        private static string screenName = "MainMenu";

        public static string ScreenName
        {
            get { return screenName; }
            set { screenName = value; }
        }

        public static void scrinSet (MouseState lastMouseState, GameTime gameTime)
        {
            switch (screenName)
            {
                case "MainMenu":
                    MainMenuLogic.MainLogic(lastMouseState, gameTime);
                    break;
                case "GameMain":
                    GameController.GameMainLogic(lastMouseState, gameTime);
                    break;
                default:                
                    GameOverLogic.OverLogic(lastMouseState, gameTime, GameController.Score);
                    break;
            }
        }

    }
}
