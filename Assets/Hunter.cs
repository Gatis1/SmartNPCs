using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public static float fastSpd = 0.8f;
    public float slowSpd = fastSpd * 0.66f;
    public float a_DiveTime = 0.005f;
    public Prey prey;
    Vector3 targetPos;
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

    IEnumerator UpdateTargetPos()
    {
        while (true)
        {
            targetPos = prey.transform.position;
            yield return new WaitForSeconds(0.1f); // update every 0.1 seconds
        }
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(UpdateTargetPos());
        float distance = Vector3.Distance(transform.position, targetPos);
        if(distance <= 2.0f)
        {
            a_state = Action.Diving;
        }
        switch(a_state)
        {
            case Action.MoveSlow:
            if(distance < 3.0f) 
            {
                speed = slowSpd;
                Vector3 direction = (transform.position - targetPos).normalized;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
            else
            {
                a_state = Action.MoveFast;
            }
            break;

            case Action.MoveFast:
            if(distance > 3.0f)
            {
                speed = fastSpd;
                Vector3 direction = (transform.position - targetPos).normalized;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed *  Time.deltaTime);
            }
            else
            {
                a_state = Action.MoveSlow;
            }
            break;

            //case Action.Diving:
            //speed = 0;
            //a_DiveStart = transform.position;
            //a_DiveEnd = a_DiveStart - (transform.right * a_DiveTime);
//
            //transform.position = Vector3.MoveTowards(transform.position, a_DiveEnd, Time.deltaTime);;
            //break;
        }
    }
}
