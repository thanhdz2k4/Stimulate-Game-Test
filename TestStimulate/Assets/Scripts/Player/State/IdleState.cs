
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

