using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Target target;
    // External tunables.
    static public float m_fMaxSpeed = 0.025f;
    static public float m_fSlowSpeed = m_fMaxSpeed * 0.066f;
    public float m_fIncSpeed = 0.00002f;
    public float m_fMagnitudeFast = 0.6f;
    public float m_fMagnitudeSlow = 0.06f;
    public float m_fFastRotateSpeed = 0.2f;
    public float m_fFastRotateMax = 10.0f;
    public float m_fDiveTime = 0.3f;
    public float m_fDiveRecoveryTime = 3.0f;
    public float m_fDiveDistance = 3.0f;
    public float m_fRecoveryTimer = 0.0f; // Added variable



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
        Stop
    }

    // private Color[] stateColors = new Color[(int)eState.kNumStates]
    // {
    //     new Color(0,     0,   0),
    //     new Color(255, 255, 255),
    //     new Color(0,     0, 255),
    //     new Color(0,   255,   0),
    // };

    public bool IsDiving()
    {
        return (m_nState == eState.kDiving);
    }

    void CheckForDive()
    {
        //if (Input.GetMouseButton(0) && (m_nState != eState.kDiving && m_nState != eState.kRecovering))
        //{
        //    // Start the dive operation
        //    m_nState = eState.kDiving;
        //    m_fSpeed = 0.0f;

        //    // Store starting parameters.
        //    m_vDiveStartPos = transform.position;
        //    m_vDiveEndPos = m_vDiveStartPos - (transform.right * m_fDiveDistance);
        //    m_fDiveStartTime = Time.time;
        //}
    }

    void Start()
    {
        // Initialize variables.
        m_fAngle = 0;
        m_fSpeed = 0;
        m_nState = eState.kMoveSlow;
        target = GameObject.FindObjectOfType(typeof(Target)) as Target;

    }

    void UpdateDirectionAndSpeed()
    {
        // // Gets position of mouse in world space
        // Vector3 vScreenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // // Used to normalize mouse offset later on. . .
        // Vector2 vScreenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // // Difference bewteen player's pos and and pos of mouse oon the screen
        // Vector2 vOffset = new Vector2(transform.position.x - vScreenPos.x, transform.position.y - vScreenPos.y);

        // // Used to determine whih direction the player will face: 90 deg face up, 0 deg face/moves to the right
        // m_fTargetAngle = Mathf.Atan2(vOffset.y, vOffset.x) * Mathf.Rad2Deg;

        // // Calculate how far away from the player the mouse is.
        // float fMouseMagnitude = vOffset.magnitude / vScreenSize.magnitude;

        // // Based on distance of player and mouse pointer position, calculate the speed the player is requesting.
        // if (fMouseMagnitude > m_fMagnitudeFast)
        // {
        //     m_fTargetSpeed = m_fMaxSpeed;
        // }
        // else if (fMouseMagnitude > m_fMagnitudeSlow)
        // {
        //     m_fTargetSpeed = m_fSlowSpeed;
        // }
        // else
        // {
        //     m_fTargetSpeed = 0.0f;
        // }

        if (target != null)
        {
            // Calculate direction towards the target
            Vector3 directionToTarget = target.transform.position - transform.position;

            // Calculate angle towards the target
            m_fTargetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Calculate distance to the target
            float distanceToTarget = directionToTarget.magnitude;

            // Based on distance to the target, calculate the speed the player should move
            if (distanceToTarget > 1.0f)
            {
                m_fTargetSpeed = m_fMaxSpeed;
            }
            else if (distanceToTarget < 0.25f)
            {
                m_fTargetSpeed = m_fSlowSpeed;
            }
            //else if (distanceToTarget <= 2.0f)
            //{
            //    // Start the dive operation
            //    m_nState = eState.kDiving;
            //    m_fSpeed = 0.0f;

            //    // Store starting parameters.
            //    m_vDiveStartPos = transform.position;
            //    m_vDiveEndPos = m_vDiveStartPos + (transform.right * m_fDiveDistance);
            //    m_fDiveStartTime = Time.time;
            //}
            else
            {
                m_fTargetSpeed = 0.0f;
            }
        }

    }

    void Update()
    {
        if(target.caught() == true){ m_nState = eState.Stop; }
        // Call updateDirectionAndSpeed per frame
        UpdateDirectionAndSpeed();
        // SetTargetDirectionAndSpeed();
        switch (m_nState)
        {
            // kMoveSlow 
            case eState.kMoveSlow:
                // Restart timer in slow state after each dive attempt
                m_fRecoveryTimer = 0.0f;
                // If player is moving slow, rotate player towards direction of mouse, calculate movement direction, and move player in that direction at a slower speed
                if (m_fTargetSpeed == m_fSlowSpeed)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, m_fTargetAngle);
                    Vector3 moveDirectionSlow = Quaternion.Euler(0, 0, m_fTargetAngle) * Vector3.right;
                    transform.position += moveDirectionSlow * m_fTargetSpeed;
                    CheckForDive();
                }
                // If player speed hasn't reached slow speed yet, move it at incremental speed
                else if (m_fTargetSpeed < m_fSlowSpeed)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, m_fTargetAngle);
                    Vector3 moveDirectionSlow = Quaternion.Euler(0, 0, m_fTargetAngle) * Vector3.right;
                    transform.position += moveDirectionSlow * m_fIncSpeed;
                }

                // If mouse speed is greater than slow speed, change state to moveFast
                else
                {
                    m_nState = eState.kMoveFast;
                }
                break;

            // kMoveFast
            case eState.kMoveFast:

                if (m_fTargetSpeed == m_fMaxSpeed)
                {
                    // Rotate player towards target angle
                    m_fAngle = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, m_fTargetAngle));

                    // If angle is within fast rotate threshold, calculate movement direction, and move player in that direction at max speed
                    if (m_fAngle <= m_fFastRotateMax)
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, m_fTargetAngle);
                        Vector3 moveDirectionFast = Quaternion.Euler(0, 0, m_fTargetAngle) * Vector3.right;
                        transform.position += moveDirectionFast * m_fTargetSpeed;
                    }
                    else
                    {
                        // If outside the threshold, gradually slow down the player, and decrease speed gradually
                        m_fTargetSpeed -= m_fIncSpeed;
                        if (m_fTargetSpeed <= m_fSlowSpeed)
                        {
                            // If speed falls below slow speed threshold, transition back to moving slowly
                            m_nState = eState.kMoveSlow;
                        }
                        else
                        {
                            // Update player's position based on current direction and speed
                            Vector3 moveDirectionFast = Quaternion.Euler(0, 0, m_fTargetAngle) * Vector3.right;
                            transform.position += moveDirectionFast * m_fTargetSpeed;
                        }
                    }
                }
                // Return to moveSlow state once player is not moving fast
                else
                {
                    m_nState = eState.kMoveSlow;
                }
                break;

            // kDiving
            case eState.kDiving:
                // Calculate the progress of the dive (0 to 1) based on time elapsed
                float diveProgress = Mathf.Clamp01((Time.time - m_fDiveStartTime) / m_fDiveTime);

                // Interpolate between the starting and ending positions
                transform.position = Vector3.Lerp(m_vDiveStartPos, m_vDiveEndPos, diveProgress);

                // Check if the dive is complete, chnage state to recovering state
                if (transform.position == m_vDiveEndPos)
                {
                    m_nState = eState.kRecovering;
                }
                break;

            // kRecovering
            case eState.kRecovering:
                // Increment revcovery timer, once recovery has lasted for over the DiveRecoveryTime, return to move slow state
                m_fRecoveryTimer += Time.deltaTime;
                if (m_fRecoveryTimer >= m_fDiveRecoveryTime)
                {
                    m_nState = eState.kMoveSlow;
                }
                break;

            case eState.Stop:
            transform.position = Vector3.zero;
            break;
        }
        // Used to change color of player that corresponds to each state
        // GetComponent<Renderer>().material.color = stateColors[(int)m_nState];
    }
}


