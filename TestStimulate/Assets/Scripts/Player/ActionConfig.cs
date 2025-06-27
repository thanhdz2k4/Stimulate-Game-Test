using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ActionConfig 
{

    public float duration; // Duration of the action in seconds
    public bool canMove; // Can the player move while performing this action?
    public bool canInterrupt; // Can this action be interrupted by another action?
    public bool requiresTarget; // Does this action require a target?
    public ChefActionAnimation[] actionAnimation;
    public float speedTransition; // Speed of transition between animations
    public float speed; // Speed of the character during this action

}

namespace Chef.TThanh
{
    public enum ChefAction
    {
        Idle,
        Walking,
        Cleaning,
    }
}
