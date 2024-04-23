using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    public Hunter _hunter;
    public SpriteRenderer fish;
    public Sprite Bones;
    public state p_state;
    private Vector3 _moveStartPos;
    private Vector3 _moveEndPos;
    private Vector3 directionToHunter;
    [SerializeField]private float mvSpd = 6.0f;
    [SerializeField]private int moveCount = 50;
    public Vector3 direction;
    public enum state
    {
        Idle,
        Calculate,
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
    }

    public bool caught()
    {
        return (p_state == state.Caught);
    }

    public bool escape()
    {
        return(p_state == state.Escape);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, _hunter.transform.position);
        switch(p_state)
        {
            case state.Idle:
            if(distance < 3.0f){p_state = state.Calculate;}
            break;

            case state.Calculate:
            direction = new Vector3(Random.Range(-3f,3f), Random.Range(-3f,3f), 0);
            _moveStartPos = transform.position;
            _moveEndPos = _moveStartPos + (direction * mvSpd * 0.06f);
            _moveEndPos = Camera.main.WorldToScreenPoint(_moveEndPos);
            directionToHunter = (_hunter.transform.position - transform.position).normalized;
            if(Vector3.Dot(direction, directionToHunter) < 0 && (_moveEndPos.x > 0) && (_moveEndPos.x < Screen.width) && (_moveEndPos.y > 0) && (_moveEndPos.y < Screen.height))
            {
                _moveEndPos = Camera.main.ScreenToWorldPoint(_moveEndPos);
                if(Vector3.Distance(_moveEndPos, _hunter.transform.position) > 2.0f){p_state = state.Run;}
                break;
            }

            if(distance > 3.0f){p_state = state.Idle;}
            break;

            case state.Run:
            if(moveCount > 0)
            {
                transform.position = Vector3.MoveTowards(_moveStartPos, _moveEndPos, mvSpd);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                if (transform.position == _moveEndPos){ p_state = state.Idle; }
                --moveCount;
            }
            else
            {
                p_state = state.Escape;
                gameObject.SetActive(false);
            }
            break;

            case state.Caught:
            break;

            case state.Escape:
            break;
        }
    }
}
