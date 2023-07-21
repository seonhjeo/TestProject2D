using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    [SerializeField] private Transform pfSpider;

    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            Transform spiderTransform = Instantiate(pfSpider, new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f)), Quaternion.identity);
        }
    }
}
