using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		static int? numeroEtapeFinale;
		static decimal? dureeMaximum;
		static ICollection<Liaison> liaisons = new List<Liaison>();

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				var ligne = Lire(line).ToArray();

				if (!numeroEtapeFinale.HasValue)
				{
					numeroEtapeFinale = ligne[0];
					dureeMaximum = ligne[1];
					continue;
				}

				if (ligne.Length >= 4)
				{
					liaisons.Add(new Liaison
					{
						Depart = ligne[0],
						Arrivee = ligne[1],
						Duree = ligne[2],
						CoutCarbone = ligne[3],
					});
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var (dureeDepuisOrigine, _) = BellmanFordDuree();

			var (d, _) = BellmanFordCoutCarbone(dureeDepuisOrigine);

			if (d[numeroEtapeFinale.Value] == decimal.MaxValue)
			{
				Console.WriteLine(-1);
				return;
			} 

			Console.WriteLine(d[numeroEtapeFinale.Value]);
		}

		private static (decimal[] d, decimal?[] pred) BellmanFordDuree()
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
			var taille = numeroEtapeFinale.Value + 1;
			var graphe = liaisons;
			var s = 0;

			var d = new decimal[taille];
			var pred = new decimal?[taille];

			for (var u = 1; u < taille; ++u)
			{
				d[u] = decimal.MaxValue;
				pred[u] = default;
			}

			d[s] = 0;

			for (var k = 1; k < taille; ++k)
			{
				foreach (var arc in graphe)
				{
					var u = arc.Depart;
					var v = arc.Arrivee;
					var poids = arc.Duree;
					if (d[u] + poids < d[v])
					{
						d[v] = d[u] + poids;
						pred[v] = u;
					}
				}
			}

			return (d, pred);
		}

		private static (decimal[] d, decimal?[] pred) BellmanFordCoutCarbone(decimal[] dureeDepuisOrigine)
		{
			var taille = numeroEtapeFinale.Value + 1;
			var graphe = liaisons;
			var s = 0;

			var d = new decimal[taille];
			var pred = new decimal?[taille];

			for (var u = 1; u < taille; ++u)
			{
				d[u] = decimal.MaxValue;
				pred[u] = default;
			}

			d[s] = 0;

			for (var k = 1; k < taille; ++k)
			{
				foreach (var arc in graphe)
				{
					var u = arc.Depart;
					var v = arc.Arrivee;
					var poids = arc.CoutCarbone;
					var respectDureeMaximum = dureeDepuisOrigine[u] < decimal.MaxValue && dureeDepuisOrigine[u] + arc.Duree < dureeMaximum;
					if (d[u] < decimal.MaxValue && d[u] + poids < d[v] && respectDureeMaximum)
					{
						d[v] = d[u] + poids;
						pred[v] = u;
					}
				}
			}

			return (d, pred);
		}
		private static IEnumerable<int> Lire(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(int.Parse);
		}
	}

	internal class Liaison
	{
		public int Depart { get; set; }
		public int Arrivee { get; set; }
		public decimal Duree { get; set; }
		public decimal CoutCarbone { get; set; }
	}
}
