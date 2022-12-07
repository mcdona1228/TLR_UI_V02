using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHunter : MonoBehaviour
{
    [HideInInspector] public GameObject pauseMenu;
    [HideInInspector] public GameObject settingsMenu;
    [HideInInspector] public GameObject mainMenu;

    private void Start()
    {
        PauseHunt();
    }


    public void PauseHunt()
    {
        if(pauseMenu == null || settingsMenu == null || mainMenu == null)
        {
            foreach(GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if(go.name == "Settings")
                {
                    settingsMenu = go;
                }
                if(go.name == "MainMenuButtons")
                {
                    mainMenu = go;
                }
                if (go.name == "MainMenuButtons")
                {
                    mainMenu = go;
                }
            }
        }
        foreach (PauseGameMenu pgm in FindObjectsOfType<PauseGameMenu>())
        {
            if(pgm.pauseGameMenu == null || settingsMenu == null)
            {
                if(SceneManager.GetActiveScene().name != "MainMenu")
                {
                    pgm.pauseGameMenu = gameObject;
                }
                
                pgm.settingsMenu = settingsMenu;
            }

            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                pgm.mainMenu = mainMenu;
            }
            
        }

        if(SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Join")
        {
            gameObject.SetActive(false);
        }
    }
}
