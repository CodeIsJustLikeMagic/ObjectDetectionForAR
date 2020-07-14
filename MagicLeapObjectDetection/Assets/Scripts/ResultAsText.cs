using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultAsText : MonoBehaviour
{
    public static ResultAsText instance;
    private Text output;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        output = GameObject.FindObjectOfType<Text>();
    }

    public void Show(System.String json)
    {
        Debug.Log("show");
        output.text = json;
    }
}
