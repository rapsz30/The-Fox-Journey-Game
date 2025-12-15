using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource;
    private bool isMuted = false;

    private void Awake()
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

        if (!bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // ⭐ FUNGSI GANTI BGM
    public void ChangeBGM(AudioClip newClip)
    {
        if (bgmSource.clip == newClip) return;

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        bgmSource.mute = isMuted;
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
