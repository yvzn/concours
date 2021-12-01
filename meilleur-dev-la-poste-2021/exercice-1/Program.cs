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
		static int? nbLignes;
		static int? nbColonnes;

		static List<string> lignesPeinture = new List<string>();

		static void Main(string[] args)
		{
			string ligne;
			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nbLignes.HasValue)
				{
					(nbLignes, nbColonnes) = Lire(ligne);
					continue;
				}

				lignesPeinture.Add(ligne);
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var ligneCentrale = lignesPeinture[nbLignes.Value / 2];
			var caractereCentral = ligneCentrale[nbColonnes.Value / 2];
			Console.WriteLine(caractereCentral);
		}

		private static (int, int) Lire(string ligne)
		{
			var split = ligne.Split(' ');
			return (int.Parse(split[0]), int.Parse(split[1]));
		}
	}
}
