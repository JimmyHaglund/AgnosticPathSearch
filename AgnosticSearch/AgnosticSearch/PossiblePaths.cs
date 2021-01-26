using System;
using System.Linq;
using System.Collections.Generic;

namespace AgnosticSearch {
    public class PossiblePaths<TKey, TNode> where TNode : class, IPathNode<TKey, TNode> where TKey : IEquatable<TKey> {
        public IPathGraph<TKey, TNode> Find(
            IPathMaker<TKey, TNode> pathMaker,
            TKey startKey,
            Func<TNode, bool> pathingBlocked,
            IGraph<TKey> graph
            ) {

            var explored = pathMaker.GetGraph();
            var currentNode = pathMaker.MakeNode(startKey, null);
            var frontier = new PriorityList<float, TNode>();

            while (currentNode != null) {
                explored.Add(currentNode);
                var surroundingNodes = GetSurroundingNodes(graph, currentNode, explored);
                ExploreSurroundingNodes(pathingBlocked, currentNode, frontier, surroundingNodes, pathMaker, explored);

                if (frontier.Count == 0) {
                    currentNode = null;
                    break;
                }
                currentNode = frontier[frontier.Count - 1];
                frontier.RemoveAt(frontier.Count - 1);
            }
            return explored;
        }

        private IEnumerable<TKey> GetSurroundingNodes(IGraph<TKey> graph, IPathNode<TKey, TNode> currentNode, IPathGraph<TKey, TNode> explored) {
            var key = currentNode.Key;
            return from coordinate in graph.GetSurroundingNodes(key, 1)[1]
                   where explored.GetNode(key) == null
                   select coordinate;
        }

        private void ExploreSurroundingNodes(
            Func<TNode, bool> pathingBlocked,
            TNode currentNode,
            PriorityList<float, TNode> frontier,
            IEnumerable<TKey> surroundingNodes,
            IPathMaker<TKey, TNode> pathMaker,
            IPathGraph<TKey, TNode> explored) {
            foreach (TKey key in surroundingNodes) {
                TNode newNode = pathMaker.MakeNode(key, currentNode);
                if (pathingBlocked(newNode)) {
                    continue;
                }
                AddNodeToFrontier(explored, newNode, frontier);
            }
        }

        void AddNodeToFrontier(IPathGraph<TKey, TNode> path, TNode newNode, PriorityList<float, TNode> frontier) {
            var key = newNode.Key;
            var travelCost = newNode.TraveledCost;
            var lastNode = newNode.Previous;
            var n = GetFrontierNodeIndex(path, frontier, key);
            if (n >= 0) {
                if (frontier[n].TraveledCost > travelCost) {
                    var node = frontier[n];
                    frontier[n].TraveledCost = travelCost;
                    frontier[n].Previous = lastNode;
                    frontier.RemoveAt(n);
                    frontier.Insert(travelCost, node);
                }
            } else {
                frontier.Insert(travelCost, newNode);
            }
        }

        int GetFrontierNodeIndex(IPathGraph<TKey, TNode> path, PriorityList<float, TNode> frontier, TKey key) {
            for (int n = 0; n < frontier.Count; n++) {
                if (frontier[n].Key.Equals(key)) return n;
            }
            return -1;
        }
    }
}
