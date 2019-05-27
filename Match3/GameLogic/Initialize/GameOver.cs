using System.Collections.Generic;
using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework;
using Match3.Screens;

namespace Match3.GameLogic.Initialize
{
    static class GameOver
    {
        public static List<GameElement> InitializeGameOver(ref ActiveElement okButton)
        {
            List<GameElement> gameOverOjects = new List<GameElement>();
            okButton = new ActiveElement("OkButton", "button");
            okButton.Position_X = GameContext.Graphics.PreferredBackBufferWidth / 2 - okButton.Position.Width / 2;
            okButton.Position_Y = GameContext.Graphics.PreferredBackBufferHeight / 2 - okButton.Position.Height / 2;
            GameElement backGround = new GameElement("BackGround", "staticElement");
            gameOverOjects.Add(backGround);
            gameOverOjects.Add(okButton);
            return gameOverOjects;
        }

        public static List<StringForDraw> InitializeGameOverStrings(int yourScore)
        {
            List<StringForDraw> stringForDraw = 
                new List<StringForDraw>() {
                    new StringForDraw(new Vector2(120, 150), "G A M E    O V E R"),
                    new StringForDraw(new Vector2(130, 250), "YOUR SCORE:  " + yourScore)
                };

            return stringForDraw;
        }
    }
}
