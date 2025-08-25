using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> music;
    private List<AudioClip> shuffledMusic;
    public TextMeshProUGUI textbox;
    private AudioSource audioSource;
    private int currentSongIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (music == null || music.Count == 0)
        {
            textbox.text = "No music loaded";
            return;
        }

        shuffledMusic = music.OrderBy(x => Random.value).ToList();

        PlayCurrentSong();
    }

    void Update()
    {
        if (audioSource.isPlaying && audioSource.clip != null)
        {
            textbox.text = audioSource.clip.name;
        }

        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            PlayNextSong();
        } 
    }

    void PlayCurrentSong() {
        if (currentSongIndex >= shuffledMusic.Count())
        {
            return;
        }

        audioSource.clip = shuffledMusic[currentSongIndex];
        audioSource.Play();
    }

    public void PlayNextSong()
    {
        currentSongIndex++;

        if (currentSongIndex > shuffledMusic.Count())
        {
            currentSongIndex = 0;
        }

        PlayCurrentSong();
    }
}
