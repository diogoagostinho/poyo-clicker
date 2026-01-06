using UnityEngine;

public class GameLockManager : MonoBehaviour
{
    public static GameLockManager Instance;

    public GameObject[] disabledButtons;
    public GameObject[] disabledButtonsDio;

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
        foreach (var b in disabledButtonsDio)
            b.SetActive(false);
    }

    public void UnlockAfterDio()
    {
        foreach (var b in disabledButtonsDio)
            b.SetActive(true);
    }

}
