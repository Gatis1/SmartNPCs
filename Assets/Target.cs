using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Player m_player;
    public SpriteRenderer fish;
    public Sprite Bones;
    public enum eState : int
    {
        kIdle,
        kHopStart,
        kHop,
        kCaught,
        kNumStates
    }

    //     private Color[] stateColors = new Color[(int)eState.kNumStates]
    //    {
    //             new Color(255, 0,   0),
    //             new Color(0,   255, 0),
    //             new Color(0,   0,   255),
    //             new Color(255, 255, 255)
    //    };

    // External tunables.
    static public float m_fHopTime = 0.2f;
    static public float m_fHopSpeed = 6.0f;
    static public float m_fScaredDistance = 3.0f;
    static public int m_nMaxMoveAttempts = 50;
    static public int moveCount;

    // Internal variables.
    public eState m_nState;
    public float m_fHopStart;
    public Vector3 m_vHopStartPos;
    public Vector3 m_vHopEndPos;
    public Vector3 m_vHopDirection = new Vector3(0.0f, 0.0f, 0.0f);


    void Start()
    {
        // Setup the initial state and get the player GO.
        m_nState = eState.kIdle;
        m_player = GameObject.FindObjectOfType(typeof(Player)) as Player;
        fish = gameObject.GetComponent<SpriteRenderer>();
        moveCount = 0;
    }

    public bool caught()
    {
        return (m_nState == eState.kCaught);
    }

    void Update()
    {

        switch (m_nState)
        {
            // kIdle - 
            case eState.kIdle:
                // Calculate direction and distance to the player, and store it in a float
                Vector3 directionToTarget = m_player.transform.position - transform.position;
                float distanceToTarget = directionToTarget.magnitude;

                // Change state to kHop when player is close
                if (distanceToTarget < m_fScaredDistance)
                {
                    m_nState = eState.kHop;
                }
                break;

            // kHopStart - after rabbit hops 50 times, reset counter and change rabbit's state back to idle for 6.0f
            case eState.kHopStart:
                moveCount = 0;
                float hopStartTimer = 0.0f;
                hopStartTimer += Time.time;
                if (hopStartTimer >= 6.0f)
                {
                    m_nState = eState.kIdle;
                }
                break;

            // kHop 
            case eState.kHop:
                // Unit vector pointing from target towards player
                Vector3 awayFromPlayerDirection = (transform.position - m_player.transform.position).normalized;

                // Randomly set the direction of hop and set hop start timer 
                if (m_vHopDirection == Vector3.zero)
                {
                    m_vHopDirection = Quaternion.Euler(0, 0, Random.Range(0f, 120.0f)) * awayFromPlayerDirection;
                    m_fHopStart = Time.time;
                }

                // Calculate amount of time since rabbit started hopping
                float elapsedTime = Time.time - m_fHopStart;

                if (elapsedTime < m_fHopTime)
                {
                    // Rotate and move rabbit's direction of hop
                    float angle = Mathf.Atan2(m_vHopDirection.y, m_vHopDirection.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                    transform.position += m_vHopDirection * Time.deltaTime * m_fHopSpeed;

                    // Clamps rabbit's postion so it does not leave the screen
                    Vector3 clampedPosition = transform.position;
                    clampedPosition.x = Mathf.Clamp(clampedPosition.x, Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x);
                    clampedPosition.y = Mathf.Clamp(clampedPosition.y, Camera.main.ScreenToWorldPoint(Vector3.zero).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y);
                    transform.position = clampedPosition;
                }
                else
                {
                    // Return to idle after a hop -- this ensures the rabbit doesn't do one long hop
                    m_nState = eState.kIdle;
                    // Reset the movement direction and hop start time for the next hop away from player
                    m_vHopDirection = Vector3.zero;
                    m_fHopStart = 0f;
                    // Keep track of number of rabbit hops to return it to hopStart state if the rabbit hops 50 times
                    if (m_fHopStart == 0f && moveCount <= m_nMaxMoveAttempts)
                    {
                        moveCount++;
                        Debug.Log($"move count is {moveCount}");
                    }
                    else
                    {
                        Vector3 offScreen = transform.position * 5;
                        transform.position = Vector3.MoveTowards(transform.position, offScreen, Time.deltaTime);
                        gameObject.SetActive(false);
                        Debug.Log("The Prey escaped!!!");
                    }
                }
                break;

            // kCaught
            case eState.kCaught:
                break;


        }
        // The color will be changed based off of the current state becuase after each case, the eState will be set to a given state, 
        // GetComponent<Renderer>().material.color = stateColors[(int)m_nState];

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Check if this is the player (in this situation it should be!)
        if (collision.gameObject == GameObject.Find("Hunter"))
        {
            m_nState = eState.kCaught;
            transform.parent = m_player.transform;
            transform.localPosition = new Vector3(0.5f, 0.0f, 0.0f);
            fish.sprite = Bones;
        }
    }
}