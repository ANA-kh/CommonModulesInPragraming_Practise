using System.Linq;

namespace BehaviorTree.Composite
{
    public abstract class Composite : NodeBase
    {
        protected int CurrentChildIndex = 0;
        
        protected Composite(string displayName, params NodeBase[] childNodes)
        {
            Name = displayName;

            ChildNodes.AddRange(childNodes.ToList());
        }
    }
}