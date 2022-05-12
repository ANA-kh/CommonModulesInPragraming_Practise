using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeStatus
    {
        Failure,
        Success,
        Running,
        Unknown,
        NotRun
    }
    public abstract class NodeBase
    {
        public int EvaluationCount;
        public string Name { get; set; }
        public string StatusReason { get; set; } = "";
        public List<NodeBase> ChildNodes = new List<NodeBase>();

        /// <summary>
        /// 节点运行
        /// </summary>
        /// <returns>当前节点的status</returns>
        public virtual NodeStatus Run()
        {
            NodeStatus nodeStatus = OnRun();
            EvaluationCount++;

            if (nodeStatus != NodeStatus.Running)
            {
                Reset();
            }

            return nodeStatus;
        }
        
        /// <summary>
        /// 复位
        /// </summary>
        public void Reset()
        {
            EvaluationCount = 0;
            OnReset();
        }

        /// <summary>
        /// 运行自定义逻辑
        /// </summary>
        /// <returns>当前节点的status</returns>
        protected abstract NodeStatus OnRun();
        
        /// <summary>
        /// 自定义复位
        /// </summary>
        protected abstract void OnReset();
    }
}