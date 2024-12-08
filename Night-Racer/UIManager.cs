using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region singleton
    public static UIManager Instance;
    #endregion

    //[SerializeField] private TextMeshProUGUI lapCount;
    // all text boxes
    [SerializeField] private TextMeshProUGUI checkpoint;
    [SerializeField] private TextMeshProUGUI lapTime;
    [SerializeField] private TextMeshProUGUI lastLap;
    [SerializeField] private TextMeshProUGUI recordLap;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI countDown;

    // ui elements
    [SerializeField] private GameObject[] countDownOverlay;
    [SerializeField] private GameObject[] UI;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject speedOutline;
    [SerializeField] private GameObject mainButton;
    [SerializeField] private GameObject resumeButton;
    private float speedMeter;
    [SerializeField] public int countDownTime = 3;
    private bool isPause = false;

    [SerializeField] private AudioSource startAudio;

    KartLap kartLap;
    LapHandler lapHandler;
    Movement movementScript;

    [SerializeField] private GameObject player;
    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        StartCoroutine(StartCountDown());

        player = GameObject.FindGameObjectWithTag("Player");
        movementScript = player.GetComponent<Movement>();
        kartLap = GameObject.FindGameObjectWithTag("Player").GetComponent<KartLap>();
        lapHandler = GameObject.FindGameObjectWithTag("Finish").GetComponent<LapHandler>();
        UpdateCheckpointUI();
        UpdateLastLapUI();
        UpdateRecordLapUI();
        UpdateSpeedMeter();
    }

    private IEnumerator StartCountDown()
    {
        DeactivateUI(); // deactiveert de ui zodat deze niet overlapt met de count down
        startAudio.Play(); // start count down audio

        while(countDownTime > 0) // doet de volgende code 3x tot dat de count down time op 0 staat
        {
            for (int i = 0; i < countDownOverlay.Length; i++)
            {
                countDown.text = countDownTime.ToString(); // zet de coun down int om naar text
                countDownOverlay[i].SetActive(true); // zet de huidige count down overlay op active
                yield return new WaitForSeconds(1f); // wacht 1 seconde
                countDownOverlay[i].SetActive(false); // zet de huidige count down overlay op non active
                countDownTime--; // count down min 1
            }
        }

        // count down op 0? dan gaat de text naar go en kan de speler gaan rijden
        countDownTime = 0;
        countDown.text = "GO!";

        yield return new WaitForSeconds(1f); // wacht 1 seconde tot dat de ui gedeactiveerd wordt 
        ActivateUI(); // activeert de nieuwe ui 

        countDown.gameObject.SetActive(false); 
        for (int i = 0; i < countDownOverlay.Length; i++) // zet de hele array van begin ui op false
        {
            countDownOverlay[i].SetActive(false);
        }
    }

    private void ActivateUI()
    {
        for (int i = 0; i < UI.Length; i++) // activeert alle elementen die in de ui array zitten
        {
            UI[i].gameObject.SetActive(true);
        }
    }

    private void DeactivateUI() 
    {
        for (int i = 0; i < UI.Length; i++) // deactiveert alle elementen die in de ui array zitten
        {
            UI[i].gameObject.SetActive(false);
        }
    }

    public void UpdateSpeedMeter()
    {
        speedMeter = movementScript.currentSpeed;
        slider.value = speedMeter;
    }

    public void UpdateCheckpointUI() // Delano?
    {
        checkpoint.text = "Checkpoint: " + kartLap.CheckpointIndex.ToString();
    }

    public void UpdateLastLapUI() // Delano?
    {
        lastLap.text = "Last Lap: " + PlayerPrefs.GetFloat("LastLap").ToString("F2");
    }

    public void UpdateRecordLapUI() // Delano?
    {
        recordLap.text = "Record Lap: " + PlayerPrefs.GetFloat("RecordTime").ToString("F2");
    }

    public void PauseGame() // pauseerd de game 
    {
        if (isPause) 
        {
            Time.timeScale = 0f;
            mainButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(true);
            DeactivateUI();
        }
        else
        {
            Time.timeScale = 1;
            mainButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(false);
            ActivateUI();
        }
    }

    public void ResumeButton()
    {
        isPause = !isPause;
        PauseGame();
    }

    void Update()
    {
        lapTime.text = "Laptime: " + lapHandler.lapTime.ToString("F2"); // Delano?

        UpdateSpeedMeter();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            PauseGame();
        }
    }
}
