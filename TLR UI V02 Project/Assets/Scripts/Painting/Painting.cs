using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Painting : MonoBehaviour
{
    public CustomCursor cursor;
    private Camera cam;
    public Slider radiusSlider;

    public int radius;

    public Color redColor;
    public Color blueColor;
    public Color greenColor;
    public Color currColor;

    public Texture2D tex;
    public Renderer rend;

    void Awake()
    {
        foreach (GameObject go in PlayerSpawning.instance.players)
        {
            if (go != null)
            {
                go.GetComponent<PlayerInput>().SwitchCurrentActionMap("Painting");
                go.GetComponent<PlayerInput>().defaultActionMap = "Painting";
                go.GetComponent<PlayerPainting>().enabled = true;
            }
        }

        cam = GetComponent<Camera>();
        currColor = Color.white;

        //ChangeRadiusSize();
        tex = rend.material.mainTexture as Texture2D;
    }

    public void PlayerSelecting(RaycastHit hit)
    {
        

        if(hit.collider != null)
        {
            print(hit.collider.name);
            if (hit.transform.name == "PaintThis")
            {
                print("painting");

                rend = hit.transform.GetComponent<Renderer>();
                MeshCollider meshCollider = hit.collider as MeshCollider;

                if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                    return;

                tex = rend.material.mainTexture as Texture2D;
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;

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
        }
        
    }

    //void Update()
    //{
    //    if (!Input.GetMouseButton(0))
    //        return;

    //    RaycastHit hit;
    //    if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit)) ;
    //        return;

    //    rend = hit.transform.GetComponent<Renderer>();
    //    MeshCollider meshCollider = hit.collider as MeshCollider;

    //    if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
    //        return;

    //    tex = rend.material.mainTexture as Texture2D;
    //    Vector2 pixelUV = hit.textureCoord;
    //    pixelUV.x *= tex.width;
    //    pixelUV.y *= tex.height;

    //    int x = (int)pixelUV.x;
    //    int y = (int)pixelUV.y;

    //    for (int u = x - radius; u < x + radius + 1; u++)
    //    {
    //        for (int v = y - radius; v < y + radius + 1; v++)
    //        {
    //            if ((x - u) * (x - u) + (y - v) * (y - v) < (radius * radius))
    //                tex.SetPixel(u, v, currColor);
    //        }
    //    }

    //    tex.Apply();
    //}

    //public void ChangeColor(string color)
    //{
    //    if(color == "Red")
    //    {
    //        currColor = redColor;
    //    }
    //    else if(color == "Blue")
    //    {
    //        currColor = blueColor;
    //    }
    //    else if(color == "Green")
    //    {
    //        currColor = greenColor;
    //    }
    //    else
    //    {

    //    }
    //}

    //public void ChangeRadiusSize()
    //{
    //    radius = (int)radiusSlider.value;
    //}

    public void SetTextureColor()
    {
        Color[] checkColors = tex.GetPixels();

        for (int i = 0; i < checkColors.Length; i++)
        {
            //print(checkColors[i].linear);
            if (checkColors[i] == Color.white)
            {
                checkColors[i] = Color.magenta;
                print("WHITE ASS");
                //return;
            }
            
        }
        print("aply");
    }
    
    public void ClearTexture()
    {
        Color[] resetColors = tex.GetPixels();

        for (int i = 0; i < resetColors.Length; i++)
        {
            resetColors[i] = Color.white;
        }

        tex.SetPixels(resetColors);
        tex.Apply();
    }
}
