using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Screens
{
    static class MainScreen
    {
        private static List<Image> drawActiveImageList = new List<Image>();
        private static List<StringForDraw> drawListOfStrings = new List<StringForDraw>();
        private static List<Image> drawStaticImageList = new List<Image>();
        private static List<Image> destroyersImageList = new List<Image>();
        private static List<Image> bombImageList = new List<Image>();

        public static List<Image> DrawActiveImageList
        {
            get => drawActiveImageList;
            set => drawActiveImageList = value;
        }

        public static List<Image> DrawStaticImageList
        {
            get => drawStaticImageList;
            set => drawStaticImageList = value;
        }

        public static List<Image> DrawListOfDestroyers
        {
            get => destroyersImageList;
            set => destroyersImageList = value;
        }

        public static List<StringForDraw> DrawListOfStrings
        {
            get => drawListOfStrings;
            set => drawListOfStrings = value;
        }

        public static void UpdateActiveImageList(List<Image> loadActiveDrawImageList)
        {
            drawActiveImageList.Clear();
            DrawActiveImageList = loadActiveDrawImageList;
        }

        public static void UpdateDestroyersImageList (List<Image> loadDestroyersDrawImageList)
        {
            destroyersImageList.Clear();
            DrawListOfDestroyers = loadDestroyersDrawImageList;
        }

        public static void UpdateBombImageList(List<Image> loadBombDrawImageList)
        {
            bombImageList.Clear();
            bombImageList = loadBombDrawImageList;
        }

        public static void UpdateStaticImageList(List<Image> loadStaticDrawImageList)
        {
            drawStaticImageList.Clear();
            DrawStaticImageList = loadStaticDrawImageList;
        }

        public static void UpdateDrawListOfStrings(List<StringForDraw> loadDrawListOfStrings)
        {
            drawListOfStrings.Clear();
            drawListOfStrings = loadDrawListOfStrings;
        }

        public static void Clear()
        {
            drawListOfStrings.Clear();
            DrawStaticImageList.Clear();
            DrawActiveImageList.Clear();
            destroyersImageList.Clear();
            bombImageList.Clear();
        }

        public static void DrawScreen(GameTime gameTime, SpriteBatch batch)
        {
            drawStaticImageList.ForEach(x => x.DrawImage(batch));
            drawListOfStrings.ForEach(x => x.DrawString(batch));
            drawActiveImageList.ForEach(x => x.DrawImage(batch));
            bombImageList.ForEach(x => x.DrawImage(batch));
            destroyersImageList.ForEach(x => x.DrawImage(batch));
        }
    }
}
