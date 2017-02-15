using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AudioSource))]
class BackgroundMusicControll : MonoBehaviour
{
    private static BackgroundMusicControll musicControll;

    public static BackgroundMusicControll GetInstance
    {
        get { return musicControll; }
    }

    [SerializeField] private SimpleSoundObject[] clips;


    private AudioClip nowClip;
    private AudioSource myAudioSource;

    void Start()
    {
        PlayMusic();
    }

    void Awake()
    {
        if (musicControll != null)
        {
            Destroy(gameObject);
        }
        else
        {
            musicControll = this;
            DontDestroyOnLoad(gameObject);
            myAudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        }
        
    }

    private SimpleSoundObject GetRandomClip()
    {
        return clips.Length == 0 ? null : clips[Random.Range(0, clips.Length)];
    }


    public void PlayMusic()
    {
        if (myAudioSource.isPlaying)
        {
           myAudioSource.Stop();
        }

        var temp = GetRandomClip();
        if (temp == null)
        {
            return;
        }

        var sound = temp.getRandomSound;

        if (sound == null)
        {
            return;
        }


        myAudioSource.clip = sound.Music;

        UIMusicController.GetInstance.SetMusicName(temp.Author, sound.Name);
        myAudioSource.Play();
        

    }

    

}

