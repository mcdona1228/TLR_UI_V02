using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

//Spawns the player in different locations
public class PlayerSpawning : MonoBehaviour
{
    public static PlayerSpawning instance;

    public GameObject[] players = new GameObject[4];
    public Transform[] hoverInfo = new Transform[4];
    public Material[] mats = new Material[4];
    //public InventoryObject[] inv = new InventoryObject[4];

    public Transform[] JoinSpawnPos = new Transform[4];
    public Transform[] MenuSpawnPos = new Transform[4];
    public Transform[] GameSpawnPos = new Transform[4];

    public bool lockedIn = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);


    }

    public void ChangePlayerInput()
    {
        foreach(GameObject go in players)
        {
            if (go != null)
            {
                int Index = System.Array.IndexOf(players, go);

                // Different spawns for each scene
                if (SceneManager.GetActiveScene().name == "Join")
                {
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                    print("calling");
                    go.GetComponent<PlayerInput>().defaultActionMap = "UI";
                }
                else if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                    print("calling");
                    go.GetComponent<PlayerInput>().defaultActionMap = "UI";
                }
                else if (SceneManager.GetActiveScene().name == "Game")
                {
                    print("pizza");
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                    go.GetComponent<PlayerInput>().defaultActionMap = "Player";
                    go.GetComponent<PlayerPainting>().enabled = false;
                }
                else if (SceneManager.GetActiveScene().name == "Credits")
                {
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                    print("calling");
                    go.GetComponent<PlayerInput>().defaultActionMap = "UI";
                }
            }
        }
    }

    public void StartingPositions()
    {
        foreach (GameObject go in players)
        {
            if (go != null)
            {
                int Index = System.Array.IndexOf(players, go);


                if (SceneManager.GetActiveScene().name == "Game")
                {
                    go.transform.position = GameSpawnPos[Index].position;
                }
            }
        }
    }

    public void SetInitialPlayerValues()
    {
        foreach(GameObject go in players)
        {
            if (go != null)
            {
                int Index = System.Array.IndexOf(players, go);
                go.transform.position = JoinSpawnPos[Index].position;

                go.transform.eulerAngles = Vector3.zero;
                go.transform.GetChild(0).GetComponent<MeshRenderer>().material = mats[Index];
                //go.GetComponent<CharacterMovement>().inventory = inv[Index];

                
            }

            //in future change to switch

        }
        //FindObjectOfType<PauseHunter>().pauseHunt();
    }
}
