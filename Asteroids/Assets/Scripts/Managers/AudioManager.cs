using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private List<AudioSource> sounds;

    public enum Sounds
    {
        Fire,
        Thrust,
        LargeExplosion,
        MediumExplosion,
        SmallExplosion
    }

    public void PlaySound(Sounds sound)
    {
        sounds[(int)sound].Play();
    }
}
