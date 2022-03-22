using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using InputSystem;
using Level.Creator;
using Misc;
using Pooling;
using UnityEngine;

namespace Manager
{
    public class Level3Manager : MonoBehaviour, ILevelManager
    {
        public event Action<int> LevelCompleted;

        private CameraController _cameraController = null;
        private ILevelCreator _levelCreator = null;
        private Level3UIManager _uiManager = null;
        private LevelState _state = LevelState.Ready;
        private List<ICharacter> _characters = new List<ICharacter>();
        private int _level = 3;
        private int _aiCount = 10;

        private void Awake()
        {
            _cameraController = GetComponent<CameraController>();
            _uiManager = GetComponent<Level3UIManager>();
            _levelCreator = LevelFactory.GetLevelCreator();

            RunnerInputController.Click += OnClicked;
        }

        private void Start()
        {
            PrepareLevel();
            CreatePlayer();
            CreateAIs();
        }

        private void Update()
        {
            if (_state == LevelState.Play)
            {
                UpdateRank();
            }
        }

        private void OnDisable()
        {
            RunnerInputController.Click -= OnClicked;
            PoolOwner.ClearPool();
        }

        private void UpdateRank()
        {
            var ranked = _characters.Where(q => !q.IsFinished).OrderByDescending(q => q.transform.position.z).ToList();
            var playerRank = 0;
            for (int i = 0; i < ranked.Count; i++)
            {   
                if (ranked[i].CharacterType == CharacterType.Player)
                {
                    playerRank = i + 1 + _finishedAis;
                    break;
                }
            }
            _uiManager.UpdateRank(playerRank);
        }

        private void PrepareLevel()
        {
            _levelCreator.CreateLevel();
            _uiManager.Reset();
        }

        private void CreatePlayer()
        {
            var player = CharacterFactory.CreateCharacter(CharacterType.Player, new Vector3(0, .5f, 0));
            player.Finished += OnCharacterFinished;

            _cameraController.SetObjectToFollow(player.Transform);
            _cameraController.StartFollowing();

            _characters.Add(player);
        }

        private void CreateAIs()
        {
            for (int i = 0; i < _aiCount; i++)
            {
                var ai = CharacterFactory.CreateCharacter(CharacterType.AI, new Vector3(-3 + (i % 4) * 2, .5f, -1 - i / 4 * 2));
                ai.Finished += OnCharacterFinished;
                _characters.Add(ai);
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
            RunCharacters();
            _uiManager.StartTimer();
            SetState(LevelState.Play);
        }

        private void RunCharacters()
        {
            foreach (var character in _characters)
            {
                character.Run();
            }
        }

        private int _finishedAis = 0;
        private void OnCharacterFinished(ICharacter character)
        {
            if (character.CharacterType == CharacterType.Player)
            {
                _cameraController.StartRotating();
                _uiManager.StopTimer();
                SetState(LevelState.Ended);
                return;
            }

            _finishedAis++;
        }

    }
}