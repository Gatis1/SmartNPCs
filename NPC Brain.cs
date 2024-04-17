using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain : MonoBehaviour
{
    public enum NPCState { Supervised, Unsupervised, Check, Relay }

    public NPCState currentState;
    public bool isSupervised = false;

    // Other variables you may need

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Supervised:
                if (isSupervised)
                {
                    SearchFortress();
                }
                else
                {
                    // Handle behavior for being in Supervised state but not supervised
                }
                break;
            case NPCState.Unsupervised:
                Wander();
                break;
            case NPCState.Check:
                CheckFortress();
                break;
            case NPCState.Relay:
                RelayInstructions();
                break;
        }
    }

    void SearchFortress()
    {
        // Implement A* search to find the nearest fortress
        // If the NPC reaches a fortress, switch to Check state
    }

    void CheckFortress()
    {
        // Check if the NPC can unlock the fortress
        // If it can unlock it, switch to Relay state
        // Otherwise, switch back to Supervised or Unsupervised state based on isSupervised variable
    }

    void RelayInstructions()
    {
        // Tell other NPCs where they should go based on the unlocked fortress
    }

    void Wander()
    {
        // Implement random wandering behavior until finding a fortress
    }
}
