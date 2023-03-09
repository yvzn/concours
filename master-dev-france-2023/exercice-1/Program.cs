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
		static void Main(string[] args)
		{
			string line;
			int X = 0, Y = 0, N = 0;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				var ligne = Lire1(line);
				X = ligne.Item1;
				Y = ligne.Item2;
				N = ligne.Item3;
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			Console.WriteLine(X * Y % N == 0 ? "YES" : "NO");
		}

		private static (int, int, int) Lire1(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
			return (split[0], split[1], split[2]);
		}
	}
}
