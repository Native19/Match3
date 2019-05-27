using Match3.GameObjects;
using Match3.GameObjects.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match3.GameLogic
{
    static class SelectedController
    {
        private static Ghost selectedFirst = null;
        private static Ghost selectedSecond = null;
        private static Point[] neighbors = new Point[] { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };

        public static void UnselectElements()
        {
            if (selectedFirst != null)
            {
                GameGrid.Grid[selectedFirst.PositionInGridX, selectedFirst.PositionInGridY].IsSelected = false;
                selectedFirst = null;
            }
            if (selectedSecond != null)
            {
                GameGrid.Grid[selectedSecond.PositionInGridX, selectedSecond.PositionInGridY].IsSelected = false;
                selectedSecond = null;
            }
        }

        private static void ElementsBeside()
        {
            for (int i = 0; i < neighbors.Length; i++)
                if (GameGrid.InGridSize(selectedSecond.PositionInGridX + neighbors[i].X, selectedSecond.PositionInGridY + neighbors[i].Y) &&
                    GameGrid.Grid[selectedSecond.PositionInGridX + neighbors[i].X, selectedSecond.PositionInGridY + neighbors[i].Y] == selectedFirst &&
                    MoveController.swap == false)
                {
                    GameGrid.ElementsSwap(selectedFirst, selectedSecond);
                    MoveController.AddElementToMovingList(selectedFirst);
                    MoveController.AddElementToMovingList(selectedSecond);
                    MoveController.swap = true;
                    return;
                }
            UnselectElements();
        }

        public static bool SwapBack()
        {
            if (GameGrid.findAndRemoveMatches())
            {
                GameGrid.ElementsSwap(
                    GameGrid.Grid[selectedFirst.PositionInGridX, selectedFirst.PositionInGridY],
                    GameGrid.Grid[selectedSecond.PositionInGridX, selectedSecond.PositionInGridY]);
                MoveController.swap = false;
                UnselectElements();
                return true;
            }
            MoveController.swap = false;
            UnselectElements();
            return false;
        }

        public static void SelectedElementSet(Ghost selectedElement)
        {
            if (selectedElement.Deleted)
            {
                UnselectElements();
                return;
            }


            if (selectedFirst == null)
            {
                selectedFirst = selectedElement;
            }
            else
            {
                selectedSecond = selectedElement;
                ElementsBeside();
            }

        }
    }
}
