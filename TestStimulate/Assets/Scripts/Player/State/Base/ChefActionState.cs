using System.Collections;
using System.Collections.Generic;
using Patterns.StateMachine.TThanh;
using UnityEngine;

namespace Chef.TThanh
{
    public class ChefActionState : IActionState<ChefAction>
    {
        protected ChefController _chef;
        protected ActionConfig _actionConfig;
        protected float _stateTimer;
        protected bool _isCompleted;

        public ChefActionState(ChefController chef, ActionConfig actionConfig)
        {
            _chef = chef;
            _actionConfig = actionConfig;
        }


        #region IActionState Implementation


        public virtual void OnEnter()
        {
            _stateTimer = 0f;
            _isCompleted = false;

            // Set animation parameters
            if (!string.IsNullOrEmpty(_actionConfig.animationTrigger))
            {
                _chef.Animator.SetTrigger(_actionConfig.animationTrigger);
            }
            if (!string.IsNullOrEmpty(_actionConfig.animationBool))
            {
                _chef.Animator.SetBool(_actionConfig.animationBool, true);
            }

            // Additional setup if needed
            OnActionStart();
        }

        public virtual void OnUpdate()
        {
            _stateTimer += Time.deltaTime;
            if (_stateTimer >= _actionConfig.duration)
            {
                _isCompleted = true;
            }
            UpdateAction();

        }

        public virtual void OnFixedUpdate() { }

        public virtual void OnExit()
        {
            // Reset animation parameters
            if (!string.IsNullOrEmpty(_actionConfig.animationBool))
            {
                _chef.Animator.SetBool(_actionConfig.animationBool, false);
            }

            // Additional cleanup if needed
            OnActionEnd();
        }

        #endregion


        #region others

        public bool CanExit()
        {
            return _actionConfig.canInterrupt || IsCompleted();
        }

        public bool IsCompleted()
        {
            return _isCompleted;
        }

        public float GetProgress()
        {
            return _actionConfig.duration > 0 ? Mathf.Clamp01(_stateTimer / _actionConfig.duration) : 1f;
        }

        // Override these in specific actions
        protected virtual void OnActionStart() { }
        protected virtual void UpdateAction() { }
        protected virtual void OnActionEnd() { }
        #endregion

    }


}
