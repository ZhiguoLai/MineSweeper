using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{   
    private int min;
    private int sec;
    private float timeStart = 0f;
    private void Update()
    {
        GetComponent<Text>().text = ((Time.time - timeStart) / 60).ToString("00")+ ":" + ((Time.time - timeStart)% 60).ToString("00"); // calculate the time
    }

    public void Reset()
    {
        timeStart = Time.time;
    }
}
