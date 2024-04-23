using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField]private Prey _target;
    [SerializeField]private float mvSpd;
    [SerializeField]private float recoverTime = 2.0f;
    private Vector3 _diveStart;
    private Vector3 _diveEnd;
    public state h_state;
    public enum state
    {
        moveSlow,
        moveFast,
        Dive,
        Recover,
        Stop
    }

    private void Start() 
    {
        h_state = state.moveSlow;
        _target = GameObject.FindObjectOfType(typeof(Prey)) as Prey;
    }

    private void Update() 
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        if(distance > 1.5 && distance < 2.0 && h_state != state.Dive){h_state = state.Dive;}
        if(_target.caught() == true){ h_state = state.Stop; }
        switch(h_state)
        {
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
            Dive();
            break;

            case state.Recover:
            StartCoroutine(Recovery());
            break;

            case state.Stop:
            transform.position = Vector2.zero;
            break;
        }
    }

    private void Chase(float mvSpd)
    {
        Vector2 direction = (_target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, mvSpd * Time.deltaTime);
    }

    private void Dive()
    {
        _diveStart = transform.position;
        Vector3 direction = (_target.transform.position - _diveStart).normalized;
        _diveEnd = _diveStart + direction;
        transform.position = Vector3.MoveTowards(transform.position, _diveEnd, 0.3f);
        if(transform.position == _diveEnd){h_state = state.Recover;}
    }

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
