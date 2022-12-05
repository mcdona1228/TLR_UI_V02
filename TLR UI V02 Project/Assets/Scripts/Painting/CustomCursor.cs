using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class CustomCursor : MonoBehaviour
{
    public Canvas parentCanvas;
    public Image paintSprite;
    public Slider paintingSlider;

    private Vector2 movementInput;
    

    


    private void Start()
    {
        ChangeCursorSize();   
    }

    //private void Update()
    //{
    //    paintSprite.rectTransform.Translate(movementInput * cursorSpeed * Time.deltaTime);
    //}

    //private void Update()
    //{
    //    MoveCursor();
    //}

    public void MoveCursor(Vector2 movementInput)
    {
        this.movementInput = movementInput;
        
        //Vector2 movePos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, movementInput, parentCanvas.worldCamera, out movePos);

        //Vector3 cursorPos = parentCanvas.transform.TransformPoint(movePos);
        //paintSprite.transform.position = cursorPos;

        //transform.position = cursorPos;
    }

    public void SetSpriteVisible(bool isEnabled)
    {
        if(paintSprite.gameObject != null)
            paintSprite.gameObject.SetActive(isEnabled);
    }

    public void ChangeCursorSize()
    {
        paintSprite.rectTransform.sizeDelta = new Vector2((int)paintingSlider.value + 10, (int)paintingSlider.value + 10);
    }
}
