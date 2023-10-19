using UnityEngine;

[CreateAssetMenu(fileName = "New Companion")]
public class Companion : ScriptableObject
{   
    public int companionId;
    public string companionName;
    public string description;

    public Sprite shipAttachmentSprite;

    //Get rid of this and have a main one with a sprite replacement in manager?
    public GameObject collectableCompanion;
    public Sprite artwork;
}
