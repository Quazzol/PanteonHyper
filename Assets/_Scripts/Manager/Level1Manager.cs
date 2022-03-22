using System;
using Character;
using InputSystem;
using Level.Creator;
using Misc;
using Pooling;
using UnityEngine;

namespace Manager
{
    public class Level1Manager : MonoBehaviour, ILevelManager
    {
        public event Action<int> LevelCompleted;

        private CameraController _cameraController = null;
        private ILevelCreator _levelCreator = null;
        private Level1UIManager _uiManager = null;
        private LevelState _state = LevelState.Ready;
        private ICharacter _player;
        private int _level = 1;

        private void Awake()
        {
            _cameraController = GetComponent<CameraController>();
            _uiManager = GetComponent<Level1UIManager>();
            _levelCreator = LevelFactory.GetLevelCreator();

            RunnerInputController.Click += OnClicked;
        }

        private void Start()
        {
            PrepareLevel();
            CreatePlayer();
        }

        private void OnDisable()
        {
            RunnerInputController.Click -= OnClicked;
            PoolOwner.ClearPool();
        }

        private void PrepareLevel()
        {
            _levelCreator.CreateLevel();
            _uiManager.Reset();
        }

        private void CreatePlayer()
        {
            _player = CharacterFactory.CreateCharacter(CharacterType.Player, new Vector3(0, .5f, 0));
            _player.Finished += OnPlayerWon;

            _cameraController.SetObjectToFollow(_player.Transform);
            _cameraController.StartFollowing();
        }

        private void SetState(LevelState state)
        {
            _state = state;
        }

        private void OnClicked()
        {
            if (_state == LevelState.Play)
                return;

            if (_state == LevelState.Ready)
            {
                StartLevel();
                return;
            }

            if (_state == LevelState.Ended)
            {
                LevelCompleted?.Invoke(_level);
            }
        }

        private void StartLevel()
        {
            _player.Run();
            _uiManager.StartTimer();
            SetState(LevelState.Play);
        }

        private void OnPlayerWon(ICharacter character)
        {
            _cameraController.StartRotating();
            _uiManager.StopTimer();
            SetState(LevelState.Ended);
        }

    }
}