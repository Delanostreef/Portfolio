using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAmmo : MonoBehaviour
{
    public GameObject prefab;

    void spawnPrefab()
    {
        Instantiate(prefab);
    }
}
