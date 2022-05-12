namespace BehaviorTree.Composite
{
    /// <summary>
    ///  无权重选择，返回 或
    /// </summary>
    public class Selector : Composite
    {
        public Selector(string displayName, params NodeBase[] childNodes) : base(displayName, childNodes)
        {
        }

        protected override NodeStatus OnRun()
        {
            if (CurrentChildIndex >= ChildNodes.Count)
            {
                return NodeStatus.Failure;
            }
            
            NodeStatus nodeStatus = (ChildNodes[CurrentChildIndex] as NodeBase).Run();

            switch (nodeStatus)
            {
                case NodeStatus.Failure:
                    CurrentChildIndex++;
                    //return OnRun();//失败时可以直接返回或者return OnRun()
                    break;
                case NodeStatus.Success:
                    return NodeStatus.Success;
            }

            return NodeStatus.Running;
        }

        protected override void OnReset()
        {
            CurrentChildIndex = 0;

            for (int i = 0; i < ChildNodes.Count; i++)
            {
                (ChildNodes[i] as NodeBase).Reset();
            }
        }
    }
}