using UnityEngine.Audio;
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New AudioClip", menuName = "AudioClip")]
    public class Sound : ScriptableObject
    {

        public string clipName;

        public AudioClip clip;

        [HideInInspector]
        public AudioSource source;
        public AudioMixerGroup mixerGroup;

        [Range(0f, 1f)]
        public float Volume = 0.5f;
        [Range(0.1f, 3f)]
        public float Pitch = 1;

        public bool Loop;
        public bool playedThisFrame;
    }
}
