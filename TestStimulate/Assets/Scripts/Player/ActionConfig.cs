using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionConfig : MonoBehaviour
{
    public string animationTrigger; // Animation trigger name
    public string animationBool; // Animation boolean name
    public float duration; // Duration of the action in seconds
    public bool canMove; // Can the player move while performing this action?
    public bool canInterrupt; // Can this action be interrupted by another action?
    public bool requiresTarget; // Does this action require a target?

    public ActionConfig(string animationTrigger, string animationBool, float duration, bool canMove, bool canInterrupt, bool requiresTarget)
    {
        this.animationTrigger = animationTrigger;
        this.animationBool = animationBool;
        this.duration = duration;
        this.canMove = canMove;
        this.canInterrupt = canInterrupt;
        this.requiresTarget = requiresTarget;
    }

}

namespace Chef.TThanh
{
    public enum ChefAction
    {
        Idle,
        Walking,
        Cooking,
        Cleaning,
        Serving,
        Chopping,
        Washing,
        Carrying,
        Plating,
        Grilling
    }
}
