using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State<PlayerParameters>
{
    bool isMoving;
    #region Main
    public override void Initialize(PlayerParameters data)
    {
        this.data = data;
        IsInitialize = data != null;
    }
    public override void StartState()
    {
        if (!IsInitialize) return;

        // Enable movement animation
        data.Animator.SetBool(AnimationParameter.Walk.ToString(), true);

    }    public override void UpdateState()
    {
        if (!IsInitialize) return;
        // Handle steering based on horizontal input only
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // Rotate player left/right for steering
            float rotationSpeed = 90f; // degrees per second
            data.PlayerObject.transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }

        // Set animation speed (always 1 when moving forward)
        data.Animator.SetFloat("Speed", 1f);
    }

    public override void EndState()
    {
        if (!IsInitialize) return;

        // Disable movement animation
        data.Animator.SetBool(AnimationParameter.Walk.ToString(), false);
        data.Animator.SetFloat("Speed", 0f);
    }
    #endregion
}
