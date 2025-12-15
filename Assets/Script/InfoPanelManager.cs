using UnityEngine;
using UnityEngine.UI;
//loww
public class InfoPanelManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject panel;     // Panel popup (nama: Panel)
    public GameObject infoButton; // Tombol "Info" (nama: Info)
    public GameObject closeButton; // Tombol X (nama: Close)

    void Start()
    {
        // Panel dimulai dalam keadaan mati
        if (panel != null)
            panel.SetActive(false);

        // Tombol Info harus aktif
        if (infoButton != null)
            infoButton.SetActive(true);
    }

    // Ketika tombol Info ditekan
    public void ShowPanel()
    {
        if (panel != null)
            panel.SetActive(true);

        if (infoButton != null)
            infoButton.SetActive(false);
    }

    // Ketika tombol Close ditekan
    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);

        if (infoButton != null)
            infoButton.SetActive(true);
    }
}
