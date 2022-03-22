using System.Collections.Generic;
using Level.Section;
using Misc;
using Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.Creator
{
    public abstract class BaseLevelCreator : ILevelCreator
    {
        protected (int, int) _duration = (25, 30); // How long is it going to take a level to complete, min-max
        protected float _levelLength;
        protected GameObject _path = null;
        protected List<IPooledMonoBehaviour> _startZonePrefabs;
        protected List<IPooledMonoBehaviour> _endZonePrefabs;

        public BaseLevelCreator()
        {
            _levelLength = Random.Range(_duration.Item1 * ConstValues.MovementSpeed, _duration.Item2 * ConstValues.MovementSpeed);

            FillPrefabsList();
        }

        protected virtual void FillPrefabsList()
        {
            _startZonePrefabs = new List<IPooledMonoBehaviour>();
            _endZonePrefabs = new List<IPooledMonoBehaviour>();

            _startZonePrefabs.AddRange(Resources.LoadAll<PooledMonoBehaviour>("Sections/StartSections"));
            _endZonePrefabs.AddRange(Resources.LoadAll<PooledMonoBehaviour>("Sections/EndSections"));
        }

        public float CreateLevel()
        {
            ReleaseCurrentLevel();

            _path = new GameObject();
            _path.name = "Path";

            float pathLength = 0;
            pathLength = CreateStartSection(_path);
            pathLength = CreateMidSection(_path, pathLength / 2);
            CreateEndSection(_path, pathLength);

            return pathLength;
        }

        protected virtual void ReleaseCurrentLevel()
        {
            if (_path == null)
                return;

            _path.DisableAllChildren();
            GameObject.Destroy(_path);
            _path = null;
        }

        protected virtual float CreateStartSection(GameObject parent)
        {
            var startSection = _startZonePrefabs[Random.Range(0, _startZonePrefabs.Count)].Get<ISection>(parent.transform);
            startSection.transform.position = new Vector3(0, 0, 0);
            return startSection.Length;
        }

        protected abstract float CreateMidSection(GameObject parent, float pathLength);

        protected virtual void CreateEndSection(GameObject parent, float pathLength)
        {
            var endSection = _endZonePrefabs[Random.Range(0, _endZonePrefabs.Count)].Get<ISection>(parent.transform);
            endSection.transform.position = new Vector3(0, 0, pathLength + endSection.Length / 2);
        }
    }
}