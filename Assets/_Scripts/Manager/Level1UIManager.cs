using UnityEngine;
using TMPro;
using System.Diagnostics;

namespace Manager
{
    public class Level1UIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _txtTime;
        [SerializeField]
        private TextMeshProUGUI _txtRank;
        private bool _timerStarted;
        private Stopwatch _stopWatch = new Stopwatch();

        private void Update()
        {
            if (!_timerStarted)
                return;

            UpdateTimer(_stopWatch.ElapsedMilliseconds);
        }

        private void UpdateTimer(long milliseconds)
        {
            _txtTime.text = string.Format("{0}:{1:00}", milliseconds / 1000, milliseconds % 1000);
        }

        public void StartTimer()
        {
            _timerStarted = true;
            _stopWatch.Start();
        }

        public void StopTimer()
        {
            _timerStarted = false;
            _stopWatch.Stop();
        }

        public void Reset()
        {
            _timerStarted = false;
            _stopWatch.Reset();
            UpdateTimer(0);
            UpdateRank(1);
        }

        public void UpdateRank(int rank)
        {
            _txtRank.text = rank.ToString();
        }
    }
}