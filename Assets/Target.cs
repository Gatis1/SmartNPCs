using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Hunter m_player;
    public enum eState : int
    {
        kIdle,
        kHopStart,
        kHop,
        kCaught,
        kNumStates
    }

    private Color[] stateColors = new Color[(int)eState.kNumStates]
   {
        new Color(255, 0,   0),
        new Color(0,   255, 0),
        new Color(0,   0,   255),
        new Color(255, 255, 255)
   };

    // External tunables.
    public float m_fHopTime = 0.2f;
    public float m_fHopSpeed = 6.0f;
    public float m_fScaredDistance = 3.0f;
    public int m_nMaxMoveAttempts = 50;

    // Internal variables.
    public eState m_nState;
    public float m_fHopStart;
    public Vector3 m_vHopStartPos;
    public Vector3 m_vHopEndPos;

    public Vector3 direction;

    void Start()
    {
        // Setup the initial state and get the player GO.
        m_nState = eState.kIdle;
        m_player = GameObject.FindObjectOfType(typeof(Hunter)) as Hunter;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, m_player.transform.position);

        switch (m_nState)
        {
            case eState.kIdle:
                //Check if the player reaches a certian distance between the target.
                if (distance < 3.0f) { m_nState = eState.kHopStart; }
                break;

            //A case that calculates the random position for the target to hop to.
            case eState.kHopStart:
                 int moves = m_nMaxMoveAttempts;
                //while loop to check if it takes a certian amount of attempts to find a random direction to hop to.
                while (moves > 0)
                {
                    //generate the random direction/position based on a random range from -2.5 to 2.5 for the x and y positions.
                    direction = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0);

                    //start hop position is the current position of the target.
                    m_vHopStartPos = transform.position;

                    //end position is calculated based on start pos, random direction, hope speed, and hop time. 
                    m_vHopEndPos = m_vHopStartPos + (direction * m_fHopSpeed * m_fHopTime);

                    //sets end pos to world point based on screen point. Used for the boundries of the screen.
                    m_vHopEndPos = Camera.main.WorldToScreenPoint(m_vHopEndPos);
                    
                    // Calculate the direction towards the player
                    Vector3 directionToPlayer = (m_player.transform.position - transform.position).normalized;

                    //check to make sure the end position is within the bounds of the screen and not towards the player's direction.
                    if (Vector3.Dot(direction, directionToPlayer) < 0 && (m_vHopEndPos.x > 0) && (m_vHopEndPos.x < Screen.width) && (m_vHopEndPos.y > 0) && (m_vHopEndPos.y < Screen.height))
                    {
                        //set end pos back to world point and change to hop stat.
                        m_vHopEndPos = Camera.main.ScreenToWorldPoint(m_vHopEndPos);
                        if(Vector3.Distance(m_vHopEndPos, m_player.transform.position) > m_fScaredDistance) { m_nState = eState.kHop; }
                        break;
                    }
                    --moves;
                }

                if (moves <= 0 && distance < 3.0f)
                {
                    m_nState = eState.kIdle;
                }
                break;

            case eState.kHop:
                // Move the character towards the end position in a smooth motion.
                transform.position = Vector3.MoveTowards(transform.position, m_vHopEndPos, m_fHopSpeed * Time.deltaTime);

                //get the angle for the target to turn to when moving.
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                //ensures the target is facing towards the direction it is going .
                angle -= 90;

                //rotates the target towards the direction it moves to.
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                //if the target reaches its end pos switch back to idle and wait for player.
                if (transform.position == m_vHopEndPos){ m_nState = eState.kIdle; }
                break;

            //Did not know what I needed to do for this state.
            case eState.kCaught:
                break;

            //Did not know what I needed to do for this state.
            case eState.kNumStates:
                break;
        }
        GetComponent<Renderer>().material.color = stateColors[(int)m_nState];
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Check if this is the player (in this situation it should be!)
        if (collision.gameObject == GameObject.Find("Player"))
        {
            // If the player is diving, it's a catch!
            if (m_player.Diving())
            {
                m_nState = eState.kCaught;
                transform.parent = m_player.transform;
                transform.localPosition = new Vector3(0.0f, -0.5f, 0.0f);
            }
        }
    }
}