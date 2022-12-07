using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class JoinFunctionality : MonoBehaviour
{
    // Choose personalities stuff
    //public GameObject nextArrayBttn;
    //public GameObject previousArrayBttn;
    public List<TextMeshProUGUI> personalityTypeTexts;
    public int index;

    // Player know how to join
    public GameObject JoinGame;
    public GameObject RobotTypes;
    private GameObject playerSpawnHelpIndex;


    private void Start()
    {
        

    }

    public void OnJoin()
    {
        if (SceneManager.GetActiveScene().name == "Join")
        {
            int playerIndex = System.Array.IndexOf(PlayerSpawning.instance.players, gameObject) + 1;
            playerSpawnHelpIndex = GameObject.Find("hi_spawn_" + playerIndex);
            //JoinGame = playerSpawnHelpIndex.transform.Find("JoinGame").gameObject;
            JoinGame = playerSpawnHelpIndex.transform.GetChild(0).GetChild(0).gameObject;
            //RoboTypes = playerSpawnHelpIndex.transform.Find("RoboTypes").gameObject;
            foreach (var item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                //print(item.name);
                if (item.name.Contains("RobotTypes_" + playerIndex))
                {
                    print(item.transform.parent.transform.parent.name);
                    
                        
                    RobotTypes = item;
                }
            }
            //RobotTypes.SetActive(false);

            JoinGame.SetActive(false);
            RobotTypes.SetActive(true);

            foreach (var item in playerSpawnHelpIndex.GetComponentsInChildren<TextMeshProUGUI>())
            {
                personalityTypeTexts.Add(item);
                item.gameObject.SetActive(false);
            }
            personalityTypeTexts[0].gameObject.SetActive(true);
        }
    }

    public void OnNextTypeRobo(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (SceneManager.GetActiveScene().name == "Join")
            {
                print("Join arrow");
                personalityTypeTexts[index].gameObject.SetActive(false);
                index = (index + 1) % personalityTypeTexts.Count;
                personalityTypeTexts[index].gameObject.SetActive(true);
            }
        }
    }

    public void OnPreviousTypeRobo(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (SceneManager.GetActiveScene().name == "Join")
            {
                print("Join arrow");
                personalityTypeTexts[index].gameObject.SetActive(false);
                if(index == 0)
                {
                    index = personalityTypeTexts.Count - 1;
                }
                else
                {
                    index -= 1;
                }
                personalityTypeTexts[index].gameObject.SetActive(true);
            }
        }
    }

}
