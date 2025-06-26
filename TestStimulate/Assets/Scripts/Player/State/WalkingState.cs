using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chef.TThanh
{

    public class WalkingState : ChefActionState
    {
        public WalkingState(ChefController chef, ActionConfig config) : base(chef, config) { }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // Handle movement
            if (_chef.IsMoving)
            {
                Vector3 moveDirection = new Vector3(_chef.MoveInput.x, 0, _chef.MoveInput.y).normalized;

                if (moveDirection.magnitude > 0.1f)
                {
                    // Rotate towards movement
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    _chef.transform.rotation = Quaternion.RotateTowards(
                        _chef.transform.rotation, targetRotation, _chef.RotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                // Auto-transition to idle if not moving
                _chef.TryStartAction(ChefAction.Idle);
            }
        }
    }
}
