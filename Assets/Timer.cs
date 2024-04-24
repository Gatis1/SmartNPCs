using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //A timer script used to count the time until the prey escapes or the hunter catches it.
    [SerializeField] Text Count;
    //Needs the Prey prefab in the inspector to call its caught and escape and functions for testing.
    [SerializeField] Prey target;
    public bool TimeOn;
    public float CurTime { get; set; }
    void Start()
    {
        //boolean value to allow the timer to keep counting or stop.
        TimeOn = true;
        CurTime = 0.0f;
    }

    void Update()
    {
        //Time keeps counting until either if conditions are met.
        if(TimeOn == true)
        {
            CurTime += Time.deltaTime;
            Count.text = CurTime.ToString();

            if(target.caught() == true){ TimeOn = false; }
            else if(target.escape() == true){ TimeOn = false;}  
        }
    }
}