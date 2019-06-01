using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Match3.GameLogic;

namespace Match3.GameObjects
{
    static class GameGrid // Класс предназначеный для работы с иговой сеткой
    {
        private static Ghost[,] gameGrid;
        private static Point gridCellSize;
        private static Point gridPosition;
        private static Point gameElementSize;
        private static Point gridSize;
        private static int colors;
        private static bool gameStarted = false;

        public static Ghost[,] Grid
        {
            get { return gameGrid; }
        }

        public static bool GameStart
        {
            set { gameStarted = value; }
        }

        static private Ghost[,] CreateGrid(Point gridSize)
        {
            Random randomizer = new Random();
            Ghost[,] gameGrid = new Ghost[gridSize.X, gridSize.Y];
            for (int row = 0; row < gridSize.X; row++)
                for (int col = 0; col < gridSize.Y; col++)
                {
                    gameGrid[row, col] = new Ghost(randomizer.Next(colors), new Point(row, col));
                    gameGrid[row, col].TextureSet(gameGrid[row, col].GhostTextureName(gameGrid[row, col].GhostColor));
                    SetPositionsGamesObject(gameGrid[row, col]);
                    gameGrid[row, col].PositionNow = gameGrid[row, col].Position;
                }
            return gameGrid;
        }

        static public void RebuildGrid()
        {
            if (MoveController.movingDestroyersList.Count != 0)
            {
                MoveController.DestMove(GameController.nowGameTime);
                return;
            }
            if (MoveController.bombIsActive)
            {
                MoveController.ElementsMove(GameController.nowGameTime);
                return;
            }
            while (!findAndRemoveMatches())
            {
                AddNewGhosts();
            }
            AddNewGhosts();
            if (!IsPossible())
            {
                InitializeGrid(gridSize, gridPosition, gridCellSize, gameElementSize, colors);
            }
            return;
        }

        static public void InitializeGrid(Point newGridSize, Point newGridPosition, Point newGridCellSize, Point newGameElementSize, int numberOfColors)
        {
            gridSize = newGridSize;
            gridPosition = newGridPosition;
            gridCellSize = newGridCellSize;
            gameElementSize = newGameElementSize;
            colors = numberOfColors;

            gameGrid = CreateGrid(gridSize);
            RebuildGrid();
            gameStarted = true;
        }

        private static List<Ghost> getMatchHorizontal(int row, int col)
        {
            List<Ghost> match = new List<Ghost>(gameGrid[row, col].GhostColor);
            match.Add(gameGrid[row, col]);
            for (int i = 1; row + i < gridSize.X; i++)
                if (!gameGrid[row + i, col].Deleted && gameGrid[row, col].GhostColor == gameGrid[row + i, col].GhostColor)
                    if (gameGrid[row + i, col].GhostBoost == 0)
                        match.Insert(0, gameGrid[row + i, col]);
                    else
                        match.Add(gameGrid[row + i, col]);
                else
                    return match;
            return match;
        }

        private static List<Ghost> getMatchVertical(int row, int col)
        {
            List<Ghost> match = new List<Ghost>(gameGrid[row, col].GhostColor);
            match.Add(gameGrid[row, col]);
            for (int i = 1; col + i < gridSize.Y; i++)
                if (!gameGrid[row, col + i].Deleted &&  gameGrid[row, col].GhostColor == gameGrid[row, col + i].GhostColor)
                    if (gameGrid[row, col + i].GhostBoost == 0)
                        match.Insert(0, gameGrid[row, col + i]);
                    else
                        match.Add(gameGrid[row, col + i]);
                else
                    return match;
            return match;
        }

        private static List<List<Ghost>> lookForMatches()
        {
            List<List<Ghost>> matchList = new List<List<Ghost>>();
            for (int col = 0; col < gridSize.Y; col++)
                for (int row = 0; row < gridSize.X - 2; row++)
                {
                    List<Ghost> match = new List<Ghost>(getMatchHorizontal(row, col));
                    if (match.Count() > 2)
                    {
                        matchList.Add(match);
                        row += match.Count() - 1;
                    }
                }
            for (int row = 0; row < gridSize.X; row++)
                for (int col = 0; col < gridSize.Y - 2; col++)
                {
                    List<Ghost> match = new List<Ghost>(getMatchVertical(row, col));
                    if (match.Count() > 2)
                    {
                        matchList.Add(match);
                        col += match.Count() - 1;
                    }
                }
            if (gameStarted)
            {
                foreach (Ghost element in FindSameElementsInMatches(matchList)) // Найти бомбу на пересечении
                {
                        Grid[element.PositionInGridX, element.PositionInGridY].SetBoost(0);
                        Grid[element.PositionInGridX, element.PositionInGridY].Deleted = false;
                        foreach (List<Ghost> list in matchList)
                            list.Remove(element);
                }
            }
            return matchList;
        }

        public static bool findAndRemoveMatches()
        {
            List<List<Ghost>> matches = new List<List<Ghost>>(lookForMatches());
            if (matches.Count() == 0)
                return true;

            for (int i = 0; i < matches.Count(); i++)
            {
                Ghost boostedElement = new Ghost();
                boostedElement = matches[i].Find(element => element.IsSelected == true);
                if (!(boostedElement != null && boostedElement.IsBoosted))
                {
                    if (!(boostedElement != null && !boostedElement.IsBoosted))
                    {
                        var random = new Random();
                        boostedElement = matches[i][random.Next(matches[i].Count)];
                        while (boostedElement.IsBoosted)
                        {
                            boostedElement = matches[i][random.Next(matches[i].Count)];
                        }
                    }
                    if (gameStarted && matches[i].Count > 3) // добавление буста
                    {
                        if (matches[i].Count > 4)
                        {
                            boostedElement.SetBoost(0);
                            matches[i].Remove(boostedElement);
                        }
                        else
                        {
                            if (IsMatchVertical(matches[i]))
                                boostedElement.SetBoost(1);
                            else
                                boostedElement.SetBoost(2);
                            matches[i].Remove(boostedElement);
                        }
                        boostedElement.Deleted = false;
                    }
                }
                for (int j = 0; j < matches[i].Count; j++)
                    RemoveGhost(gameGrid[matches[i][j].PositionInGrid.X, matches[i][j].PositionInGrid.Y]);
            }
            return false;
        }

        public static void RemoveGhost(Ghost deleteGhost)
        {
            if (gameStarted)
                    GameController.Score +=  10;
            deleteGhost.Deleted = true;
            if (deleteGhost.IsBoosted)
            {
                RemoveBoostedGhost(deleteGhost);
            }
            if (!MoveController.destroyer && !MoveController.bombIsActive)
            {
                AffectAbove(deleteGhost);
            }
        }

        private static void RemoveBoostedGhost(Ghost deleteGhost) 
        {
            switch (deleteGhost.GhostBoost)
            {
                case 0:
                    List<Ghost> deleteList = new List<Ghost>();
                    for (int row = -1; row <= 1; row++)
                        for (int col = -1; col <= 1; col++)
                            if (!(row == 0 && col == 0) &&
                                InGridSize(deleteGhost.PositionInGrid.X + row, deleteGhost.PositionInGrid.Y + col) &&
                                !Grid[deleteGhost.PositionInGrid.X + row, deleteGhost.PositionInGrid.Y + col].Deleted)
                                deleteList.Add(gameGrid[deleteGhost.PositionInGrid.X + row, deleteGhost.PositionInGrid.Y + col]);
                    Bomb newBomb = new Bomb("Explosion", "explosion", new Point(
                        gameGrid[deleteGhost.PositionInGrid.X, deleteGhost.PositionInGrid.Y].Position.X - gridCellSize.X - (gridCellSize.X - gameElementSize.X) / 2,
                        gameGrid[deleteGhost.PositionInGrid.X, deleteGhost.PositionInGrid.Y].Position.Y - gridCellSize.Y - (gridCellSize.Y - gameElementSize.Y) / 2),
                        deleteList);
                    MoveController.bombIsActive = true;
                    MoveController.activeBomb.Add(newBomb);
                    deleteGhost.IsBoosted = false;
                    deleteGhost.GhostBoost = -1;
                    MoveController.ElementsMove(GameController.nowGameTime);
                    break;
                case 1:
                    MoveController.destroyer = true;
                    Destroyer destroyer1 = new Destroyer(
                        "Destroyer1", "destroyer",
                        new Point(gameGrid[0, deleteGhost.PositionInGrid.Y].Position.Location.X - (gridCellSize.X - gameElementSize.X) / 2,
                        gameGrid[0, deleteGhost.PositionInGrid.Y].Position.Location.Y - (gridCellSize.Y - gameElementSize.Y) / 2),
                        new Point(deleteGhost.Position.Location.X - (gridCellSize.X - gameElementSize.X) / 2,
                        deleteGhost.Position.Location.Y - (gridCellSize.Y - gameElementSize.Y) / 2), Direction.Up);
                    Destroyer destroyer2 = new Destroyer(
                        "Destroyer1", "destroyer",
                        gameGrid[gridSize.X - 1, deleteGhost.PositionInGrid.Y].Position.Location,
                        new Point(deleteGhost.Position.Location.X - (gridCellSize.X - gameElementSize.X)/2,
                        deleteGhost.Position.Location.Y - (gridCellSize.Y - gameElementSize.Y) / 2), Direction.Down);
                    MoveController.movingDestroyersList.Add(destroyer1);
                    MoveController.movingDestroyersList.Add(destroyer2);
                    MoveController.DestMove(GameController.nowGameTime);
                    MoveController.DestroyerMove();
                    break;
                case 2:
                    MoveController.destroyer = true;
                    Destroyer destroyer3 = new Destroyer(
                        "Destroyer1", "destroyer",
                        new Point(gameGrid[deleteGhost.PositionInGrid.X, 0].Position.Location.X - (gridCellSize.X - gameElementSize.X) / 2,
                        gameGrid[deleteGhost.PositionInGrid.X, 0].Position.Location.Y - (gridCellSize.Y - gameElementSize.Y) / 2),
                        new Point(deleteGhost.Position.Location.X - (gridCellSize.X - gameElementSize.X) / 2,
                        deleteGhost.Position.Location.Y - (gridCellSize.Y - gameElementSize.Y) / 2), Direction.Left);
                    Destroyer destroyer4 = new Destroyer(
                        "Destroyer1", "destroyer",
                        gameGrid[deleteGhost.PositionInGrid.X, gridSize.Y - 1].Position.Location,
                        new Point(deleteGhost.Position.Location.X - (gridCellSize.X - gameElementSize.X) / 2,
                        deleteGhost.Position.Location.Y - (gridCellSize.Y - gameElementSize.Y) / 2), Direction.Right);
                    MoveController.movingDestroyersList.Add(destroyer3);
                    MoveController.movingDestroyersList.Add(destroyer4);
                    MoveController.DestMove(GameController.nowGameTime);
                    MoveController.DestroyerMove();
                    break;
                default:
                    break;
            }
            deleteGhost.IsBoosted = false;
            deleteGhost.GhostBoost = -1;
        }

        public static bool InGridSize(int row, int col)
        {
            if (row < 0 || row >= gridSize.X || col < 0 || col >= gridSize.Y)
                return false;
            return true;
        }

        private static List<Ghost> FindSameElementsInMatches (List<List<Ghost>> list) // Поиск элементов на пересечении матчей
        {
            List<Ghost> returnList = new List<Ghost>();
            var hash = new HashSet<long>();
            for (int listNumber = 0; listNumber < list.Count; listNumber++)
                for (int checkedList = listNumber + 1; checkedList < list.Count; checkedList++)
                    {
                    foreach (Ghost element in list[listNumber])
                       if ( list[checkedList].Contains(element) && !element.IsBoosted)
                        returnList.Add(element);
                    }
            return returnList;
        }

        private static bool IsMatchVertical (List<Ghost> match)
        {
            if (match.Find(element => match[0].PositionInGridX + 1 == element.PositionInGridX || match[0].PositionInGridX - 1 == element.PositionInGridX) != null)
                return true;
            return false;
        }

        public static void AffectAboveAlldeleted()
        {
            for (int row = 0; row < gridSize.Y; row++)
            {
                for (int col = 0; col < gridSize.X; col++)
                {
                    if (Grid[row, col].Deleted)
                        AffectAbove(Grid[row, col]);
                }
            }
        }

        private static void AffectAbove(Ghost deletedDghost)
        {
            for (int row = deletedDghost.PositionInGrid.X - 1; row >= 0; row--)
                if (!gameGrid[row, deletedDghost.PositionInGrid.Y].Deleted)
                {
                    ElementsSwap(gameGrid[row + 1, deletedDghost.PositionInGrid.Y], gameGrid[row, deletedDghost.PositionInGrid.Y]);
                    if (gameStarted)
                    {
                        gameGrid[row + 1, deletedDghost.PositionInGrid.Y].IsMoving = true;
                        gameGrid[row, deletedDghost.PositionInGrid.Y].IsMoving = true;
                    }
                }
        }

        private static void NewGhostFall(Ghost newDghost)
        {
            for (int row = 1; row < gridSize.X; row++)
                if (gameGrid[row, newDghost.PositionInGrid.Y].Deleted)
                {
                    ElementsSwap(gameGrid[row, newDghost.PositionInGrid.Y], gameGrid[row - 1, newDghost.PositionInGrid.Y]);
                }
                else
                    return;
        }

        private static void AddNewGhosts()
        {
            bool lineElementsNotdeleted = true;
            Random randomizer = new Random();
            for (int row = 0; row < gridSize.X; row++)
            {
                if (MoveController.IsFirstLineClear(gridPosition, gridCellSize, gridSize))
                {
                    lineElementsNotdeleted = true;
                    for (int col = 0; col < gridSize.Y; col++)
                    {
                        if (gameGrid[0, col].Deleted)
                        {
                            lineElementsNotdeleted = false;
                            gameGrid[0, col].GhostColor = randomizer.Next(colors);
                            gameGrid[0, col].TextureSet(gameGrid[0, col].GhostTextureName(gameGrid[0, col].GhostColor));
                            gameGrid[0, col].Deleted = false;
                            if (gameStarted)
                            {
                                gameGrid[0, col].PositionNow_X = gridPosition.X + gridCellSize.X * col + (gridCellSize.X - gameElementSize.X) / 2;
                                gameGrid[0, col].PositionNow_Y = gridPosition.Y + (gridCellSize.Y - gameElementSize.Y) / 2;
                                NewGhostFall(gameGrid[0, col]);
                            }
                            else
                            {
                                NewGhostFall(gameGrid[0, col]);
                            }
                        }
                    }
                    if (lineElementsNotdeleted)
                        return;
                }
                if (gameStarted && MoveController.movingElementsList.Count != 0)
                    MoveController.ElementsMove(GameController.nowGameTime);
            }
        }         

        static private bool MatchPattern(Point posInGrid, int[,] secondElement, int[,] thirdElement) //Функция поиска возможных перестановок для получения матча
        {
            int pointColour = gameGrid[posInGrid.X, posInGrid.Y].GhostColor;

            for (int i = 0; i < secondElement.GetLength(0); i++)
                if (InGridSize(posInGrid.X + secondElement[i, 0], posInGrid.Y + secondElement[i, 1]))
                {
                    if (pointColour != gameGrid[posInGrid.X + secondElement[i, 0], posInGrid.Y + secondElement[i, 1]].GhostColor)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            for (int i = 0; i < thirdElement.GetLength(0); i++)
                if (InGridSize(posInGrid.X + thirdElement[i, 0], posInGrid.Y + thirdElement[i, 1]))          
                {
                    if (pointColour == gameGrid[posInGrid.X + thirdElement[i, 0], posInGrid.Y + thirdElement[i, 1]].GhostColor)
                    {
                        return true;
                    }
                }
            return false;
        }

        private static bool IsPossible() // Возможна ли перестанока приводящая к составлению матча
        {
            for (int row = 0; row < gridSize.X; row++)
                for (int col = 0; col < gridSize.Y; col++)
                {
                    if (MatchPattern(new Point(row, col), new int[,] { { 1, 0 } }, new int[,] { { -2, 0 }, { -1, -1 }, { -1, 1 }, { 2, -1 }, { 2, 1 }, { 3, 0 } }))
                    {
                        return true;
                    }
                    if (MatchPattern(new Point(row, col), new int[,] { { 2, 0 } }, new int[,] { { 1, -1 }, { 1, 1 } }))
                    {
                        return true;
                    }
                    if (MatchPattern(new Point(row, col), new int[,] { { 0, 1 } }, new int[,] { { 0, -2 }, { -1, -1 }, { 1, -1 }, { -1, 2 }, { 1, 2 }, { 0, 3 } }))
                    {
                        return true;
                    }
                    if (MatchPattern(new Point(row, col), new int[,] { { 0, 2 } }, new int[,] { { -1, 1 }, { 1, 1 } }))
                    {
                        return true;
                    }
                }
            return false;
        }

        public static void ElementSelected(MouseState lastMouseState)
        {
            for (int row = 0; row < gridSize.X; row++)
                for (int col = 0; col < gridSize.Y; col++)
                    if (gameGrid[row, col].IsPresed(lastMouseState) && MoveController.movingElementsList.Count == 0)
                    {
                        gameGrid[row, col].IsSelected = true;
                        SelectedController.SelectedElementSet(gameGrid[row, col]);
                    }
        }

        public static List<Bomb> BombToGameObjects()
        {
            List<Bomb> returnList = new List<Bomb>();
            if (MoveController.bombIsActive)
                returnList.AddRange(MoveController.activeBomb);
            return returnList;
        }

        public static List<Destroyer> DestroyersToGameObjects()
        {
            List<Destroyer> retunList = new List<Destroyer>();
            if (MoveController.destroyer)
            {
                for (int destroyer = 0; destroyer < MoveController.movingDestroyersList.Count; destroyer++)
                    retunList.Add(MoveController.movingDestroyersList[destroyer]);
            }
            return retunList;
        }

        public static List<ActiveElement> GridToGameObjects()
        {
            List<ActiveElement> returnList = new List<ActiveElement>();
            for (int i = 0; i < gridSize.X; i++)
            {
                for (int j = 0; j < gridSize.Y; j++)
                {
                    if (GameGrid.Grid[i, j].IsSelected)
                        returnList.Add(
                        new ActiveElement("GridActiveElement", "staticElement",
                        new Point(
                            gridPosition.X + gridCellSize.X * j,
                            gridPosition.Y + gridCellSize.Y * i)));
                    if (GameGrid.Grid[i, j].Deleted != true)
                        returnList.Add(GameGrid.Grid[i, j]);

                    switch (GameGrid.Grid[i, j].GhostBoost)
                    {
                        case 0:
                            returnList.Add(
                            new ActiveElement("Bomb", "bomb",
                            new Point(
                                GameGrid.Grid[i, j].PositionNow.X,
                                GameGrid.Grid[i, j].PositionNow_Y)));
                            break;
                        case 1:
                            ActiveElement lineVertical = new ActiveElement("LineVertical", "lineBonus");
                            lineVertical.PositionNow_X = GameGrid.Grid[i, j].PositionNow.X;
                            lineVertical.PositionNow_Y = GameGrid.Grid[i, j].PositionNow.Y - (lineVertical.Texture.Height - gameElementSize.Y)/ 2;
                            returnList.Add(lineVertical);
                            break;
                        case 2:
                            ActiveElement lineHorizontal = new ActiveElement("LineHorizontal", "lineBonus");
                            lineHorizontal.PositionNow_X = GameGrid.Grid[i, j].PositionNow.X - (lineHorizontal.Texture.Width - gameElementSize.X) / 2;
                            lineHorizontal.PositionNow_Y = GameGrid.Grid[i, j].PositionNow.Y;
                            returnList.Add(lineHorizontal);
                            break;
                    }
                }
            }
            return returnList;
        }

        private static void SetPositionsGamesObject( Ghost element)
        {
            element.Position = new Rectangle(
                        gridPosition.X + gridCellSize.X * element.PositionInGridY + (gridCellSize.X - gameElementSize.X) / 2,
                        gridPosition.Y + gridCellSize.Y * element.PositionInGridX + (gridCellSize.Y - gameElementSize.Y) / 2,
                        gameElementSize.X,
                        gameElementSize.Y);
        }

        public static void ElementsSwap(Ghost elementToSwap1, Ghost elementToSwap2)
        {
            Ghost temp3 = gameGrid[elementToSwap2.PositionInGridX, elementToSwap2.PositionInGridY];
            Point temp2 = elementToSwap2.PositionInGrid;
            Point temp1 = elementToSwap1.PositionInGrid;
            gameGrid[temp2.X, temp2.Y] = gameGrid[temp1.X, temp1.Y];
            gameGrid[temp1.X, temp1.Y] = temp3;
            gameGrid[temp2.X, temp2.Y].PositionInGrid = temp2;
            gameGrid[temp1.X, temp1.Y].PositionInGrid = temp1;
            SetPositionsGamesObject(gameGrid[temp1.X, temp1.Y]);
            SetPositionsGamesObject(gameGrid[temp2.X, temp2.Y]);
            if (gameStarted)
            {
                gameGrid[temp2.X, temp2.Y].IsMoving = true;
                gameGrid[temp1.X, temp1.Y].IsMoving = true;
                MoveController.AddElementToMovingList(gameGrid[temp1.X, temp1.Y]);
                MoveController.AddElementToMovingList(gameGrid[temp2.X, temp2.Y]);
            }
            else
            {
                gameGrid[temp1.X, temp1.Y].PositionNow = gameGrid[temp1.X, temp1.Y].Position;
                gameGrid[temp2.X, temp2.Y].PositionNow = gameGrid[temp2.X, temp2.Y].Position;
            }
        }
    }
}

