using System.Collections.Generic;
using UnityEngine;

// [System.Serializable] tells Unity to let us see and edit this custom class in the Inspector
[System.Serializable]
public class AudioEntry
{
    public string name;      // The exact name you will type in Yarn (e.g., "FrisscityCalm")
    public AudioClip clip;   // The actual audio file you drag and drop in the Inspector
}

public class AudioManager : MonoBehaviour
{
    [Header("Audio Files")]
    // These lists are what you will actually see and fill out in the Unity Inspector
    [SerializeField] private List<AudioEntry> music = new List<AudioEntry>();
    [SerializeField] private List<AudioEntry> sfx = new List<AudioEntry>();

    // Dictionaries are hidden from the Inspector, but they allow the code to 
    // look up your audio files instantly during gameplay without lagging.
    private Dictionary<string, AudioClip> musicDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // When the game starts, this loop copies all the music from your Inspector List 
        // into the Dictionary for lightning-fast lookups.
        foreach (var entry in music)
        {
            musicDictionary[entry.name] = entry.clip;
        }

        // Does the exact same thing for your Sound Effects
        foreach (var entry in sfx)
        {
            sfxDictionary[entry.name] = entry.clip;
        }
    }

    // Your VisualNovel.cs script calls this method when it needs a background track
    public AudioClip GetMusic(string name)
    {
        if (musicDictionary.TryGetValue(name, out AudioClip clip))
        {
            return clip; // Found it! Send the audio clip back.
        }

        // If you make a typo in Yarn, this warns you exactly which file is missing!
        Debug.LogWarning($"Music '{name}' was not found in the AudioManager.");
        return null;
    }

    // Your VisualNovel.cs script calls this method when it needs a sound effect
    public AudioClip GetSFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out AudioClip clip))
        {
            return clip; // Found it! Send the audio clip back.
        }

        Debug.LogWarning($"SFX '{name}' was not found in the AudioManager.");
        return null;
    }
}