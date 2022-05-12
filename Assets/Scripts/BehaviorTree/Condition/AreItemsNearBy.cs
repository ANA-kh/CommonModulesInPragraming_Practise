using BehaviorTree.Test;
using UnityEngine;

namespace BehaviorTree.Condition
{
    public class AreItemsNearBy : Condition
    {
        private float _distanceToCheck;

        public AreItemsNearBy(float maxDistance) : base($"Are Items within {maxDistance}f?") 
        { 
            _distanceToCheck = maxDistance; 
        }
            
        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            if (GameManager.Instance == null || GameManager.Instance.CircleMan == null)
            {
                StatusReason = "GameManager and/or CircleMan is null";
                return NodeStatus.Failure;
            }

            GameObject item = GameManager.Instance.GetClosestItem();

            if (item == null)
            {
                StatusReason = "No items near by";
                return NodeStatus.Failure;

            }
            else if (Vector3.Distance(item.transform.position, 
                GameManager.Instance.CircleMan.transform.position) > _distanceToCheck)
            {
                StatusReason = $"No items within range of {_distanceToCheck} meters";
                return NodeStatus.Failure;
            }

            return NodeStatus.Success;
        }
    }
}