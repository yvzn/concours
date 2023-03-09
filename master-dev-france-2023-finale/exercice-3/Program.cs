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
		static int? P = default;
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
					(N, P) = Lire1(line);
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données

		}

		private static (int, int) Lire1(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
			return (split[0], split[1]);
		}

		private static IEnumerable<int> Lire2(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(int.Parse);
		}
	}
}
