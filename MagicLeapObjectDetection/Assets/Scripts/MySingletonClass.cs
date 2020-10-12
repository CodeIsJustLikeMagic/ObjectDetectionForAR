using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySingletonClass : MonoBehaviour
{
    public static MySingletonClass instance = null;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }


    public void DoSomething()
    {
    }
}
