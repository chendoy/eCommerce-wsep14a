using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*.NET graph implementaion
 https://docs.microsoft.com/en-us/previous-versions/ms379574(v=vs.80)?redirectedfrom=MSDN
 */
namespace eCommerce_14a
{
    public class Graph<T>
    {
        private List<GraphNode<T>> nodeSet;

        public Graph()
        {
            this.nodeSet = new List<GraphNode<T>>();
        }
        public Graph(List<GraphNode<T>> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new List<GraphNode<T>>();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            nodeSet.Add(node);
        }

        public void AddNode(T value)
        {
            // adds a node to the graph
            nodeSet.Add(new GraphNode<T>(value));
        }

        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public bool Contains(T value)
        {
            return FindByValue(value) != null;
        }


        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphNode<T> gnode in nodeSet)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public List<GraphNode<T>> Nodes
        {
            get { return nodeSet; }
        }

        public GraphNode<T> FindByValue(T value)
        {
            foreach (GraphNode<T> node in nodeSet)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            // didn't found
            return null;

        }
        public int Count
        {
            get { return nodeSet.Count; }
        }
    }
}
