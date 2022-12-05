using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

public class PlayerPainting : MonoBehaviour
{
    public Painting painting;
    public CustomCursor cursor;
    public GameObject capsule;
    public RectTransform cursorTransform;
    public RectTransform canvasTransform;

    private Mouse virtualMouse;
    private bool prevMouseState;
    public int cursorSpeed = 500;
    public float padding = 6f;


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
        Vector2 newPos = currPos + deltaVal;

        newPos.x = Mathf.Clamp(newPos.x, padding, Screen.width - padding);
        newPos.y = Mathf.Clamp(newPos.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, deltaVal);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if (prevMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            prevMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPos);
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
        cursorTransform = cursor.GetComponent<RectTransform>();
        canvasTransform = FindObjectOfType<Canvas>().transform as RectTransform;
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
