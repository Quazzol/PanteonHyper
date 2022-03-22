using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private ILevelManager _currentLevel;

        private void Awake()
        {
            _currentLevel = GetComponent<ILevelManager>();
            _currentLevel.LevelCompleted += OnLevelCompleted;
            InputSystem.RunnerInputController.BackClicked += OnBackClicked;
        }

        private void OnBackClicked()
        {
            Application.Quit();
        }

        private void OnLevelCompleted(int currentLevel)
        {
            SceneManager.LoadScene(SceneNames.GetNextLevel(currentLevel));
        }
    }
}