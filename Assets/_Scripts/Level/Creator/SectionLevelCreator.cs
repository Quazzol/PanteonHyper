using System.Collections.Generic;
using Level.Section;
using Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.Creator
{
    public class SectionLevelCreator : BaseLevelCreator
    {
        private List<IPooledMonoBehaviour> _sectionPrefabs;

        public SectionLevelCreator() : base()
        { }

        protected override void FillPrefabsList()
        {
            base.FillPrefabsList();

            _sectionPrefabs = new List<IPooledMonoBehaviour>();
            _sectionPrefabs.AddRange(Resources.LoadAll<PooledMonoBehaviour>("Sections/MidSections"));
        }

        protected override float CreateMidSection(GameObject parent, float pathLength)
        {
            float lastLength = pathLength;

            while (true)
            {
                var section = _sectionPrefabs[Random.Range(0, _sectionPrefabs.Count)].Get<ISection>(parent.transform);
                section.transform.position = new Vector3(0, 0, section.Length / 2 + lastLength);
                lastLength += section.Length;

                if (_levelLength <= lastLength)
                    break;
            }

            return lastLength;
        }

    }
}