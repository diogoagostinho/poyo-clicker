using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBallManager : MonoBehaviour
{
    public static DragonBallManager Instance;

    [Header("Spawn")]
    public GameObject dragonBallPrefab;
    public RectTransform spawnArea;
    public float minSpawnTime = 20f;
    public float maxSpawnTime = 45f;

    [Header("Inventory UI")]
    public Transform collectedUIParent;

    [Header("State")]
    public bool canSpawn = true;

    Dictionary<DragonBallType, DragonBallData> collected =
        new Dictionary<DragonBallType, DragonBallData>();

    GameObject currentSpawnedBall;
    [Header("Dragon Balls")]
    public List<DragonBallData> allDragonBalls;

    BossManager bossManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bossManager = FindFirstObjectByType<BossManager>();
        StartCoroutine(SpawnRoutine());

        foreach (var savedType in SaveManager.Instance.data.collectedDragonBalls)
        {
            DragonBallData d = allDragonBalls.Find(b => b.type == savedType);

            if (d != null && !collected.ContainsKey(savedType))
            {
                collected.Add(savedType, d);
                AddToUI(d);
            }
        }

    }

    IEnumerator SpawnRoutine()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            if (collected.Count >= 7)
                yield break;

            if (CanSpawnBall())
            {
                SpawnBall();
            }
        }
    }

    bool CanSpawnBall()
    {
        if (bossManager != null && bossManager.IsBossActive)
            return false;

        if (GogetaBossManager.Instance != null &&
            GogetaBossManager.Instance.IsActive)
            return false;

        if (!canSpawn)
            return false;

        return true;

    }

    public void ClearActiveBall()
    {
        if (currentSpawnedBall != null)
        {
            Destroy(currentSpawnedBall);
            currentSpawnedBall = null;
        }
    }

    void SpawnBall()
    {
        if (currentSpawnedBall != null)
            return;

        Vector2 pos = new Vector2(
            Random.Range(spawnArea.rect.xMin, spawnArea.rect.xMax),
            Random.Range(spawnArea.rect.yMin, spawnArea.rect.yMax)
        );

        DragonBallData nextBall = GetNextUncollectedBall();
        if (nextBall == null)
            return;

        currentSpawnedBall = Instantiate(dragonBallPrefab, spawnArea);
        currentSpawnedBall.GetComponent<RectTransform>().anchoredPosition = pos;

        currentSpawnedBall.GetComponent<DragonBall>().Init(nextBall);
    }

    DragonBallData GetNextUncollectedBall()
    {
        //List<DragonBallData> available = new List<DragonBallData>();

        foreach (var ball in allDragonBalls)
        {
            if (!collected.ContainsKey(ball.type))
            {
                return ball;
            }
        }
        return null;
    }


    public void CollectBall(DragonBallData data)
    {
        if (data == null)
        {
            Debug.LogError("DragonBallData is NULL!");
            return;
        }

        if (collected.ContainsKey(data.type))
            return;

        collected.Add(data.type, data);
        AddToUI(data);

        currentSpawnedBall = null;

        if (!SaveManager.Instance.data.collectedDragonBalls.Contains(data.type))
        {
            SaveManager.Instance.data.collectedDragonBalls.Add(data.type);
            SaveManager.Instance.Save();
        }

        if (collected.Count == 7)
            TriggerGogeta();
    }


    void AddToUI(DragonBallData data)
    {
        GameObject img = new GameObject("BallUI", typeof(RectTransform), typeof(UnityEngine.UI.Image));
        img.transform.SetParent(collectedUIParent, false);

        RectTransform rt = img.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(40, 40); // icon size (adjust if you want)
        rt.localScale = Vector3.one;

        var image = img.GetComponent<UnityEngine.UI.Image>();
        image.sprite = data.icon;
        image.preserveAspect = true;
    }

    void TriggerGogeta()
    {
        canSpawn = false;

        SecretBossVideoPlayer.Instance.PlaySBVideo();
        //GogetaBossManager.Instance.StartGogetaFight();
    }

    public void ResetForPrestige()
    {
        collected.Clear();
        canSpawn = true;

        foreach (Transform child in collectedUIParent)
            Destroy(child.gameObject);

        StartCoroutine(SpawnRoutine());
    }

    public void ClearCollectedUI()
    {
        collected.Clear();

        foreach (Transform child in collectedUIParent)
            Destroy(child.gameObject);
    }

}
