using UnityEngine;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "New PowerUp")]
    public class PowerUp : ScriptableObject
    {
        public int powerUpId;
        public string powerUpName;
        public string description;
        public int level;

        public Sprite artwork;
    }
}