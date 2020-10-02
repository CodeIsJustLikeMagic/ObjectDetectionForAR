using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationUI : MonoBehaviour
{
    public static InformationUI instance;
    public Text output;
    public Text markers;
    public Text iteration;
    public GameObject imageDispalyObject;
    private void Awake()
    {
        instance = this;
    }
    public void ShowImage(byte[] imageData)
    {
        Texture2D texture = new Texture2D(8, 8);
        bool status = texture.LoadImage(imageData);

        if (status && (texture.width != 8 && texture.height != 8))
        {
            imageDispalyObject.SetActive(true);
            Renderer renderer = imageDispalyObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
            }

        }
    }
    public void Show(System.String json)
    {
        output.text = json;
    }
    public void Add(System.String json)
    {
        output.text = output.text + "\n" + json;
    }
    public void AddMarkers(System.String json)
    {
        markers.text = markers.text + "\n" + json;
    }
    public void ShowIteration(string str)
    {
        iteration.text = "Custom Vision Iteration : " + str;
    }
    public void ShowMarkers(string str)
    {
        markers.text = "Markers :\n" + str;
    }
}
