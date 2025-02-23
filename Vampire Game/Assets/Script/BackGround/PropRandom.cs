using System.Collections.Generic;
using UnityEngine;

public class PropRandom : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject sp in propSpawnPoints) 
        {
            int ran = Random.Range(0,propPrefabs.Count);
            Instantiate(propPrefabs[ran], sp.transform.position, Quaternion.identity);
        }

    }
}
