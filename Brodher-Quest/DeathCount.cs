using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCount : MonoBehaviour
{

    private TextMeshProUGUI deathCount;

    void Start()
    {
        deathCount = GetComponent<TextMeshProUGUI>();
        PlayerPrefs.SetInt("DeathCount", PlayerPrefs.GetInt("DeathCount", 1));
        PlayerPrefs.Save();
    }

    void Update()
    {
        Debug.Log(PlayerPrefs.GetInt("DeathCount"));
        deathCount.text = PlayerPrefs.GetInt("DeathCount").ToString();
    }
}
