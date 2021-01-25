using System.Collections.Generic;
namespace AgnosticSearch {
    public interface IPathGraph<TKey, TNode> : IEnumerable<TNode> {
        void Add(TNode node);
        bool Contains(TNode node);
        void RemoveNode(TNode node);
        TNode GetNode(TKey key);
    }
}