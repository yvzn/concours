using System;
using System.Collections.Generic;
using System.Linq;

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
			var header = true;
			var catalog = new Dictionary<string, int>();
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (header)
				{

				}
				else
				{
					var data = line.Split(' ');
					var score = int.Parse(data[0]);
					var label = data[1];
					catalog.Add(label, score);
				}

				header = false;
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var result = catalog.OrderByDescending(x => x.Value).FirstOrDefault();
			Console.WriteLine(result.Key);
		}
	}
}
