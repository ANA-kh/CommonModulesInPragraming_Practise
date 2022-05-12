using UnityEngine;

namespace BehaviorTree.Decorator
{
    /// <summary>
    /// 一段时间内一直运行子节点
    /// </summary>
    public class Timer : Decorator
    {
        private float _startTime;
        private bool _useFixedTime;
        private float _timeToWait;

        public Timer(float timeToWait, NodeBase childNode,  bool useFixedTime = false) : 
            base($"Timer for {timeToWait}", childNode) 
        {
            _useFixedTime = useFixedTime;
            _timeToWait = timeToWait;
        }
        
        protected override void OnReset() { }
        protected override NodeStatus OnRun()
        {
            if (ChildNodes.Count == 0 || ChildNodes[0] == null)
            {
                return NodeStatus.Failure;
            }

            NodeStatus originalStatus = (ChildNodes[0] as NodeBase).Run();

            if (EvaluationCount == 0)
            {
                StatusReason = $"Starting timer for {_timeToWait}. Child NodeBase status is: {originalStatus}";
                _startTime = _useFixedTime ? Time.fixedTime : Time.time;
            }

            float elapsedTime = Time.fixedTime - _startTime;

            if (elapsedTime > _timeToWait)
            {
                StatusReason = $"Timer complete - Child NodeBase status is: { originalStatus}";
                return NodeStatus.Success;
            }

            StatusReason = $"Timer is {elapsedTime} out of {_timeToWait}. Child NodeBase status is: {originalStatus}";
            return NodeStatus.Running;

        }
    }
}