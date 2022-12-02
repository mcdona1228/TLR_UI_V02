using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Button buttonToSelect;


    private void Start()
    {
        buttonToSelect.Select();
    }

}
