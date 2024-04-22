using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text Count;
    [SerializeField] Target target;
    public bool TimeOn;

    public float CurTime { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        TimeOn = true;
        CurTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeOn == true)
        {
            CurTime += Time.deltaTime;
            Count.text = CurTime.ToString();

            if(target.caught() == true){ TimeOn = false; }
            else if(target.escape() == true){ TimeOn = false;}  
        }
    }
}