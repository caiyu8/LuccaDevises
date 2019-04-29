
namespace LuccaDevises
{
    /// <summary>
    /// Represent a direct link between two currencies
    /// </summary>
    public class Edge
    {
        public string Start { get; private set; }
        public string End { get; private set; }
        public decimal ExchaneRate { get; private set; }

        public Edge(string start, string end, decimal rate)
        {
            Start = start;
            End = end;
            ExchaneRate = rate;
        }
    }
}
