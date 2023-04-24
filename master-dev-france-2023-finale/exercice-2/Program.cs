using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*******
 * Read input from Console
 * Use: Console.WriteLine to output your result to STDOUT.
 * Use: Console.Error.WriteLine to output debugging information to STDERR;
 *       
 * ***/

namespace CSharpContestProject
{
	class Program
	{
		static int? N = default;
		static int? M = default;
		static int? T = default;
		static List<int> positionTricheurs = new List<int>();
		static ICollection<Liaison> liaisons = new List<Liaison>();

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!N.HasValue)
				{
					(N, M, T) = Lire1(line);
					continue;
				}

				if (positionTricheurs.Count == 0)
				{
					positionTricheurs = Lire2(line).ToList();
					continue;
				}

				var ligne = Lire2(line).ToList();
				liaisons.Add(new Liaison
				{
					Depart = ligne[0],
					Arrivee = ligne[1],
				});
				liaisons.Add(new Liaison
				{
					Depart = ligne[1],
					Arrivee = ligne[0],
				});
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var (distanceVainqueur, _) = BellmanFord(1);
			var distancesTricheurs = positionTricheurs.Select(p =>
			{
				var (d, _) = BellmanFord(p);
				return d;
			}).ToList();

			var batimentsAccessibles = Enumerable
				.Range(1, N.Value)
				.Where(b => !distancesTricheurs.Any(d => d[b] < int.MaxValue && d[b] <= distanceVainqueur[b]));
			Console.WriteLine(string.Join(" ", batimentsAccessibles.OrderBy(x => x)));
		}

		private static (int[] d, int?[] pred) BellmanFord(int source)
		{
			// https://fr.wikipedia.org/wiki/Algorithme_de_Bellman-Ford

			//    pour u dans S faire
			//    |       d[u] = +∞
			//    |       pred[u] = null
			//    d[s] = 0
			//    //Boucle principale
			//    pour k = 1 jusqu'à taille(S) - 1 faire
			//     |      pour chaque arc (u, v) du graphe faire
			//     |      |    si d[u] + poids(u, v) < d[v] alors
			//     |      |    |    d[v] := d[u] + poids(u, v)
			//     |      |    |    pred[v]:= u

			//    retourner d, pred
			var taille = N.Value;
			var graphe = liaisons;
			var s = source;

			var d = new int[taille + 1];
			var pred = new int?[taille + 1];

			for (var u = 1; u < taille; ++u)
			{
				d[u] = int.MaxValue;
				pred[u] = default;
			}

			d[s] = 0;

			for (var k = 1; k < taille; ++k)
			{
				foreach (var arc in graphe)
				{
					var u = arc.Depart;
					var v = arc.Arrivee;
					var poids = 1;
					if (d[u] < int.MaxValue && d[u] + poids < d[v])
					{
						d[v] = d[u] + poids;
						pred[v] = u;
					}
				}
			}

			return (d, pred);
		}

		private static (int, int, int) Lire1(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
			return (split[0], split[1], split[2]);
		}

		private static IEnumerable<int> Lire2(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(int.Parse);
		}

		internal class Liaison
		{
			public int Depart { get; set; }
			public int Arrivee { get; set; }
		}
	}
}
