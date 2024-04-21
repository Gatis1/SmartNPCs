using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text Count;
    [SerializeField] Target target;
    private bool TimeOn = true;

    public float CurTime { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        CurTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        while(TimeOn)
        {
            CurTime += 1 * Time.deltaTime;
            Count.text = CurTime.ToString();

            if(target.caught() == true){ TimeOn = false; }    
        }
    }
}