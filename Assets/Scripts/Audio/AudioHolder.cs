using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class AudioDataDictionary : SerializableDictionaryBase<string, AudioClip> { }
[CreateAssetMenu(fileName = "Audio Data Holder", menuName = "Audio Holder")]
public class AudioHolder : ScriptableObject
{
    [SerializeField] private AudioDataDictionary audio = new AudioDataDictionary();

    public AudioClip GetClip(string id)
    {
        return audio[id];
    }

}
