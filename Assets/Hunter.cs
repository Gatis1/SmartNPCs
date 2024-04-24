using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    //Modifiable variables
    [SerializeField]private Prey _target;
    [SerializeField]private float recoverTime = 2.0f;

    //Internal Variables
    private float mvSpd;
    private Vector3 _diveStart;
    private Vector3 _diveEnd;
    public state h_state;

    //Hunter states
    public enum state
    {
        moveSlow,
        moveFast,
        Dive,
        Recover,
        Stop
    }

    //Starts the hunter in the default slow state and finds where the prey character is.
    private void Start() 
    {
        h_state = state.moveSlow;
        _target = GameObject.FindObjectOfType(typeof(Prey)) as Prey;
    }

    //The base unity function that allows the hunter move and use other functions
    private void Update() 
    {
        //Calculates the distance from the itself to the prey.
        float distance = Vector3.Distance(transform.position, _target.transform.position);

        //An if check to see if it can dive towards the prey. The dive mechanic was problematic to implement.
        //if(distance > 1.5 && distance < 2.0 && h_state != state.Dive){h_state = state.Dive;}

        //An if check to move the hunter ot the center of the scebe if it catches the prey
        if(_target.caught() == true){ h_state = state.Stop; }

        switch(h_state)
        {
            //Checks if the distance is within a certian threshold and changes to slow movement speed or changes to fast state.
            case state.moveSlow:
            if(distance <= 2.0f){mvSpd = 2.0f;}
            else{h_state = state.moveFast;}
            Chase(mvSpd);
            break;

            case state.moveFast:
            mvSpd = 5.0f;
            Chase(mvSpd);
            break;

            case state.Dive:
            //Dive();
            break;

            //If the hunter dove it will call a function to enter a cooldown
            case state.Recover:
            StartCoroutine(Recovery());
            break;

            //The state the moves the hunter to the center of the scene if successful.
            case state.Stop:
            transform.position = Vector2.zero;
            break;
        }
    }

    //A movement function that moves the hunter towards the prey and rotates it to face the prey's fdirection.
    private void Chase(float mvSpd)
    {
        Vector2 direction = (_target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, mvSpd * Time.deltaTime);
    }

    //The dive function for the hunter proved problematic so it will be excempt for now.
    // /private void Dive()
    // /{
        // _diveStart = transform.position;
        // Vector3 direction = (_target.transform.position - _diveStart).normalized;
        // _diveEnd = _diveStart + direction;
        // transform.position = Vector3.MoveTowards(transform.position, _diveEnd, 0.3f);
        // if(transform.position == _diveEnd){h_state = state.Recover;}
    // /}

    //A Special function used for a cooldown if the hunter dove at the prey.
    IEnumerator Recovery()
    {
        float timer = recoverTime;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        h_state = state.moveSlow;
    }
}
