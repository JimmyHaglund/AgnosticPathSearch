using System.Collections.Generic;

namespace AgnosticSearch {
    public interface IGraph<CoordinateType> {
        ICollection<CoordinateType>[] GetSurroundingNodes(CoordinateType centre, int steps);
    }
}