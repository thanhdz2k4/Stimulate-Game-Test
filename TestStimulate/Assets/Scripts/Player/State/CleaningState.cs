using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chef.TThanh
{
    public class CleaningState : ChefActionState
    {
        public CleaningState(ChefController chef, ActionConfig config) : base(chef, config) { }

        protected override void OnActionStart()
        {
            base.OnActionStart();
        }

        protected override void OnActionUpdate()
        {
            base.OnActionUpdate();
            if (IsCompleted())
            {
                _chef.TryStartAction(ChefAction.Idle);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }



    }

}
