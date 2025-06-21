using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerParameters>
{
    #region Main
    public override void Initialize(PlayerParameters data)
    {
        this.data = data;
        IsInitialize = data != null;
    }
    public override void StartState()
    {

        if (!IsInitialize)
        {
            return;
        }

        // Enable idle animation
        data.Animator.SetBool(AnimationParameter.Idle.ToString(), true);
        data.Animator.SetBool(AnimationParameter.Walk.ToString(), false);
        data.Animator.SetFloat("Speed", 0f);
    }

    public override void UpdateState()
    {
        if (!IsInitialize) return;


    }

    public override void EndState()
    {
        if (!IsInitialize) return;

        // Disable idle animation
        data.Animator.SetBool(AnimationParameter.Idle.ToString(), false);
    }
    #endregion
}
