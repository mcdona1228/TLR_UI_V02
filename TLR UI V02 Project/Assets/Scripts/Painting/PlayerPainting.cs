using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

public class PlayerPainting : MonoBehaviour
{
    public Painting painting;
    public RectTransform cursorTransform;
    public RectTransform canvasTransform;
    public CustomCursor cursor;
    public Texture2D tex;
    public Slider radiusSlider;

    public Mouse virtualMouse;
    private Vector2 virtualMousePos;
    private bool prevMouseState;
    public int cursorSpeed = 500;
    public float padding = 6f;
    public int radius = 10;

    public Color redColor;
    public Color blueColor;
    public Color greenColor;
    public Color currColor = Color.red;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;


    public void OnEnable()
    {
        GetPaintingSupplies();
        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, GetComponent<PlayerInput>().user);

        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
    }
    private void OnDisable()
    {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }
    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
            return;

        Vector2 deltaVal = Gamepad.current.leftStick.ReadValue();
        deltaVal *= cursorSpeed * Time.deltaTime;

        Vector2 currPos = virtualMouse.position.ReadValue();
        virtualMousePos = currPos + deltaVal;

        virtualMousePos.x = Mathf.Clamp(virtualMousePos.x, padding, Screen.width - padding);
        virtualMousePos.y = Mathf.Clamp(virtualMousePos.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, virtualMousePos);
        InputState.Change(virtualMouse.delta, deltaVal);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        

        if (prevMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            
            prevMouseState = aButtonIsPressed;
        }

        if (aButtonIsPressed)
            TryPaint();

        AnchorCursor(virtualMousePos);
    }
    private void TryPaint()
    {
        print("trying to paint");
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = virtualMousePos;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        
        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit " + result.gameObject.name);
            if (result.gameObject.name == "Painting")
            {
                painting = FindObjectOfType<Painting>();
                //RaycastHit hit;
                //print(virtualMousePos);
                //if (!Physics.Raycast(Camera.main.ScreenPointToRay(virtualMousePos), out hit))
                //    return;
                RectTransform rectTransform = result.gameObject.GetComponent<RectTransform>();
                Rect r = rectTransform.rect;
                Vector2 localPoint;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, virtualMousePos, null, out localPoint))
                    return;

                tex = result.gameObject.GetComponent<Image>().sprite.texture;
                painting.tex = tex;
                float px = Mathf.Clamp(0, (((localPoint.x - r.x) * tex.width) / r.width), tex.width);
                float py = Mathf.Clamp(0, (((localPoint.y - r.y) * tex.height) / r.height), tex.height);
                //print(px + "," + py);
                


                
                //Vector2 pixelUV = hit.textureCoord;
                Vector2 pixelUV = new Vector2(px, py);
                //print(pixelUV);
                //pixelUV.x *= tex.width;
                //pixelUV.y *= tex.height;

                int x = (int)pixelUV.x;
                int y = (int)pixelUV.y;

                for (int u = x - radius; u < x + radius + 1; u++)
                {
                    for (int v = y - radius; v < y + radius + 1; v++)
                    {
                        if ((x - u) * (x - u) + (y - v) * (y - v) < (radius * radius))
                            tex.SetPixel(u, v, currColor);
                    }
                }

                tex.Apply();
            }

            else if(result.gameObject.name == "RedButton")
            {
                ChangeColor("Red");
            }
            else if(result.gameObject.name == "GreenButton")
            {
                ChangeColor("Green");
            }
            else if(result.gameObject.name == "BlueButton")
            {
                ChangeColor("Blue");
            }

            else if(result.gameObject.name == "Handle")
            {
                ChangeRadiusSize();
            }
            else if(result.gameObject.name == "ApplyTexture")
            {
                ScenesManager.instance.StartGameScene();
            }
            else if(result.gameObject.name == "ClearTexture")
            {
                FindObjectOfType<Painting>().ClearTexture();
            }
        }



    }
    private void AnchorCursor(Vector2 pos)
    {
        Vector2 anchorPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pos, null, out anchorPos);

        cursorTransform.anchoredPosition = anchorPos;
    }
    public void GetPaintingSupplies()
    {
        painting = FindObjectOfType<Painting>();
        
        cursor = GameObject.FindGameObjectWithTag("Cursors").transform.GetChild(System.Array.IndexOf(PlayerSpawning.instance.players, gameObject)).GetComponent<CustomCursor>();
        cursor.gameObject.SetActive(true);
        radiusSlider = FindObjectOfType<Slider>();
        cursorTransform = cursor.GetComponent<RectTransform>();
        canvasTransform = FindObjectOfType<Canvas>().transform as RectTransform;
        m_Raycaster = canvasTransform.GetComponent<GraphicRaycaster>();
        m_EventSystem = EventSystem.current;
    }

    public void ChangeColor(string color)
    {
        if (color == "Red")
        {
            currColor = redColor;
        }
        else if (color == "Blue")
        {
            currColor = blueColor;
        }
        else if (color == "Green")
        {
            currColor = greenColor;
        }
        else
        {

        }
    }

    public void ChangeRadiusSize()
    {
        radius = (int)radiusSlider.value - 10;
        cursor.paintSprite.rectTransform.sizeDelta = new Vector2((int)radiusSlider.value + 10, (int)radiusSlider.value + 10);
    }

    public void OnSelect(InputAction.CallbackContext ctx)
    {
        //RaycastHit hit;
        //Vector2 movePos;
        //print(Input.mousePosition);
        //print(cursor.paintSprite.rectTransform.anchoredPosition);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(FindObjectOfType<Canvas>().transform as RectTransform, cursor.paintSprite.rectTransform.anchoredPosition, null, out movePos);
        //Vector3 cursorPos = FindObjectOfType<Canvas>().transform.TransformPoint(movePos);
        //print(cursorPos);
        //Debug.DrawRay(Camera.main.transform.position, new Vector3(cursorPos.x, cursorPos.y, 100f), Color.red, 100f);
        //if (!Physics.Raycast(Camera.main.ScreenPointToRay(cursorPos), out hit))
        //    return;
        //painting.PlayerSelecting(hit);
    }
    public void OnMoveCursor(InputAction.CallbackContext ctx) 
    {
        //cursor.MoveCursor(ctx.ReadValue<Vector2>());
    }
}
