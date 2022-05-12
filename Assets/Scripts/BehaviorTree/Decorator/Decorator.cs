namespace BehaviorTree.Decorator
{
    public abstract class Decorator : NodeBase
    {
        public Decorator(string displayName, NodeBase NodeBase)
        {
            Name = displayName;
            ChildNodes.Add(NodeBase);
        }
    }
}