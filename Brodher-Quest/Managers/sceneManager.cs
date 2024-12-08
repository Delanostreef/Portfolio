using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour
{
    //Laad de scene die in de inspector ingevuld is
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    //Quit de game
    public void Quit()
    {
        Debug.Log("test");
        Application.Quit();
    }

    //Ga naar de main menu
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
