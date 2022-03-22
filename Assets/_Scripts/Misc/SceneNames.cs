using System.Collections.Generic;

namespace Misc
{
    public static class SceneNames
    {
        public static Dictionary<int, string> LevelNames = new Dictionary<int, string>();
        public static string SoloRun = "SoloRunScene";
        public static string WallPaint = "WallPaintScene";
        public static string MultiRun = "MultiplayerRunScene";

        static SceneNames()
        {
            LevelNames.Add(1, SoloRun);
            LevelNames.Add(2, WallPaint);
            LevelNames.Add(3, MultiRun);
        }

        public static string GetNextLevel(int currentLevel)
        {
            var nextLevel = currentLevel + 1;
            if (!LevelNames.ContainsKey(nextLevel))
                nextLevel = 1;

            return LevelNames[nextLevel];
        }
    }
}