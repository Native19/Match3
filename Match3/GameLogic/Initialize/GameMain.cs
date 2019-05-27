using System.Collections.Generic;
using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework;
using Match3.Screens;
using Match3.GameObjects;

namespace Match3.GameLogic.Initialize
{
    static class GameMain
    {
        public static List<GameElement> InitializeGameMain()
        {
            GameGrid.InitializeGrid(GameController.GridSize, GameController.GridStartPos, GameController.GridElementSize, GameController.GridObjectSize, GameController.NumberOfColors);
            return BackGroundObjects();
        }

        public static List<StringForDraw> InitializeGameMainStrings()
        {
            List<StringForDraw> stringForDraw = new List<StringForDraw>()
            {
                new StringForDraw(new Vector2(80, 100), "Time:" + 60),
                new StringForDraw(new Vector2(330, 100), "Score:" + 0)
            };
            return stringForDraw;
        }

        public static List<GameElement> BackGroundObjects()
        {
            List<GameElement> gameMainObjects = new List<GameElement>();
            gameMainObjects.Add(new GameElement("backGround", "staticElement"));
            for (int i = 0; i < GameController.GridSize.X; i++)
                for (int j = 0; j < GameController.GridSize.Y; j++)
                {
                    gameMainObjects.Add(
                        new GameElement("GridElement", "staticElement",
                        new Point(
                            GameController.GridStartPos.X + GameController.GridElementSize.X * i,
                            GameController.GridStartPos.Y + GameController.GridElementSize.Y * j)));
                }
            return gameMainObjects;
        }
    }
}
