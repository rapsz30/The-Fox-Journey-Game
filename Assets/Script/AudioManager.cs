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

        }
        else
        {
            Destroy(gameObject);
        }
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
