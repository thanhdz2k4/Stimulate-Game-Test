using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chef.TThanh
{

    public class WalkingState : ChefActionState
    {
        private Quaternion initialRotation; // Hướng ban đầu
        private Quaternion targetRotation;
        
        public WalkingState(ChefController chef, ActionConfig config) : base(chef, config) { }

        protected override void OnActionStart()
        {
            base.OnActionStart();
            // Lưu hướng ban đầu khi bắt đầu walking
            initialRotation = _chef.transform.rotation;
            targetRotation = initialRotation;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // Handle movement
            if (_chef.IsMoving)
            {
                Vector3 moveInput = _chef.MoveInput;

                if (moveInput.magnitude > 0.1f)
                {
                    // Tính góc xoay dựa trên input (tính bằng độ)
                    float rotationAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
                    
                    // Tạo quaternion xoay từ hướng ban đầu
                    Quaternion inputRotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);
                    
                    // Cộng vào hướng ban đầu
                    targetRotation = initialRotation * inputRotation;
                    
                    // Smooth rotation
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
