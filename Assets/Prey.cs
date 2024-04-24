using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    //External values the Prey needs
    public Hunter _hunter;
    public SpriteRenderer fish;
    public Sprite Bones;

    //Display their current state during testing
    public state p_state;

    //Internal values
    private Vector3 _moveStartPos;
    private Vector3 _moveEndPos;
    private Vector3 directionToHunter;
    [SerializeField]private float mvSpd = 6.0f;
    private int moveCount;
    [SerializeField]private int moveAttempts = 50;
    [SerializeField]private float moveRange = 90.0f;
    public Vector3 direction;

    //The Prey states
    public enum state
    {
        Idle,
        Run,
        Caught,
        Escape
    }

    // Start is called before the first frame update
    void Start()
    {
        p_state = state.Idle;
        _hunter = GameObject.FindObjectOfType(typeof(Hunter)) as Hunter;
        fish = gameObject.GetComponent<SpriteRenderer>();
        moveCount = 0;
    }

    //A bool function to return if the prey is in the cuaght state to other scripts
    public bool caught()
    {
        return (p_state == state.Caught);
    }

    //A bool function to return if the prey is in the escap state to other scripts
    public bool escape()
    {
        return(p_state == state.Escape);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 hunterDirection = (transform.position - _hunter.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _hunter.transform.position);
        switch(p_state)
        {
            //Default state to check if the hunter is close by.
            case state.Idle:
            if(distance < 3.0f){p_state = state.Run;}
            break;

            //State to run to a new position away from the hunter.
            case state.Run:
            // Unit vector pointing from target towards player
                Vector3 awayFromHunterDirection = (transform.position - _hunter.transform.position).normalized;

                Vector3 _runTo = Quaternion.Euler(0, 0, Random.Range(0f, moveRange)) * awayFromHunterDirection;
                float moveTime = Time.time;

                // Calculate amount of time since prey started moving
                float elapsedTime = Time.time - moveTime;

                if (elapsedTime < 2)
                {
                    // Rotate and move prey's direction of movement
                    float angle = Mathf.Atan2(_runTo.y, _runTo.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                    transform.position += _runTo * Time.deltaTime * mvSpd;

                    // Clamps prey's postion so it does not leave the screen
                    Vector3 clampedPosition = transform.position;
                    clampedPosition.x = Mathf.Clamp(clampedPosition.x, Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x);
                    clampedPosition.y = Mathf.Clamp(clampedPosition.y, Camera.main.ScreenToWorldPoint(Vector3.zero).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y);
                    transform.position = clampedPosition;
                }
                else
                {
                    // Return to idle after a run -- this ensures the prey doesn't do one long run
                    p_state = state.Idle;
                    // Reset the movement direction and move time for the next move away from hunter
                    _runTo = Vector3.zero;
                    moveTime = 0f;
                    // Keep track of number of prey movements.
                    if (moveTime == 0f && moveCount <= moveAttempts)
                    {
                        moveCount++;
                        Debug.Log($"move count is {moveCount}");
                    }
                    //If the max move attempts were met the prey is allowed to escape.
                    else
                    {
                        p_state = state.Escape;
                        gameObject.SetActive(false);
                        Debug.Log("The Prey escaped!!!");
                    }
                }
                break;

            //A "caught" state to stop the prey from moving once caught.
            case state.Caught:
            break;

            //A "escape" state to show that the prey escaped the hunter.
            case state.Escape:
            break;
        }
    }

    //A collision function that checks if the hunter collided with thte prey and append them as a child of the hunter if caught and changes its apperance.
    void OnTriggerStay2D(Collider2D collision)
    {
        // Check if this is the hunter (in this situation it should be!)
        if (collision.gameObject == GameObject.Find("Hunter"))
        {
            p_state = state.Caught;
            transform.parent = _hunter.transform;
            transform.localPosition = new Vector3(0.5f, 0.0f, 0.0f);
            fish.sprite = Bones;
        }
    }
}
