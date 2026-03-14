using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] public AudioSource armHitAudio;
    [SerializeField] public AudioSource doorAudio;
    // Start is called before the first frame update
    void Start()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
