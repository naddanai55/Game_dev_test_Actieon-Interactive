using UnityEngine;
using System.Collections;
using System.Collections.Generic;       


public class Main : MonoBehaviour
{
    public static Main Instance;
    public Web_Req webReq;
    void Awake()
    {
        Instance = this;
        webReq = GetComponent<Web_Req>();
    }
}
