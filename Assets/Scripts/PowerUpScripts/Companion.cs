using UnityEngine;

[CreateAssetMenu(fileName = "New Companion")]
public class Companion : ScriptableObject
{   
    public int companionId;
    public string companionName;
    public string description;

    public Sprite shipAttachmentSprite;
    public GameObject collectableCompanion;
    public Sprite artwork;
}
