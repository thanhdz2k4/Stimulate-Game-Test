

namespace Chef.TThanh
{
    public class IdleState : ChefActionState
    {
        public IdleState(ChefController chef, ActionConfig actionConfig) : base(chef, actionConfig) { }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_chef.IsMoving)
            {
                _chef.TryStartAction(ChefAction.Walking);
            }
        }

    }

}

