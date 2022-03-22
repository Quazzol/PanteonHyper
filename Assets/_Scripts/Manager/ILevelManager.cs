using System;

namespace Manager
{
    public interface ILevelManager
    {
        public event Action<int> LevelCompleted;
    }
}