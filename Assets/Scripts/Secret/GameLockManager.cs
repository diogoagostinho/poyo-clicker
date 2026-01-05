using UnityEngine;

public class GameLockManager : MonoBehaviour
{
    public static GameLockManager Instance;

    public GameObject[] disabledButtons;

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
}
