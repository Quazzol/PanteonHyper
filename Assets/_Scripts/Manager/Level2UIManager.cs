using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Manager
{
    public class Level2UIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _txtPercent;
        [SerializeField]
        private Image _imgPercent;

        public void UpdatePercent(float percent)
        {
            _imgPercent.fillAmount = percent;
            _txtPercent.text = string.Format("{0:0.00}", percent * 100);
        }
    }
}