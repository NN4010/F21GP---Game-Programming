public enum EdgeType
{
    Walk,
    Jump
}

public class Edge
{
    public Node target;
    public float cost;
    public EdgeType type;

    public Edge(Node target, float cost, EdgeType type)
    {
        this.target = target;
        this.cost = cost;
        this.type = type;
    }
}