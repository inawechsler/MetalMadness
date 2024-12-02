using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;

    [HideInInspector] public AudioSource source;
}

[System.Serializable]
public class SceneMusic
{
    public string sceneName;
    public string musicName;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    public SceneMusic[] sceneMusics;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        PlaySceneMusic(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    public void PlaySound(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Este sonido '" + name + "' no está");
            return;
        }
        sound.source.Play();
    }

    public void StopSound(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Este sonido '" + name + "' no está");
            return;
        }
        sound.source.Stop();
    }

    public void PlayMusic(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Este sonido '" + name + "' no está");
            return;
        }

        StopAllMusic();
        sound.source.Play();
    }

    public void StopAllMusic()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void PlaySceneMusic(string sceneName)
    {
        SceneMusic sceneMusic = System.Array.Find(sceneMusics, sm => sm.sceneName == sceneName);
        if (sceneMusic != null)
        {
            PlayMusic(sceneMusic.musicName);
        }
        else
        {
            Debug.LogWarning("No hay música asignada para la escena '" + sceneName + "'.");
        }
    }
}