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
    private string author;
    private string musicName;
    private float mainVolume;

    void Start()
    {
        PlayMusic();
        mainVolume = myAudioSource.volume;
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

    void Update()
    {
        if (myAudioSource.time >= myAudioSource.clip.length - 2f)
        {
            PlayMusic();
        }

        if (myAudioSource.time >= myAudioSource.clip.length - 12f)
        {
            UIMusicController.GetInstance.SetMusicName(author, musicName);
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
        author = temp.Author;
        musicName = sound.Name;
        myAudioSource.Play();
        

    }


    public void onDieVolume()
    {
        myAudioSource.volume = mainVolume * 0.4f;
    }

    public void resetVolume()
    {
        myAudioSource.volume = mainVolume;
    }
    

}

