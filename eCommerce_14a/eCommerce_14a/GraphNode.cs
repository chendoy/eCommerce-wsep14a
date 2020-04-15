

using System.Collections.Generic;

/*.NET graph implementaion
 https://docs.microsoft.com/en-us/previous-versions/ms379574(v=vs.80)?redirectedfrom=MSDN
 */
namespace eCommerce_14a
{
    public class GraphNode<T>
    {
        private T val;
        private List<int> costs;
        private List<GraphNode<T>> neighbors;
        public GraphNode(T value)
        {
            this.val = value;
            this.neighbors = null;
        }
        public GraphNode(T value, List<GraphNode<T>> neighbors)
        {
            this.val = value;
            this.neighbors = neighbors;

        }

        public List<GraphNode<T>> Neighbors
        {
            get { return neighbors; }
        }
        public T Value
        {
            get { return val; }
            set { val = value; }
        }

        public List<int> Costs
        {
            get
            {
                if (costs == null)
                    costs = new List<int>();

                return costs;
            }
        }
    }
}
