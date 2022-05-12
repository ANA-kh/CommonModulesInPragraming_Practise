using BehaviorTree.Test;

namespace BehaviorTree.Condition
{
    public class IsNavigationActivityTypeOf : Condition
    {
        private NavigationActivity _activityToCheckFor;

        public IsNavigationActivityTypeOf(NavigationActivity activity) : 
            base($"Is Navigation Activity {activity}?")
        {
            _activityToCheckFor = activity;
        }
        
        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            if (GameManager.Instance == null || GameManager.Instance.CircleMan == null)
            {
                StatusReason = "GameManager and/or CircleMan is null";
                return NodeStatus.Failure;
            }

            StatusReason = $"CircleMan Activity is {_activityToCheckFor}";

            return GameManager.Instance.CircleMan.MyActivity == _activityToCheckFor ? NodeStatus.Success : NodeStatus.Failure; 
        }
    }
}