using System;
using Character;
using InputSystem;
using Misc;
using Pooling;
using UnityEngine;
using WallLevel;

namespace Manager
{
    public class Level2Manager : MonoBehaviour, ILevelManager
    {
        public event Action<int> LevelCompleted;

        [SerializeField]
        private Brush _brushPrefab;

        private Level2UIManager _uiManager = null;
        private LevelState _state = LevelState.Ready;
        private ICharacter _player;
        private int _level = 2;
        private Brush _brush;

        private void Awake()
        {
            _uiManager = GetComponent<Level2UIManager>();
        }

        private void Start()
        {
            CreatePlayer();
            CreateBrush();

            InputSystem.RunnerInputController.Click += OnClicked;
        }

        private void OnDisable()
        {
            InputSystem.RunnerInputController.Click -= OnClicked;
            PoolOwner.ClearPool();
        }

        private void CreatePlayer()
        {
            _player = CharacterFactory.CreateCharacter(CharacterType.Player, new Vector3(3f, .5f, 5.5f));
        }

        private void CreateBrush()
        {
            _brush = _brushPrefab.Get<Brush>();
            _brush.CompletePercent += OnCompletePercentChanged;
            _brush.SetActive(true);
        }

        private void OnCompletePercentChanged(float percent)
        {
            _uiManager.UpdatePercent(percent);
            if (percent > .75f)
            {
                _brush.SetActive(false);
                SetState(LevelState.Ended);
                _player.Victory();
            }
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
            SetState(LevelState.Play);
        }
    }
}