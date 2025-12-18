using UnityEngine;
using UnityEngine.UI;
public class InfoPanelManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject panel;     
    public GameObject infoButton; 
    public GameObject closeButton; 

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);

        if (infoButton != null)
            infoButton.SetActive(true);
    }

    public void ShowPanel()
    {
        if (panel != null)
            panel.SetActive(true);

        if (infoButton != null)
            infoButton.SetActive(false);
    }
    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);

        if (infoButton != null)
            infoButton.SetActive(true);
    }
}
