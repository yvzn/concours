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
		static int? nombreDevises;
		static int? nombreMaxEchanges;
		static List<Conversion> conversions = new List<Conversion>();

		static void Main(string[] args)
		{
			string line;
			var i = 0;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nombreDevises.HasValue)
				{
					(nombreDevises, nombreMaxEchanges) = Lire1(line);
					continue;
				}

				var ligne = Lire2(line).ToList();
				for (var j = 0; j < ligne.Count && j < nombreDevises; ++j)
				{
					conversions.Add(new Conversion
					{
						Origine = i,
						Destination = j,
						Taux = ligne[j]
					});
				}
				++i;
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var (d, _) = BellmanFord();
			var maxTaux = decimal.MinValue;
			for(var k = 1; k < nombreDevises; ++k) {
				var conversionFinale = conversions.Where(c => c.Destination == 0 && c.Origine == k).FirstOrDefault();
				if (conversionFinale is null) {
					continue;
				}
				if (maxTaux < d[k] * conversionFinale.Taux) {
					maxTaux = d[k] * conversionFinale.Taux;
				}
			}

			var capitalDepart = 10_000;
			Console.WriteLine(maxTaux * capitalDepart);
		}

		private static (decimal[] d, decimal?[] pred) BellmanFord()
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
			var taille = nombreDevises.Value;
			var graphe = conversions;
			var s = 0;

			var d = new decimal[taille];
			var len = new decimal[taille];
			var pred = new decimal?[taille];

			for (var u = 1; u < taille; ++u)
			{
				d[u] = decimal.MinValue;
				len[u] = 0;
				pred[u] = default;
			}

			d[s] = 1;

			for (var k = 0; k < taille; ++k)
			{
				foreach (var arc in graphe)
				{
					var u = arc.Origine;
					var v = arc.Destination;
					var poids = arc.Taux;
					if (d[u] * poids > d[v] && len[u] < nombreMaxEchanges)
					{
						d[v] = d[u] * poids;
						len[v] = len[u] + 1;
						pred[v] = u;
					}
				}
			}

			return (d, pred);
		}

		private static (int, int) Lire1(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
			return (split[0], split[1]);
		}

		private static IEnumerable<decimal> Lire2(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(decimal.Parse);
		}
	}

	internal class Conversion
	{
		public int Origine { get; set; }
		public int Destination { get; set; }
		public decimal Taux { get; set; }
	}
}
