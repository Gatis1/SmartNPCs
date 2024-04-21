using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public Target prey;
    private Vector3 a_DiveStart;
    private Vector3 a_DiveEnd;
    private float speed;
    private float angle;
    private Action a_state;

    public enum Action : int
    {
        MoveSlow,
        MoveFast,
        Diving,
        Recovering
    }

    public bool Diving()
    {
        return (a_state == Action.Diving);
    }

    private void Start() {
        speed = 0;
        angle = 0;
        a_state = Action.MoveSlow;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, prey.transform.position);
        switch(a_state)
        {
            case Action.MoveSlow:
            if(distance < 3.0f) 
            {
                speed = 0.3f;
                transform.position = Vector3.Lerp(transform.position, prey.transform.position, speed * Time.deltaTime);
            }
            else
            {
                a_state = Action.MoveFast;
            }
            break;

            case Action.MoveFast:
            if(distance > 3.0f)
            {
                speed = 0.8f;
                transform.position = Vector3.Lerp(transform.position, prey.transform.position, speed *  Time.deltaTime);
            }
            else
            {
                a_state = Action.MoveSlow;
            }
            break;
        }
    }
}
