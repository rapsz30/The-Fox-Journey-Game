using UnityEngine;

public class SampleSceneBGM : MonoBehaviour
{
    public AudioClip sampleSceneBGM;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ChangeBGM(sampleSceneBGM);
        }
    }
}
