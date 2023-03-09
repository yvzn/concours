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
		static List<List<string>> ingredients = new List<List<string>>();
		static HashSet<string> tousLesIngredients = new HashSet<string>();
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
					N = int.Parse(line);
					continue;
				}

				var ligne = Lire2(line).ToList();
				ingredients.Add(ligne);
				ligne.ForEach(i => tousLesIngredients.Add(i));
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			for (var nb = 1; nb < tousLesIngredients.Count && nb < 13; ++nb)
			{

				foreach (var combinaison in GenererCombinaison(nb))
				{
					if (TousLesClientsSatisfaits(combinaison))
					{
						Console.WriteLine(nb);
						return;
					}
				}
			}
		}

		private static bool TousLesClientsSatisfaits(IEnumerable<string> combinaison)
		{
			var list = combinaison.ToList();
			foreach (var client in ingredients)
			{
				if (!ClientSatisfait(client, list))
				{
					return false;
				}
			}
			return true;
		}

		private static bool ClientSatisfait(List<string> client, IList<string> combinaison)
		{
			return client.Intersect(combinaison).Any();
		}

		private static IEnumerable<IEnumerable<string>> GenererCombinaison(int nb)
		{
			return Combinations(tousLesIngredients, nb);
		}

		private static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> list, int length) where T : IComparable
		{
			if (length == 0) return Array.Empty<IEnumerable<T>>();
			if (length == 1) return list.Select(t => new T[] { t });
			
			return Combinations(list, length - 1)
				.SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
					(t1, t2) => t1.Concat(new T[] { t2 }));
		}

		private static (int, int) Lire1(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
			return (split[0], split[1]);
		}

		private static IEnumerable<string> Lire2(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split;
		}
	}
}
