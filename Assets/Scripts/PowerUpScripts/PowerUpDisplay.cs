using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PowerUps
{
    public class PowerUpDisplay : MonoBehaviour
    {
        [SerializeField] private PowerUp _powerUp;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private Image artworkImage;

        public PowerUp DisplayedPowerUp
        {
            get { return _powerUp; }
            set { _powerUp = value; }
        }

        public void UpdateDisplay()
        {
            _nameText.text = _powerUp.powerUpName;
            _descriptionText.text = _powerUp.description;
            _levelText.text = _powerUp.level.ToString();

            artworkImage.sprite = _powerUp.artwork;
        }
    }
}