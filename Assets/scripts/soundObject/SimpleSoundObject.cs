using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SimpleSoundObject {

    [Serializable]
    public class Sound
    {
        [SerializeField] private string name;
        [SerializeField] private AudioClip sound;

        public string Name { get { return name; } }
        public AudioClip Music { get { return sound; } }
    }


    [SerializeField] string author;
    [SerializeField] private Sound[] clips;



    public string Author { get { return author; } }

    public Sound getRandomSound
    {
        get { return clips.Length == 0 ? null : clips[Random.Range(0, clips.Length)]; }
    }

    


}
