using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // External tunables.
    static public float m_fMaxSpeed = 0.015f;
    public float m_fSlowSpeed = m_fMaxSpeed * 0.66f;
    public float m_fIncSpeed = 0.00002f;
    public float m_fMagnitudeFast = 0.6f;
    public float m_fMagnitudeSlow = 0.06f;
    public float m_fFastRotateSpeed = 0.2f;
    public float m_fFastRotateMax = 10.0f;
    public float m_fDiveTime = 0.3f;
    public float m_fDiveRecoveryTime = 0.5f;
    public float m_fDiveDistance = 3.0f;

    // Internal variables.
    public Vector3 m_vDiveStartPos;
    public Vector3 m_vDiveEndPos;
    public float m_fAngle;
    public float m_fSpeed;
    public float m_fTargetSpeed;
    public float m_fTargetAngle;
    public eState m_nState;
    public float m_fDiveStartTime;

    public enum eState : int
    {
        kMoveSlow,
        kMoveFast,
        kDiving,
        kRecovering,
        kNumStates
    }

    private Color[] stateColors = new Color[(int)eState.kNumStates]
    {
        new Color(0,     0,   0),
        new Color(255, 255, 255),
        new Color(0,     0, 255),
        new Color(0,   255,   0),
    };

    public bool IsDiving()
    {
        return (m_nState == eState.kDiving);
    }

    void CheckForDive()
    {
        if (Input.GetMouseButtonDown(0) && (m_nState != eState.kDiving && m_nState != eState.kRecovering))
        {
            // Start the dive operation
            m_nState = eState.kDiving;
            m_fSpeed = 0.0f;

            // Store starting parameters.
            m_vDiveStartPos = transform.position;
            m_vDiveEndPos = m_vDiveStartPos - (transform.right * m_fDiveDistance);
            m_fDiveStartTime = Time.time;
        }
    }

    void Start()
    {
        // Initialize variables.
        m_fAngle = 0;
        m_fSpeed = 0;
        m_nState = eState.kMoveSlow;
    }

    void UpdateDirectionAndSpeed()
    {
        // Get relative positions between the mouse and player
        Vector3 vScreenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 vScreenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 vOffset = new Vector2(transform.position.x - vScreenPos.x, transform.position.y - vScreenPos.y);

        // Find the target angle being requested.
        m_fTargetAngle = Mathf.Atan2(vOffset.y, vOffset.x) * Mathf.Rad2Deg;

        // Calculate how far away from the player the mouse is.
        float fMouseMagnitude = vOffset.magnitude / vScreenSize.magnitude;

        // Based on distance, calculate the speed the player is requesting.
        if (fMouseMagnitude > m_fMagnitudeFast)
        {
            m_fTargetSpeed = m_fMaxSpeed;
        }
        else if (fMouseMagnitude > m_fMagnitudeSlow)
        {
            m_fTargetSpeed = m_fSlowSpeed;
        }
        else
        {
            m_fTargetSpeed = 0.0f;
        }
    }

    void Update()
    {
        if(!IsDiving()){ UpdateDirectionAndSpeed(); }
        CheckForDive();

        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        switch (m_nState)
        {
            case eState.kMoveSlow:
                //rotates the character based on mouse position.
                transform.rotation = Quaternion.AngleAxis(m_fTargetAngle, Vector3.forward);

                //moves character based on mouse position and speed values.
                transform.position += direction * Mathf.Lerp(m_fTargetSpeed, m_fIncSpeed, Time.deltaTime);

                //moves only the x and y transforms to keep the character on screen.
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);

                //a check to see if the speed of the character reaches a threshold to switch to fast state.
                if(m_fTargetSpeed >= m_fMaxSpeed){ m_nState = eState.kMoveFast; }
                break;

            case eState.kMoveFast:
                //rotates the character taking the current rotation and movement of the character to keep the character from sharply turning in fast state.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(m_fTargetAngle, Vector3.forward), 1f);

                transform.position -= transform.right * Mathf.Lerp(m_fTargetSpeed, m_fIncSpeed, Time.deltaTime) * 12;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);

                //a check to see if the speed of the character reaches a threshold to switch to slow state.
                if (m_fTargetSpeed <= m_fSlowSpeed){ m_nState = eState.kMoveSlow; }
                break;

            case eState.kDiving:
                //Makes the character dive based on its current position, calculated end position and dive time.
                transform.position = Vector3.Lerp(transform.position, m_vDiveEndPos, m_fDiveTime);

                //Switch character to recover state when the character reaches it's dive end position.
                if(transform.position == m_vDiveEndPos){ m_nState = eState.kRecovering; }
                break;

            case eState.kRecovering:
                //Call a coroutine to run the recovery cooldown from a dive.
                StartCoroutine(Recovery());                
                break;

            //Did not know what to do with this state.
            case eState.kNumStates:
                break;
        }
        GetComponent<Renderer>().material.color = stateColors[(int)m_nState];
    }

    //A coroutine to perform the recovery. Other methods I tried had a near intant recovery instead of a cool down.
    IEnumerator Recovery()
    {
        //Set the timer to recovery time.
        float timer = m_fDiveRecoveryTime;

        while (timer > 0)
        {
            //Decrease the timer by delta time.
            timer -= Time.deltaTime;

            //Wait for the next frame.
            yield return null;
        }
        m_nState = eState.kMoveSlow;
    }
}
