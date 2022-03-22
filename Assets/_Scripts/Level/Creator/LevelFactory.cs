using System;

namespace Level.Creator
{
    [Serializable]
    public class LevelFactory
    {
        public static ILevelCreator GetLevelCreator()
        {
            return new SectionLevelCreator();
        }
    }
}