using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseFunctionality : MonoBehaviour
{
    public void BackToMainMenu()
    {
        ScenesManager.instance.ChangeToScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        foreach (GameObject go in PlayerSpawning.instance.players)
        {
            if (go != null)
            {
                go.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                go.GetComponent<PlayerInput>().defaultActionMap = "Player";
            }

        }
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }
}
