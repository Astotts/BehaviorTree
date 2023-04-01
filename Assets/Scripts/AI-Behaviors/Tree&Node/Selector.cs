using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree

{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue; //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/jump-statements
                        //continue jumps out of the immediate most "enclosing iteration statement" (bracket pair if,while,for,switch)
                        //break jumps out of all loops
                        //return jumps out of the function alltogether
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }
            //In other words if node state is FAILURE evalute the next child
            //If node state is success execute that node and do not evalute other
            //If node state is running execut that node and do not evalute other

            //chooses the first node to evalute as success to continue (OR logic gate)

            state = NodeState.FAILURE;
            return state;
        }

    }

}

