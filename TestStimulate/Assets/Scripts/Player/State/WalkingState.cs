using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chef.TThanh
{

    public class WalkingState : ChefActionState
    {
        private Quaternion initialRotation; // Hướng ban đầu
        private Quaternion targetRotation;
        bool isRunning = false; // Biến để kiểm tra trạng thái chạy nhanh

        public WalkingState(ChefController chef, ActionConfig config) : base(chef, config) { }

        protected override void OnActionStart()
        {
            base.OnActionStart();
            // Lưu hướng ban đầu khi bắt đầu walking
            initialRotation = _chef.transform.rotation;
            targetRotation = initialRotation;
        }

        protected override void OnActionUpdate()
        {
            base.OnActionUpdate();
             CheckRunning();
        }


        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();



            // Handle movement
            if (_chef.IsMoving)
            {
                Rotating(_chef.MoveInput.x, _chef.MoveInput.y);
            }
            else
            {
                // Auto-transition to idle if not moving
                _chef.TryStartAction(ChefAction.Idle);
            }
        }

        // Rotate the player to match correct orientation, according to camera and key pressed.
        Vector3 Rotating(float horizontal, float vertical)
        {
            // Get camera forward direction, without vertical component.
            Vector3 forward = _chef.Camera.TransformDirection(Vector3.forward);

            // Player is moving on ground, Y component of camera facing is not relevant.
            forward.y = 0.0f;
            forward = forward.normalized;

            // Calculate target direction based on camera forward and direction key.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 targetDirection = forward * vertical + right * horizontal;

            // Lerp current direction to calculated target direction.
            if (_chef.IsMoving && targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion newRotation = Quaternion.Slerp(_chef.Rigidbody.rotation, targetRotation, _chef.TurnSmoothing);
                _chef.Rigidbody.MoveRotation(newRotation);
                _chef.SetLastDirection(targetDirection);
            }
            // If idle, Ignore current camera facing and consider last moving direction.
            if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
            {
                _chef.Repositioning();
            }

            return targetDirection;
        }

        private void CheckRunning()
        {
            if (Input.GetKey(KeyCode.LeftShift) && !isRunning)
            {
                _chef.Animator.CrossFade(_actionConfig.actionAnimation[1].ToString(), _actionConfig.speedTransition);
                _chef.CamScript.SetFOV(_chef.SprintFOV);
                isRunning = true; // Đặt trạng thái chạy nhanh
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) && isRunning)
            {
                _chef.Animator.CrossFade(_actionConfig.actionAnimation[0].ToString(), _actionConfig.speedTransition);
                _chef.CamScript.ResetFOV(); // Trả về FOV ban đầu
                isRunning = false; // Đặt trạng thái không chạy nhanh
            }
        }

    }


}
