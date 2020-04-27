using System;
using System.Collections.Generic;
using TreeSharpPlus;

namespace StoryScripts
{
    public class RandomSelector : NodeGroup
    {
        private int randomIndex;

        public RandomSelector(params Node[] children)
            : base(children)
        {
            randomIndex = new Random().Next(0, children.Length);
             
        }
        public override IEnumerable<RunStatus> Execute()
        {
            var node = Children[randomIndex];
            Selection = node;
            node.Start();

            // If the current node is still running, report that. Don't 'break' the enumerator
            RunStatus result;
            while ((result = TickNode(node)) == RunStatus.Running)
                yield return RunStatus.Running;
            
            // Call Stop to allow the node to clean anything up.
            node.Stop();
            
            // return the status wether this succeeds or fails, we do not care a
            yield return result;
        }
    }
}