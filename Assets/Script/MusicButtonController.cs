using UnityEngine;
using UnityEngine.UI; 

public class MusicButtonController : MonoBehaviour
{
    private Image buttonImage;
    private AudioSource backgroundMusic; 

    public Sprite musicOnSprite; 
    public Sprite musicOffSprite; 

    void Start()
    {
        buttonImage = GetComponent<Image>();

        backgroundMusic = FindObjectOfType<AudioSource>(); 
        
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
        UpdateSprite(AudioListener.volume > 0);
    }
    public void UpdateSprite(bool isMusicOn)
    {
        if (buttonImage == null) return;
        
        if (isMusicOn)
        {
            buttonImage.sprite = musicOnSprite;
        }
        else
        {
            buttonImage.sprite = musicOffSprite;
        }
    }
}