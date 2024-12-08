using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{
    public Animator transition;

    // Vul in de inspector in wat het huide level is
    [SerializeField]
    private int currentLevel;

    [Header("LevelToLoad")]
    // Bepaalt naar welk level de player gaat.
    [SerializeField] private string levelName;
    void Start()
    {
        //Maak een player pref aan als die nog niet bestaat.
        PlayerPrefs.SetInt("lastLevel", PlayerPrefs.GetInt("lastLevel", 1));
        PlayerPrefs.Save();
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        // Check of de player de trigger entered.
        if (player.CompareTag("Player"))
        {
            // Check of het huide level gelijk is last level
            if (currentLevel > PlayerPrefs.GetInt("lastLevel"))
            {
                //Zet het LastLevel omhoog
                PlayerPrefs.SetInt("lastLevel", currentLevel);
                PlayerPrefs.Save();

                //  Save de coins

                CoinManager.instance.Save();
            }

            StartCoroutine(LoadLevel()); 
        }
    }

    IEnumerator LoadLevel()
    {
        //Zet de transition animation aan
        transition.SetTrigger("LevelEnd");
        //Wacht 1 seconden voor de animation
        yield return new WaitForSeconds(1);
        // Ga naar de volgende scene
        SceneManager.LoadScene(levelName);
    }
}
