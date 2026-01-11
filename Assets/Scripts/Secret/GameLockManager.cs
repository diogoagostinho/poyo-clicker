using UnityEngine;
using UnityEngine.UI;

public class GameLockManager : MonoBehaviour
{
    public static GameLockManager Instance;

    public GameObject[] disabledButtons;
    public GameObject[] disabledButtonsDio;

    public Button[] dioButtons;

    void Awake()
    {
        Instance = this;
    }

    public void LockForSecretBoss() 
    {
        foreach (var b in disabledButtons)
            b.SetActive(false);
    }

    public void UnlockAfterSecretBoss()
    {
        foreach (var b in disabledButtons)
            b.SetActive(true);
    }

    public void LockForDio()
    {
        foreach (var b in dioButtons)
            b.interactable = false;
    }

    public void UnlockAfterDio()
    {
        foreach (var b in dioButtons)
            b.interactable = true;
    }

}
