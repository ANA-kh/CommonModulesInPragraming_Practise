using System;
using BehaviorTree.Test;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Action
{
    public class NavigateToDestination : NodeBase
    {
        private Vector3 _targetDestination;

        public NavigateToDestination()
        {
            Name = "Navigate";
        }

        protected override void OnReset()
        {
        }

        protected override NodeStatus OnRun()
        {
            if (GameManager.Instance == null || GameManager.Instance.CircleMan == null)
            {
                StatusReason = "GameManager or CircleMan is null";
                return NodeStatus.Failure;
            }

            if (EvaluationCount == 0)
            {
                GameObject destinationGO = GameManager.Instance.CircleMan.MyActivity ==
                                           NavigationActivity.PickupItem
                    ? GameManager.Instance.GetClosestItem()
                    : GameManager.Instance.GetNextWayPoint();

                if (destinationGO == null)
                {
                    StatusReason = $"Unable to find game object for {GameManager.Instance.CircleMan.MyActivity}";
                    return NodeStatus.Failure;
                }

                _targetDestination = destinationGO.transform.position;

                GameManager.Instance.CircleMan.SetDestination(_targetDestination);
                StatusReason = $"Starting to navigate to {destinationGO.transform.position}";

                return NodeStatus.Running;
            }

            float distanceToTarget = Vector3.Distance(_targetDestination, GameManager.Instance.CircleMan.transform.position);

            if (distanceToTarget < 0.25f)
            {
                StatusReason = $"Navigation ended. " +
                               $"\n - Evaluation Count: {EvaluationCount}. " +
                               $"\n - Target Destination: {_targetDestination}" +
                               $"\n - Distance to target: {Math.Round(distanceToTarget, 1)}";

                return NodeStatus.Success;
            }

            StatusReason = $"Distance to target: {distanceToTarget}";
            return NodeStatus.Running;
        }
    }
}