using System;
using System.Collections.Generic;
using Match3.GameObjects;
using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework;

namespace Match3.GameLogic
{
    static class MoveController
    {
        public static List<ActiveElement> movingElementsList = new List<ActiveElement>();
        public static List<Destroyer> movingDestroyersList = new List<Destroyer>();
        private static float time = 1f;
        public static bool swap = false;
        public static bool destroer = false;
        public static bool bombIsActive = false;
        public static List<Bomb> activeBomb = new List<Bomb>();

        public static void AddElementToMovingList(ActiveElement element)
        {
            movingElementsList.Add(element);
        }

        public static bool IsFirstLineClear(Point gridPosition, Point gridCellSize, Point GridSize)
        {
            if (movingElementsList.Find(element =>
            new Rectangle(
                gridPosition.X,
                gridPosition.Y,
                gridCellSize.X * GridSize.X,
                gridCellSize.Y)
                .Contains(
                new Point(
                    element.PositionNow.X,
                    element.PositionNow_Y)
                    ))
            != null)
                return false;
            return true;
        }

        public static void DestroyerMove()
        {
            if (destroer && movingDestroyersList.Count != 0)
            {
                for (int i = 0; i < movingDestroyersList.Count; i++)
                {
                    for (int row = 0; row < GameController.GridSize.X; row++)
                        for (int col = 0; col < GameController.GridSize.Y; col++)
                            if (movingDestroyersList[i].PositionNow.Contains(GameGrid.Grid[row, col].Position.Center) && !GameGrid.Grid[row, col].Deleted)
                                GameGrid.RemoveGhost(GameGrid.Grid[row, col]);
                }
                return;
            }
            destroer = false;
            GameGrid.AffectAboveAllDelited();
        }

        public static void DestMove(GameTime gameTime)
        {
            time -= (float)gameTime.ElapsedGameTime.Milliseconds / 2;
            while (movingDestroyersList.Count != 0)
            {
                if (time < 0)
                {
                    time = 1f;
                    for (int i = 0; i < movingDestroyersList.Count; i++)
                        if (movingDestroyersList[i].Move(8))
                            movingDestroyersList.Remove(movingDestroyersList[i]);
                        else
                            if (Math.Abs(movingDestroyersList[i].Position.X + 1 - movingDestroyersList[i].PositionNow.X) % 15 < 1 ||
                                Math.Abs(movingDestroyersList[i].Position.Y + 1 - movingDestroyersList[i].PositionNow.Y) % 15 < 1)
                            movingDestroyersList[i].TextureUpdate();
                }
                else
                { return; }
            }
            GameGrid.AffectAboveAllDelited();
            GameGrid.RebuildGrid();
        }

        public static void BombIsActive(GameTime gameTime)
        {
            activeBomb.ForEach(bomb => bomb.BombTimer -= (float)gameTime.ElapsedGameTime.Milliseconds);
            for (int i = 0; i < activeBomb.Count; i++)
                if (activeBomb[i].BombTimer < 0)
                {
                    for (int item = 0; item < activeBomb[i].DeliteList.Count; item++)
                        GameGrid.RemoveGhost(activeBomb[i].DeliteList[item]);
                    activeBomb[i].DeliteList.Clear();
                    GameGrid.AffectAboveAllDelited();
                    activeBomb.Remove(activeBomb[i]);
                    i--;
                    if (activeBomb.Count == 0)
                        bombIsActive = false;
                }
                else
                    return;
        }

        public static void ElementsMove(GameTime gameTime)
        {
            time -= (float)gameTime.ElapsedGameTime.Milliseconds / 10;
            DestroyerMove();
            if (bombIsActive)
                BombIsActive(gameTime);
            while (movingElementsList.Count != 0)
                if (time < 0)
                {
                    time = 1f;
                    for (int i = 0; i < movingElementsList.Count; i++)
                        if (movingElementsList[i].Move(3))
                            movingElementsList.Remove(movingElementsList[i]);
                }
                else
                    return;

            if (swap && movingElementsList.Count == 0)
                SelectedController.SwapBack();
            else
                GameGrid.RebuildGrid();
        }
    }
}
