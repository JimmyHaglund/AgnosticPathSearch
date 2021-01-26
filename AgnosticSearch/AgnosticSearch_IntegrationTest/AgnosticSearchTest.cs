using NUnit.Framework;
using AgnosticSearch;
using System.Collections.Generic;
using System.Collections;

namespace AgnosticSearchTest {
    public class When_Searching_Path_Of_10_Elements_In_Graph_With_Int_Coordinates_Ranging_From_0_To_10 {
        MockPathMaker _pathMaker;
        MockGraph _graph;
        PossiblePaths<int, MockNode> _finder;
        int _startKey = 5;
        [SetUp]
        public void Setup() {
            _pathMaker = new MockPathMaker();
            _graph = new MockGraph();
            _finder = new PossiblePaths<int, MockNode>();
        }

        [Test]
        public void Start_Node_Should_Be_In_Graph() {
            var path = _finder.Find(_pathMaker, _startKey, SearchBlocked, _graph);
            Assert.NotNull(path.GetNode(_startKey));
        }

        

        [Test]
        public void Result_Should_Contain_10_Elements() {
            var path = _finder.Find(_pathMaker, _startKey, SearchBlocked, _graph);
            var count = 0;
            foreach (var node in path) count++;
            Assert.AreEqual(10, count);

        }

        [Test]
        public void Result_Should_Contain_9() {
            var path = _finder.Find(_pathMaker, _startKey, SearchBlocked, _graph);
            Assert.NotNull(path.GetNode(9));

        }

        [Test]
        public void Result_Should_Contain_0() {
            var path = _finder.Find(_pathMaker, _startKey, SearchBlocked, _graph);
            var count = 0;
            foreach (var node in path) count++;
            Assert.NotNull(path.GetNode(0));

        }

        [Test]
        public void Result_Should_Not_Cointain_Minus1() {
            var path = _finder.Find(_pathMaker, _startKey, SearchBlocked, _graph);
            Assert.Null(path.GetNode(-1));
        }

        [Test]
        public void Result_Should_Not_Cointain_10() {
            var path = _finder.Find(_pathMaker, _startKey, SearchBlocked, _graph);
            Assert.Null(path.GetNode(10));
        }

        private static bool SearchBlocked(MockNode node) => node.Key > 9 || node.Key < 0;

        private class MockNode : IPathNode<int, MockNode> {
            public int Key { get; set; }
            public MockNode Previous { get; set; }
            public MockNode Next { get; set; }
            public float TraveledCost { get; set; }
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
                return new List<int>[] { 
                    new List<int>() { centre }, 
                    new List<int>() { centre - 1, centre + 1 } 
                };
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
                return _nodes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
    }
}