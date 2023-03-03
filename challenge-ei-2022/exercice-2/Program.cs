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
		static int? nombreChauffages = default;
		static ICollection<int> chauffages = default;
		static ICollection<int> maisons = default;

		static void Main(string[] args)
		{
			string ligne;

			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if(!nombreChauffages.HasValue) {
					nombreChauffages = 0;
					continue;
				}

				if (chauffages is null) {
					chauffages = Lire(ligne).ToArray();
					continue;
				}

				if (maisons is null) {
					maisons = Lire(ligne).ToArray();
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var result = maisons.Select(maison => chauffages.Where(chauffage => maison <= chauffage).OrderBy(x => x).FirstOrDefault()).Sum();
			Console.WriteLine(result);
		}

		private static IEnumerable<int> Lire(string ligne)
		{
			var split = ligne.Split(' ');
			return split.Select(int.Parse);
		}
	}
}
