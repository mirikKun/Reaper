using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Players
{
    public class UIPlayerStateDisplay : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image[] _actionPoints;
        [SerializeField] private Image _actionPointsBar;

        [SerializeField] private Color _activePointsColor;
        [SerializeField] private Color _spentPointsColor;

        private int _actionPointCount = 5;

        public void ChangeActionPointCount(int newCount)
        {
            _actionPointCount = Mathf.Clamp(newCount, 1, _actionPoints.Length);
            for (var i = 0; i < _actionPoints.Length; i++)
            {
                var point = _actionPoints[i];
                point.gameObject.SetActive(_actionPointCount > i);
                if (_actionPointCount > i)
                {
                    //TODO
                }
            }
        }

        public void FillActionPoints(float value)
        {
            _actionPointsBar.fillAmount = value;
            for (int i = 0; i < _actionPointCount; i++)
            {
                if (value >= (i + 1f) / _actionPointCount)
                {
                    _actionPoints[i].color = _activePointsColor;
                }
                else
                {
                    _actionPoints[i].color = _spentPointsColor;
                }
            }
        }

        public void FillActionPoint(int leftPoint)
        {
            FillActionPoints((float)leftPoint / _actionPointCount);
        }

        public void SetHealthValue(float percent)
        {
            _healthBar.fillAmount = percent;
        }
    }
}