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
		static int? nombreParcelles = default;

		static IList<int> largeurParcelles = default;

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nombreParcelles.HasValue)
				{
					nombreParcelles = Lire(line).First();
					continue;
				}

				largeurParcelles = Lire(line).ToList();
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var longueurTotaleHaie = 0;
			var largeurPrecedente = 0;
			foreach (var largeur in largeurParcelles)
			{
				longueurTotaleHaie += 3 * largeur;
				if (largeur > largeurPrecedente)
				{
					longueurTotaleHaie += largeur - largeurPrecedente;
				}
				largeurPrecedente = largeur;
			}
			Console.WriteLine(longueurTotaleHaie);
		}

		private static IEnumerable<int> Lire(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(int.Parse);
		}
	}
}
