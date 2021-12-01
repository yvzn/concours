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
		static string sequenceMagique;
		static string texte;
		static void Main(string[] args)
		{
			string ligne;
			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (sequenceMagique == null)
				{
					sequenceMagique = ligne;
					continue;
				}

				if (texte == null)
				{
					texte = ligne;
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var resultat = texte.Split(' ').Select(mot => mot.Contains(sequenceMagique) ? mot : Inverser(mot));
			Console.WriteLine(string.Join(" ", resultat));
		}

		private static string Inverser(string mot)
		{
			var sb = new StringBuilder(mot.Length);
			foreach (var c in mot.Reverse())
			{
				sb.Append(c);
			}
			return sb.ToString();
		}
	}
}
