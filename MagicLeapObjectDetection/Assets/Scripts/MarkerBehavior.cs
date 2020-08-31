using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerBehavior : MonoBehaviour
{
    public TextMesh textHolder;
    public void SetText(string str)
    {
        textHolder.text = str;
    }
    public string GetText()
    {
        return textHolder.text;
    }

}
