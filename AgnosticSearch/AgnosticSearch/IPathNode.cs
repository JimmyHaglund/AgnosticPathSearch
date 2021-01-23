namespace AgnosticSearch {
    public interface IPathNode<TKey, TNode> where TNode:IPathNode<TKey, TNode> {
        TKey Key { get; }
        TNode Previous { get; set; }
        TNode Next { get; set; }
        float TraveledCost { get; set; }
    }
}