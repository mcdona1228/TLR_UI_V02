using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform lookAt;
    private Transform localTrans;

    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    void Update()
    {
        if (lookAt)
        {
            localTrans.LookAt(2 * localTrans.position - lookAt.position);
        }
    }
}
