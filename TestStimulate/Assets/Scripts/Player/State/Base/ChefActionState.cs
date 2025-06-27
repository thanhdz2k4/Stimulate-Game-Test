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
            // Blend từ animation hiện tại sang animation mới trong 0.2 giây
            _chef.Animator.CrossFade(_actionConfig.actionAnimation[0].ToString(), _actionConfig.speedTransition);
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
            _chef.Animator.SetFloat("Speed", _actionConfig.speed);
            OnActionUpdate();

        }

        public virtual void OnFixedUpdate() { }

        public virtual void OnExit()
        {
            OnActionEnd();
            _isCompleted = false;
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
        protected virtual void OnActionUpdate() { }
        protected virtual void OnActionEnd() { }
        #endregion

    }


}
