using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PowerUps
{
    public class PowerUpDisplay : MonoBehaviour
    {
        [SerializeField] private PowerUp _powerUp;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image artworkImage;

        public PowerUp DisplayedPowerUp
        {
            get { return _powerUp; }
            set { _powerUp = value; }
        }

        public void UpdateDisplay()
        {
            nameText.text = _powerUp.powerUpName;
            descriptionText.text = _powerUp.description;

            artworkImage.sprite = _powerUp.artwork;
        }
    }
}