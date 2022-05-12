using BehaviorTree.Test;

namespace BehaviorTree.Action
{
    public class SetNavigationActivityTo : NodeBase
    {
        private NavigationActivity _newActivity;

        public SetNavigationActivityTo(NavigationActivity newActivity)
        {
            _newActivity = newActivity;
            Name = $"Set NavigationActivity to {_newActivity}";
        }

        protected override void OnReset()
        {
        }

        protected override NodeStatus OnRun()
        {
            if (GameManager.Instance == null || GameManager.Instance.CircleMan == null)
            {
                StatusReason = "GameManager and/or CircleMan is null";
                return NodeStatus.Failure;
            }

            GameManager.Instance.CircleMan.MyActivity = _newActivity;

            return NodeStatus.Success;
        }
    }
}