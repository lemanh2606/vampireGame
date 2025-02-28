using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multipler;
    
    public float Multipler { get => multipler; private set => multipler = value; }

    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefabs;
    public GameObject NextLevelPrefabs { get => nextLevelPrefabs; private set => nextLevelPrefabs = value; }

    [SerializeField]
    Sprite icon; // Not mean modified in game [Only in editor]
    public Sprite Icon { get => icon; private set => icon = value; }
}
