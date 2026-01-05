using UnityEngine;

public class CustomizeTabs : MonoBehaviour
{
    public GameObject skinsContent;
    public GameObject backgroundsContent;
    public GameObject musicContent;

    public GameObject skinsScrollView;
    public GameObject backgroundsScrollView;
    public GameObject musicScrollView;

    void Start()
    {
        ShowSkins();
    }

    public void ShowSkins()
    {
        skinsContent.SetActive(true);
        backgroundsContent.SetActive(false);
        musicContent.SetActive(false);

        skinsScrollView.SetActive(true);
        backgroundsScrollView.SetActive(false);
        musicScrollView.SetActive(false);
    }

    public void ShowBackgrounds()
    {
        skinsContent.SetActive(false);
        backgroundsContent.SetActive(true);
        musicContent.SetActive(false);

        skinsScrollView.SetActive(false);
        backgroundsScrollView.SetActive(true);
        musicScrollView.SetActive(false);
    }

    public void ShowMusic()
    {
        skinsContent.SetActive(false);
        backgroundsContent.SetActive(false);
        musicContent.SetActive(true);

        skinsScrollView.SetActive(false);
        backgroundsScrollView.SetActive(false);
        musicScrollView.SetActive(true);
    }
}
