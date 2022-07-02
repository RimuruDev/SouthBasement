using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRoom : MonoBehaviour
{
    public GameObject CloseWalls;
    public void Close()
    {
        if(CloseWalls != null & CloseWalls.activeSelf == false)
           CloseWalls.gameObject.SetActive(true);
    }
    public void Open()
    {
        if(CloseWalls != null & CloseWalls.activeSelf == true)
            CloseWalls.gameObject.SetActive(false);
    }
}
