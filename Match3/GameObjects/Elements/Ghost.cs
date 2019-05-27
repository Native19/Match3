using Microsoft.Xna.Framework;

namespace Match3.GameObjects.Elements
{
    class Ghost : ActiveElement
    {
        private bool isSelected = false;
        private int ghostColor = -1;
        private bool isBoosted = false;
        private int ghostBoost = -1;
        private Point positionInGrid = new Point(-1,-1);
        private bool deleted = false;
        private static string[] ColorMap = new string[] { "RedGhost", "BlueGhost", "OrangeGhost", "Greenghost", "PurplegGhost" };

        public Ghost()
        {

        }

        public Ghost(int ghostColor, Point positionInGrid)
        {
            this.ghostColor = ghostColor;
            this.positionInGrid = positionInGrid;
        }


        public Ghost(int ghostColor, string elementType, Rectangle position)
        {
            this.ElementType = elementType;
            this.GhostColor = ghostColor;
            this.Position = position;
        }

        public void GhostInitialize(string texture, string elementType, Rectangle position, int ghostColor)
        {
            this.ElementType = elementType;
            this.Position = position;
            this.ghostColor = ghostColor;
        }

        public Ghost(bool isSelected, int ghostColor)
        {
            this.isSelected = isSelected;
            this.ghostColor = ghostColor;
        }

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        public Point PositionInGrid
        {
            get { return positionInGrid; }
            set { positionInGrid = value; }
        }

        public int PositionInGridX
        {
            get { return positionInGrid.X; }
            set { positionInGrid.X = value; }
        }

        public int PositionInGridY
        {
            get { return positionInGrid.Y; }
            set { positionInGrid.Y = value; }
        }

        public int GhostColor
        {
            get { return ghostColor; }
            set { ghostColor = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public bool IsBoosted
        {
            get { return isBoosted; }
            set { isBoosted = value; }
        }

        public int GhostBoost
        {
            get { return ghostBoost; }
            set { ghostBoost = value; }
        }

        public string GhostTextureName(int ghostColorNumber)
        {
            return ColorMap[ghostColorNumber];
        }

        public void SetBoost(int boostType)
        {
            this.IsBoosted = true;
            this.GhostBoost = boostType;
            this.IsSelected = false;
        }
    }
}
