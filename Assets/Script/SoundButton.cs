using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public GameObject iconOn;
    public GameObject iconOff;

    void Start()
    {
        bool muted = AudioManager.instance.IsMuted();
        iconOn.SetActive(!muted);
        iconOff.SetActive(muted);
    }

    public void Toggle()
    {
        AudioManager.instance.ToggleSound();

        bool muted = AudioManager.instance.IsMuted();
        iconOn.SetActive(!muted);
        iconOff.SetActive(muted);
    }
}
