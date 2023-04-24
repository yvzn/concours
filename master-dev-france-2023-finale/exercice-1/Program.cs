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
		static List<Soumission> soumissions = new List<Soumission>();

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
				var temps = int.Parse(ligne[0]);
				var hash = ligne[1];
				soumissions.Add(new Soumission { temps = temps, hash = hash });
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var duplicatedHashes = soumissions
			.GroupBy(s => s.hash)
			.Where(g => g.Count() > 1)
			.Select(g => g.First().hash)
			.ToHashSet();

			var result = soumissions.Where(s => !duplicatedHashes.Contains(s.hash)).Select(s => s.temps).OrderBy(x => x);
			result.ToList().ForEach(Console.WriteLine);
		}

		private static (int, int) Lire1(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
			return (split[0], split[1]);
		}

		private static IEnumerable<string> Lire2(string ligne)
		{
			return ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		}
	}

	internal class Soumission
	{
		public int temps { get; set; }
		public string hash { get; set; }
	}
}
