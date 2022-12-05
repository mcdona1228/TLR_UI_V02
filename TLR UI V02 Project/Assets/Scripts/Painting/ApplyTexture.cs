using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTexture : MonoBehaviour
{
    public void SaveTextureAndStartGame()
    {
        print("Started Game");
        ScenesManager.instance.StartGameScene();
    }
}
