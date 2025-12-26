using UnityEngine;

public class CustomizeTabs : MonoBehaviour
{
    public GameObject skinsContent;
    public GameObject backgroundsContent;
    public GameObject musicContent;

    void Start()
    {
        ShowSkins();
    }

    public void ShowSkins()
    {
        skinsContent.SetActive(true);
        backgroundsContent.SetActive(false);
        musicContent.SetActive(false);
    }

    public void ShowBackgrounds()
    {
        skinsContent.SetActive(false);
        backgroundsContent.SetActive(true);
        musicContent.SetActive(false);
    }

    public void ShowMusic()
    {
        skinsContent.SetActive(false);
        backgroundsContent.SetActive(false);
        musicContent.SetActive(true);
    }
}
