using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss Data")]
public class BossData : ScriptableObject
{
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

    [Header("Difficulty")]
    public float baseHealth = 100f;
    public float healthMultiplier = 1.2f;
}
