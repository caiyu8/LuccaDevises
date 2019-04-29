using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LuccaDevises
{
    public class CurrencyConvertor
    {
        public IList<Edge> Edges { get; private set; }
        public string SourceCurrency { get; private set; }
        public int Amount { get; private set; }
        public string TargetCurrency { get; private set; }

        public CurrencyConvertor(string path)
        {
            InitCurrencyData(path);
        }

        public int Convert()
        {
            var CurrencyGraph = new Graph(Edges);

            var path = CurrencyGraph.BreadthFirstSearch(SourceCurrency, TargetCurrency);

            if (path == null || path.Count == 0)
            {
                throw new Exception($"Cann't convert {SourceCurrency} to {TargetCurrency}, no possible path found");
            }

            Console.WriteLine("\nShortest conversion steps:");
            Console.WriteLine($"{string.Join("=>", path)}");

            decimal result = Amount;

            for (var i = 0; i < path.Count - 1; i++)
            {
                result *= GetExchangeRate(path[i], path[i + 1]);
            }

            Console.WriteLine("\nResult:");
            Console.WriteLine($"{Amount} {SourceCurrency} = {System.Convert.ToInt32(result)} {TargetCurrency} ");

            return System.Convert.ToInt32(result);
        }

        void InitCurrencyData(string filePath)
        {
            var lines = File.ReadLines(filePath);
            var line1 = lines.Take(1).First().Split(';');
            var numberOfExchangeRates = lines.Skip(1).Take(1).First();
            SourceCurrency = line1[0];
            Amount = int.Parse(line1[1]);
            TargetCurrency = line1[2];

            Edges = new List<Edge>();
            lines.Skip(2).ToList().ForEach(line =>
            {
                var s = line.Split(';');
                var e = new Edge(s[0].Trim(), s[1].Trim(), decimal.Parse(s[2].Trim(), CultureInfo.InvariantCulture));
                Edges.Add(e);
            });

            Console.WriteLine("File content:");
            Console.WriteLine($"First line : Source currency:{SourceCurrency}, Amount: {Amount}, Target currency:{TargetCurrency}");
            Console.WriteLine($"Second line: Number of exchange rates : {numberOfExchangeRates}");
            Console.WriteLine("Other lines:");
            Edges.ToList().ForEach(e => Console.WriteLine($"{e.Start} --> { e.End}, Exchange rate: {e.ExchaneRate}"));
        }


        /// <summary>
        /// Return exchange rate between two direct linked currencies.
        /// Divided by 1 if in reverse order.
        /// </summary>
        /// <param name="start">source currency</param>
        /// <param name="end">Destination currency</param>
        /// <returns>Exchange rate between direct linked currencies</returns>
        decimal GetExchangeRate(string start, string end)
        {
            var edge = Edges.SingleOrDefault(e => e.Start == start && e.End == end);
            var rate = 1M;

            if (edge != null)
            {
                rate = edge.ExchaneRate;
                Console.WriteLine($"{edge.Start} => {edge.End}, rate:{edge.ExchaneRate}");
            }
            else
            {
                var edge1 = Edges.SingleOrDefault(e => e.Start == end && e.End == start);
                rate = decimal.Round(1 / edge1.ExchaneRate, 4);
                Console.WriteLine($"{edge1.Start} => {edge1.End}, rate: {edge1.ExchaneRate}, 1/rate: {rate}");
            }

            return rate;
        }
    }
}
