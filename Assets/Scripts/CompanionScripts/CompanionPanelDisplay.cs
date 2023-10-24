using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionPanelDisplay : MonoBehaviour
{
    private Companion _companion;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image artworkImageShadow;
    [SerializeField] private Image artworkImage;

    public Companion DisplayedCompanion
    {
        get { return _companion; }
    }

    public void UpdateDisplay(Companion _newRandCompanion)
    {
        _companion = _newRandCompanion;
        _nameText.text = _companion.companionName;
        _descriptionText.text = _companion.description;
        artworkImage.sprite = _companion.artwork;
        artworkImageShadow.sprite = _companion.artwork;
    }
}
