namespace BehaviorTree.Decorator
{
    /// <summary>
    /// 反转节点，返回与子节点状态相反的状态
    /// </summary>
    public class Inverter : Decorator
    {
        public Inverter(string displayName, NodeBase childNode) : base(displayName, childNode) { }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {

            if (ChildNodes.Count == 0 || ChildNodes[0] == null)
            {
                return NodeStatus.Failure;
            }

            NodeStatus originalStatus = (ChildNodes[0] as NodeBase).Run();

            switch (originalStatus)
            {
                case NodeStatus.Failure:
                    return NodeStatus.Success;
                case NodeStatus.Success:
                    return NodeStatus.Failure;
            }

            return originalStatus;

        }
    }
}