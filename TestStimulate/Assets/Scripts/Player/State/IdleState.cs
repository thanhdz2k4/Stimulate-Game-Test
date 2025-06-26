
using UnityEngine;

namespace Chef.TThanh
{
    public class IdleState : ChefActionState
    {
        public IdleState(ChefController chef, ActionConfig actionConfig) : base(chef, actionConfig) 
        { 

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnActionStart()
        {
            base.OnActionStart();
            
            if (_chef?.Animator != null && !string.IsNullOrEmpty(_actionConfig.animationBool))
            {
                _chef.Animator.SetBool(_actionConfig.animationBool, true);
            }
            else
            {
                Debug.LogWarning($"Cannot set animation - Chef: {_chef}, Animator: {_chef?.Animator}, AnimBool: {_actionConfig?.animationBool}");
            }
        }

        protected override void OnActionUpdate()
        {
            base.OnActionUpdate();

            if (_chef.IsMoving)
            {
                _chef.TryStartAction(ChefAction.Walking);
            }
        }


    }

}

