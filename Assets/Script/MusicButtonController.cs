using UnityEngine;
using UnityEngine.UI; 

public class MusicButtonController : MonoBehaviour
{
    private Image buttonImage;
    private AudioSource backgroundMusic; // <-- BARIS BARU: Komponen AudioSource
    
    // Seret gambar icon "Music ON" ke slot ini di Inspector
    public Sprite musicOnSprite; 
    // Seret gambar icon "Music OFF" ke slot ini di Inspector
    public Sprite musicOffSprite; 

    void Start()
    {
        buttonImage = GetComponent<Image>();
        
        // Coba dapatkan AudioSource, asumsikan berada pada objek yang sama
        // Jika musik berada di objek lain, ganti GetComponent<> dengan FindObjectOfType<> atau deklarasi publik
        backgroundMusic = FindObjectOfType<AudioSource>(); 
        
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play(); // Mulai musik jika belum berjalan
        }
        
        // Panggil update saat game dimulai untuk mencocokkan status volume awal
        UpdateSprite(AudioListener.volume > 0);
    }
    
    // Dipanggil oleh GameManager setelah ToggleMute()
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