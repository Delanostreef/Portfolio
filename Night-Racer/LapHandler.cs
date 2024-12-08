using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class LapHandler : MonoBehaviour
{
    // Delano als je dit ziet zou je hier comments in willen zetten?


    [SerializeField] private int cpAmmount;

    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject restartButton;

    public GhostHolder holder;
    public float lapTime;

    [SerializeField] private GameObject player;
    Movement movementScript;


    private void Awake()
    {
        PlayerPrefs.SetFloat("LastLap", 0);
        
        
    }
    private void Start()
    {
        lapTime = -3f;
    }
    private void FixedUpdate()
    {
        lapTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveToJSON();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnFinish(other);
    }

    private void OnFinish(Collider other)
    {
        if (other.GetComponent<KartLap>())
        {
            KartLap kart = other.GetComponent<KartLap>();

            if (kart.CheckpointIndex == cpAmmount)
            {
                Restart();
                kart.CheckpointIndex = 0;
                UIManager.Instance.UpdateCheckpointUI();
                SaveLastLapTime();
                SaveRecordTime();
                ResetLapTime();
            }
        }
    }

    private void ResetLapTime()
    {
        lapTime = 0;
    }
    private void Restart()
    {
        mainMenuButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    private void SaveLastLapTime()
    {
        PlayerPrefs.SetFloat("LastLap", lapTime);
        UIManager.Instance.UpdateLastLapUI();
    }

    private void SaveRecordTime()
    {
        float recordTime = PlayerPrefs.GetFloat("RecordTime");

        if (recordTime == 0 || lapTime <= PlayerPrefs.GetFloat("RecordTime"))
        {
            PlayerPrefs.SetFloat("RecordTime", lapTime);
            UIManager.Instance.UpdateRecordLapUI();
            Debug.Log(PlayerPrefs.GetFloat("RecordTime"));
        }
    }

    public void SaveToJSON()
    {
        string ghostPos = JsonUtility.ToJson(holder.ghostPos);
        string filePath = Application.persistentDataPath + "/GhostPos.data";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, ghostPos);
        Debug.Log("Ghost Saved");
    }
}
