using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Controls the scenario of the current instance.
    public static GameManager Instance;

    // Flag to check if the puzzle is completed or not.
    public bool puzzleCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
           Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPuzzleCompletion();
    }

    void CheckPuzzleCompletion()
    {
        // The logic for the NPCs to complete the puzzle code here...
    }
}
