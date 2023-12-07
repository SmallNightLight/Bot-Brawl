using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private SoundEffectGameEvent _raiser;
    [SerializeField] private SoundEffectReference _sound;

    public void Raise()
    {
        _raiser.Raise(_sound.Value);
    }
}