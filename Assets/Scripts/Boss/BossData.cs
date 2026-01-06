using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss Data")]
public class BossData : ScriptableObject
{
    public string bossName;
    public Sprite bossSprite;
    public AudioClip bossMusic;
    public bool flipsUI;

    [Header("Difficulty")]
    public float baseHealth = 100f;
    public float healthMultiplier = 1.2f;
}
