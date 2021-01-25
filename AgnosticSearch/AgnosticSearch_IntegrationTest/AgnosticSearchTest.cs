using NUnit.Framework;
using AgnosticSearch;
using System.Collections.Generic;
using System.Collections;

namespace AgnosticSearchTest {
    public class When_Searching_Path_Of_10_Elements {
        MockPathMaker _pathMaker;
        MockGraph _graph;
        PossiblePaths<int, MockNode> _finder;
        [SetUp]
        public void Setup() {
            _pathMaker = new MockPathMaker();
            _graph = new MockGraph();
            _finder = new PossiblePaths<int, MockNode>();
        }

        [Test]
        public void Start_Node_Should_Be_In_Graph() {
            var startKey = 5;
            var path = _finder.Find(_pathMaker, startKey, SearchBlocked, _graph);
            Assert.NotNull(path.GetNode(startKey));
        }

        [Test]
        public void Key_Outside_Range_Should_Not_Be_In_Graph() {
            var startKey = 5;
            var outsideRangeKey = -1;
            var path = _finder.Find(_pathMaker, startKey, SearchBlocked, _graph);
            Assert.Null(path.GetNode(outsideRangeKey));
        }

        private static bool SearchBlocked(MockNode node) => node.Key >= 10 || node.Key < 0;

        private class MockNode : IPathNode<int, MockNode> {
            public int Key { get; set; } = 0;
            public MockNode Previous { get; set; } = default;
            public MockNode Next { get; set; } = default;
            public float TraveledCost { get; set; } = default;
            public MockNode(int key, MockNode previous) {
                Key = key;
                Previous = previous;
            }
        }

        private class MockPathMaker : IPathMaker<int, MockNode> {
            private MockPathGraph _graph = new MockPathGraph();

            public IPathGraph<int, MockNode> GetGraph() => _graph;
            public MockNode MakeNode(int key, MockNode previous) {
                return new MockNode(key, previous);
            }
        }

        private class MockGraph : IGraph<int> {
            public ICollection<int>[] GetSurroundingNodes(int centre, int steps) {
                return new List<int>[] { new List<int>() { centre }, new List<int>() { centre - 1, centre + 1 } };
            }
        }

        private class MockPathGraph : IPathGraph<int, MockNode> {
            private ICollection<MockNode> _nodes = new LinkedList<MockNode>();

            public void Add(MockNode node) => _nodes.Add(node);
            public void RemoveNode(MockNode node) => _nodes.Remove(node);
            public bool Contains(MockNode node) => _nodes.Contains(node);
            public MockNode GetNode(int key) {
                foreach(MockNode node in _nodes) {
                    if (node.Key == key) return node;
                }
                return null;
            }

            public IEnumerator<MockNode> GetEnumerator() {
                return null;
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return null;
            }
        }
    }
}