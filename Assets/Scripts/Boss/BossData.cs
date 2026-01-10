using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss Data")]
public class BossData : ScriptableObject
{
    public string bossID;
    public string bossName;
    public Sprite bossSprite;
    public AudioClip bossMusic;

    [Header("Special Abilities")]
    public bool flipsUI;

    [Header("Point Drain Ability")]
    public bool drainPointsOnStart;
    public int minPointDrain = 100;
    public int maxPointDrain = 30000;
    public Sprite pointsIconOverride;

    [Header("Multi-Phase Boss")]
    public bool hasSecondPhase;

    [Header("Second Phase Settings")]
    public string secondPhaseName;
    public Sprite secondPhaseSprite;
    public float secondPhaseHealthMultiplier = 3f;

    [Header("Special Boss: Dio")]
    public bool isDio;
    public AudioClip spawnSfx;

    [Tooltip("Grayscale material or shader for Dio fight")]
    public Material dioGrayscaleMaterial;

    [Header("Difficulty")]
    public float baseHealth = 100f;
    public float healthMultiplier = 1.2f;
}
