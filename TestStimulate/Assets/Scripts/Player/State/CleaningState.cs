using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chef.TThanh
{
    public class CleaningState : ChefActionState
    {
        public CleaningState(ChefController chef, ActionConfig config) : base(chef, config) { }

        protected override void UpdateAction()
        {
            // Cleaning animation speed based on progress
            if (_chef.Animator != null)
            {
                _chef.Animator.SetFloat("CleanSpeed", 1f + GetProgress());
            }
        }

        protected override void OnActionEnd()
        {
            Debug.Log("Cleaning finished! Sparkles everywhere!");
            _chef.TryStartAction(ChefAction.Idle);
        }

    }

}
