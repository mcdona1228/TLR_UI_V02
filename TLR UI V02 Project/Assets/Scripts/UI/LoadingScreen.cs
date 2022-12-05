using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{

    public float time = 0f;
    public float startTime = 5f;

    void Start()
    {
        gameObject.SetActive(true);
        time = startTime;
    }

    private void Update()
    {
        time -= 1 * Time.deltaTime;

        if(time <= 0)
        {
            time = 0;
            gameObject.SetActive(false);
        }
    }

}
