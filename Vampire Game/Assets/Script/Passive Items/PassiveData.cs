using UnityEngine;

[CreateAssetMenu(fileName = "Passive Data", menuName = "2D Top-down Rogue-like/Passive Data")]
public class PassiveData : ItemData
{
    public Passive.Modifier baseStats;
    public Passive.Modifier[] growth;

    public Passive.Modifier GetLevelData(int level)
    {
        // Pick the stats from the next Level.
        if (level - 2 < growth.Length)
            return growth[level - 2];

        // Return an empty value and a warning.
        Debug.LogWarning(string.Format("Passive doesn't have its Level up stats configured for Level {0}!", level));
        return new Passive.Modifier();
    }
}
