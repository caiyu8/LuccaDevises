using System;
using System.Collections.Generic;

namespace LuccaDevises
{
    public class Graph
    {
        /// <summary>
        /// Adjacency matrix of current graph
        /// </summary>
        public Dictionary<string, List<string>> Neighbours { get; private set; }

        public Graph(IEnumerable<Edge> edges)
        {
            InitGraph(edges);
        }

        /// <summary>
        /// Initialize the adjacency matrix
        /// </summary>
        /// <param name="edges">Direct links between two vertex</param>
        private void InitGraph(IEnumerable<Edge> edges)
        {
            Neighbours = new Dictionary<string, List<string>>();

            foreach (var e in edges)
            {
                if (!Neighbours.ContainsKey(e.Start))
                {
                    Neighbours[e.Start] = new List<string>();
                }

                if (!Neighbours.ContainsKey(e.End))
                {
                    Neighbours[e.End] = new List<string>();
                }

                Neighbours[e.Start].Add(e.End);
                Neighbours[e.End].Add(e.Start);
            }
        }

        /// <summary>
        /// Using Breadth First Search to find shorest path between two points on a graph
        /// Reference: https://www.youtube.com/watch?v=s-CYnVz-uh4
        /// </summary>
        /// <param name="sourceVertex">Start point</param>
        /// <param name="endVertex">Destination point</param>
        /// <returns>Shortest path containing each vertex</returns>
        public List<string> BreadthFirstSearch(string sourceVertex, string endVertex)
        {
            if(IsGraphEmpty())
            {
                throw new Exception($"The graph is empty");
            }
            else if (!HasVertex(sourceVertex))
            {
                throw new Exception($"The start vertex {sourceVertex} is not in the graph");
            }else if(!HasVertex(endVertex))
            {
                throw new Exception($"The end vertex {endVertex} is not in the graph");
            }

            //Initialization
            var Parents = new Dictionary<string, string>();//Track vertex parents
            var Levels = new Dictionary<string, int>(); //track visited vertex
            var frontiers = new List<string>();//Track every level of vertex

            //Set start point to Level 0
            frontiers.Add(sourceVertex);
            Levels.Add(sourceVertex, 0);
            Parents.Add(sourceVertex, null);
            //Set next level
            var level = 1;

            //Traverse the graph level by level
            while (frontiers.Count > 0)
            {
                var Next = new List<string>();
               
                foreach (var u in frontiers)
                {
                    foreach(var v in Neighbours[u])
                    {
                        if (!Levels.ContainsKey(v))
                        {
                            Levels[v] = level; //Set level
                            Parents[v] = u; //Set parent
                            Next.Add(v);
                        }
                    }
                }
                frontiers = Next;
                level += 1;
            }

            if(!Parents.ContainsKey(endVertex))
            {
                return new List<string>();
                //throw new Exception("Path not found");
            }
            
            var parent = Parents[endVertex]; //end point's parent
            var path = new List<string> { endVertex };

            //Construct shortest path
            while (parent != sourceVertex)
            {
                path.Add(parent);
                parent = Parents[parent];
            }

            path.Add(sourceVertex);
            path.Reverse();

            return path;
        }

        public bool HasVertex(string vertex)
        {
            return Neighbours.ContainsKey(vertex);
        }

        public bool IsGraphEmpty()
        {
            return Neighbours == null || Neighbours.Count == 0;
        }
    }
}
