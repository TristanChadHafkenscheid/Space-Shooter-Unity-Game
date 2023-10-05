// <header>
// File Name : AudioManager_UU.cs
// Purpose:  Handles the playing of audio clips
// Created By  : Unknown
// Created On  : --/--/----
// Modified By : Ellivro Guevarra
// Modified On : 12/16/2021
// Modification Note: Script Formatting
// Other Notes:
// </header>


using System;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    [Serializable]
    public class AudioChannel
    {
        public string _ChannelName;
        public bool _Enabled;
        public float _BaseVolume;

        public void SetVolume(float val)
        {
            _BaseVolume = val;
        }

        public void SetEnabled(bool val)
        {
            Debug.Log(_ChannelName + " set to " + val);
            _Enabled = val;
        }
    }

    public class AudioManager : MonoBehaviour
    {
        #region [VARIABLES] Static/Private

        public static AudioManager Instance;

        private List<AudioSource> m_ScavAudioSources = new List<AudioSource>();


        #endregion



        #region [VARIABLES] Public/Inspector

        public List<Sound> sounds = new List<Sound>();
        public Slider optionSlider;

        public AudioMixer m_AudioMixer;
        public List<AudioChannel> audioChannels;

        #endregion



        #region [METHODS] Unity

        // Use this for initialization
        void Awake()
        {

            if (!Instance)
                Instance = this;

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.outputAudioMixerGroup = s.mixerGroup;
                s.source.clip = s.clip;
                s.source.clip.name = s.clipName;
                s.source.volume = s.Volume;
                s.source.pitch = s.Pitch;
                s.source.loop = s.Loop;
            }
        }

        private void Update()
        {
            foreach (Sound sound_ in sounds)
                sound_.playedThisFrame = false;
        }

        #endregion



        #region [METHODS] Public

        #region Play Audio
        /// <summary>
        /// Plays the specified audio clip
        /// </summary>
        /// <param name="name">Name of the audio clip</param>
        public void Play(string name)
        {
            Sound s = GetSound(name);
            if (s == null)
            {
                Debug.LogWarning("Sound " + name + " doesn't exist!");
                return;
            }
            if (s.Loop)
                s.source.Play();
            else
            {
                if (!s.playedThisFrame)
                {
                    s.source.PlayOneShot(s.clip);
                    s.playedThisFrame = true;
                }
            }
        }

        /// <summary>
        /// Checks if the specified audio clip is playing
        /// </summary>
        /// <param name="audioName">Name of the audio clip</param>
        /// <returns>If the audio clip is playing</returns>
        internal bool IsAudioPlaying(string audioName)
        {
            return GetSound(audioName).source.isPlaying;
        }
        #endregion


        #region Pause Audio
        /// <summary>
        /// Pauses the specified audio clip
        /// </summary>
        /// <param name="name">Name of the audio clip</param>
        /// <param name="pause">If the clip should be paused/resumed</param>
        public void Pause(string name, bool pause)
        {
            Sound s = GetSound(name);

            if (s == null)
                Debug.LogWarning("Sound " + name + " doesn't exist!");

            if (pause)
                s.source.Pause();
            else if (!pause)
                s.source.UnPause();
        }

        /// <summary>
        /// Unpauses the specified audio clip
        /// </summary>
        /// <param name="name">Name of the audio clip</param>
        public void Unpause(string name)
        {
            Sound s = GetSound(name);

            if (s == null)
                Debug.LogWarning("Sound " + name + " doesn't exist!");

            s.source.UnPause();
        }

        /// <summary>
        /// Pauses all audio clips
        /// </summary>
        /// <param name="pause">If the clips should be paused/resumed</param>
        public void PauseAllSounds(bool pause = true)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i])
                {
                    if (pause)
                        sounds[i].source.Pause();
                    else if (!pause)
                        sounds[i].source.Play();
                }
            }
        }

        /// <summary>
        /// Pauses all playing audio clips
        /// </summary>
        /// <param name="pause">If playing clips should be paused/resumed</param>
        public void PausePlayingSounds(bool pause = true)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i])
                {
                    if (sounds[i].source.isPlaying && pause)
                        sounds[i].source.Pause();
                    else if (!pause)
                        sounds[i].source.UnPause();
                }
            }

            for (int i = 0; i < m_ScavAudioSources.Count; i++)
            {
                if (m_ScavAudioSources[i])
                {
                    if (m_ScavAudioSources[i].isPlaying && pause)
                        m_ScavAudioSources[i].Pause();
                    else if (!pause)
                        m_ScavAudioSources[i].UnPause();
                }
            }
        }
        #endregion

        #region Stop Audio
        /// <summary>
        /// Stop the specified audio clip
        /// </summary>
        /// <param name="name">Name of the audio clip</param>
        public void Stop(string name)
        {
            Sound s = GetSound(name);
            if (s == null)
            {
                Debug.LogWarning("Sound " + name + " doesn't exist!");
                return;
            }
            s.source.Stop();
        }

        /// <summary>
        /// Stop all audio clips
        /// </summary>
        public void StopAll()
        {
            foreach (Sound sound in sounds)
                sound.source.Stop();
        }
        #endregion



        #region Audio Channels

        /// <summary>
        /// Adjusts the global volume
        /// </summary>
        /// <param name="val">New global volume level</param>
        public void ChangeChannelVolume(string name, float val)
        {
            AudioChannel channelRef = new AudioChannel();
            channelRef._ChannelName = null;
            channelRef._Enabled = false;

            foreach (AudioChannel ac in audioChannels)
            {
                if (ac._ChannelName.Contains(name))
                {
                    channelRef._ChannelName = ac._ChannelName;
                    channelRef._Enabled = ac._Enabled;
                }
            }
            if (!channelRef._ChannelName.Contains(name))
                return;

            m_AudioMixer.SetFloat(name, channelRef._Enabled ? val : -80);
        }

        /// <summary>
        /// Resets the channel's volume to its default value
        /// </summary>
        /// <param name="name"></param>
        public void ResetChannelVolume(string name)
        {
            AudioChannel channelRef = new AudioChannel();
            channelRef._ChannelName = null;
            channelRef._Enabled = false;
            channelRef._BaseVolume = 0;

            foreach (AudioChannel ac in audioChannels)
            {
                if (ac._ChannelName.Contains(name))
                {
                    channelRef._ChannelName = ac._ChannelName;
                    channelRef._Enabled = ac._Enabled;
                    channelRef._BaseVolume = ac._BaseVolume;
                }
            }
            if (!channelRef._ChannelName.Contains(name))
                return;

            m_AudioMixer.SetFloat(name, channelRef._Enabled ? channelRef._BaseVolume : -80);
        }

        /// <summary>
        /// Toggles an audio channel's volume
        /// </summary>
        /// <param name="name">Name of the audio channel</param>
        /// <param name="enabled">If the channel should be playing audio</param>
        public void ToggleChannelVolume(string name, bool enabled)
        {
            AudioChannel channelRef = new AudioChannel();
            channelRef._ChannelName = null;
            channelRef._Enabled = false;
            channelRef._BaseVolume = 0;

            foreach (AudioChannel ac in audioChannels)
            {
                if (ac._ChannelName.Contains(name))
                {
                    ac.SetEnabled(enabled);

                    channelRef._ChannelName = ac._ChannelName;
                    channelRef._Enabled = ac._Enabled;
                    channelRef._BaseVolume = ac._BaseVolume;
                }
            }
            if (!channelRef._ChannelName.Contains(name))
                return;

            m_AudioMixer.SetFloat(name, channelRef._Enabled ? channelRef._BaseVolume : -80);
        }

        /// <summary>
        /// Returns the current volume of an audio channel
        /// </summary>
        /// <param name="name">Name of the audio channel</param>
        /// <returns>The current volume of the channel</returns>
        public float GetVolume(string name)
        {
            float val = -1;

            m_AudioMixer.GetFloat(name, out val);
            return val;
        }

        /// <summary>
        /// Returns the status of an audio channel
        /// </summary>
        /// <param name="name">Name of the audio channel</param>
        /// <returns>If the channel is currently active</returns>
        public bool GetChannelEnabled(string name)
        {
            foreach (AudioChannel ac in audioChannels)
            {
                if (ac._ChannelName.Contains(name))
                    return ac._Enabled;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// Set the volume of the specified audio clip
        /// </summary>
        /// <param name="name">Name of the audio clip</param>
        /// <param name="vol">Normalized volume value</param>
        public void Volume(string name, float vol)
        {
            Sound s = GetSound(name);
            if (s == null)
                Debug.LogWarning("Sound " + name + " doesn't exist!");

            s.source.volume = vol;
        }

        /// <summary>
        /// Get reference to the specified audio clip
        /// </summary>
        /// <param name="audioClipName">Name of the audio clip</param>
        /// <returns>Reference to the speficied audio clip</returns>
        public Sound GetSound(string audioClipName)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i].name == audioClipName || sounds[i].clipName == audioClipName)
                    return sounds[i]; //returns audioclip when it has been found
            }
            return null; //return nothing if the attempt failed
        }

        /// <summary>
        /// Add an audiosource to the list of active sources
        /// </summary>
        /// <param name="s">Reference to an AudioSource</param>
        public void AddScavAudioSource(AudioSource s)
        {
            m_ScavAudioSources.Add(s);
        }
        #endregion
    }
}