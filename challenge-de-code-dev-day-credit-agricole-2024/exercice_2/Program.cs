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
		static int? nombreYeux;
		static int? nombreJambes;
		static int? nombreQueues;
		static int maxHumains;
		static int maxChiens;
		static int maxOiseaux;

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nombreYeux.HasValue)
				{
					nombreYeux = Lire(line);
					continue;
				}
				if (!nombreJambes.HasValue)
				{
					nombreJambes = Lire(line);
					continue;
				}
				if (!nombreQueues.HasValue)
				{
					nombreQueues = Lire(line);
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			maxHumains = new int[] { nombreYeux.Value / 2, nombreJambes.Value / 2 }.Min();
			maxChiens = new int[] { nombreYeux.Value / 2, nombreJambes.Value / 4, nombreQueues.Value }.Min();
			maxOiseaux = new int[] { nombreYeux.Value / 2, nombreJambes.Value / 2, nombreQueues.Value }.Min();

			var possibles = Configurations();
			foreach (var configuration in possibles)
			{
				if (EstValide(configuration))
				{
					Console.WriteLine(configuration.nombreHumains);
					Console.WriteLine(configuration.nombreChiens);
					Console.WriteLine(configuration.nombreOiseaux);
					return;
				}
			}

			Console.WriteLine("Hallucination");
		}

		private static bool EstValide((int nombreHumains, int nombreChiens, int nombreOiseaux) configuration)
		{
			var totalYeux = 2 * (configuration.nombreHumains + configuration.nombreChiens + configuration.nombreOiseaux);
			var totalJambes = 2 * (configuration.nombreOiseaux + configuration.nombreHumains) + 4 * configuration.nombreChiens;
			var totalQueues = configuration.nombreOiseaux + configuration.nombreChiens;

			return
				totalYeux == nombreYeux &&
				totalJambes == nombreJambes &&
				totalQueues == nombreQueues;
		}

		private static List<(int nombreHumains, int nombreChiens, int nombreOiseaux)> Configurations()
		{
			var result = new List<(int nombreHumains, int nombreChiens, int nombreOiseaux)>();
			var nombreIndividus = nombreYeux.Value / 2;
			for (int i = 0; i <= maxHumains; i++)
			{
				for (int j = 0; j <= maxChiens; j++)
				{
					for (int k = 0; k <= maxOiseaux; k++)
					{
						if (i + j + k == nombreIndividus && j + k == nombreQueues)
							result.Add((i, j, k));
					}
				}
			}
			return result;
		}

		private static int Lire(string line)
		{
			return int.Parse(line);
		}
	}
}
