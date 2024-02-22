using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;

        private AudioSource audioSource;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayMusic(AudioClip audioClip)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
