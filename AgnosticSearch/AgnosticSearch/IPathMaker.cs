namespace AgnosticSearch {
    public interface IPathMaker<TKey, TNode> {
        TNode MakeNode(TKey key, TNode previous);
        IPathGraph<TKey, TNode> GetGraph();
    }
}