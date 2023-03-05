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
		static string ligne1 = default;
		static string ligne2 = default;
		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (ligne1 is null)
				{
					ligne1 = line;
					continue;
				}

				if (ligne2 is null)
				{
					ligne2 = line;
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var sb = new StringBuilder();
			Enumerable.Zip(ligne1, ligne2, (c1, c2) => new { First = c1, Second = c2 }).ToList().ForEach(tuple => sb.Append(tuple.First).Append(tuple.Second));
			Console.WriteLine(sb.ToString());
		}
	}
}
