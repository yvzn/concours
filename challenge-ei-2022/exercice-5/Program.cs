using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		static int? numeroEtapeFinale;
		static int? dureeMaximum;
		static decimal?[,] grapheDuree;
		static decimal?[,] grapheCoutCarbone;

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				var ligne = Lire(line).ToArray();

				if (!numeroEtapeFinale.HasValue)
				{
					numeroEtapeFinale = ligne[0];
					dureeMaximum = ligne[1];
					var nombreEtapes = ligne[0] + 1;
					grapheDuree = new decimal?[nombreEtapes, nombreEtapes]; 
					grapheCoutCarbone = new decimal?[nombreEtapes, nombreEtapes];
					continue;
				}

				if (ligne.Length >= 4)
				{					
						var depart = ligne[0];
						var arrivee = ligne[1];
						var duree = ligne[2];
						var coutCarbone = ligne[3];
						grapheDuree[depart, arrivee] = duree;
						grapheCoutCarbone[depart, arrivee] = coutCarbone;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var coutCarboneMinimal = decimal.MaxValue;

			var stopWatch = new Stopwatch();
			stopWatch.Start();
			var trajets = GenererTrajets();
			stopWatch.Stop();
			Console.Error.WriteLine(stopWatch.Elapsed);

			stopWatch.Restart();
			foreach (var trajet in trajets)
			{
				var duree = trajet.duree;
				var cout = trajet.coutCarbone;
				if (duree < dureeMaximum && cout < coutCarboneMinimal)
				{
					coutCarboneMinimal = cout;
				}
			}
			stopWatch.Stop();
			Console.Error.WriteLine(stopWatch.Elapsed);

			if (coutCarboneMinimal == decimal.MaxValue)
			{
				coutCarboneMinimal = -1;
			}
			Console.WriteLine(coutCarboneMinimal);
		}

		struct Trajet
		{
			public IList<int> etapes;
			public decimal duree;
			public decimal coutCarbone;
		}

		private static IEnumerable<Trajet> GenererTrajets()
		{
			var trajets = new HashSet<Trajet>();
			trajets.Add(new Trajet { etapes = new List<int> { 0 }, duree = 0, coutCarbone = 0 });

			IList<Trajet> trajetsAtraiter;
			do
			{
				trajetsAtraiter = trajets.Where(t => t.etapes.Last() != numeroEtapeFinale).ToList();

				foreach (var trajet in trajetsAtraiter)
				{
					trajets.Remove(trajet);
					for(var arrivee = 1; arrivee <= numeroEtapeFinale; ++arrivee )
					{
						if (!grapheDuree[trajet.etapes.Last(), arrivee].HasValue || !grapheCoutCarbone[trajet.etapes.Last(), arrivee].HasValue) {
							continue;
						}

						var nouveauTrajet = new Trajet
						{
							etapes = trajet.etapes.Append(arrivee).ToList(),
							duree = trajet.duree + grapheDuree[trajet.etapes.Last(), arrivee].Value,
							coutCarbone = trajet.coutCarbone + grapheCoutCarbone[trajet.etapes.Last(), arrivee].Value,
						};
						if (nouveauTrajet.duree < dureeMaximum)
						{
							trajets.Add(nouveauTrajet);
						}
					}
				}
			}
			while (trajetsAtraiter.Any());

			return trajets;
		}

		private static IEnumerable<int> Lire(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(int.Parse);
		}
	}

	internal class Liaison
	{
		public int Depart { get; set; }
		public int Arrivee { get; set; }
		public decimal Duree { get; set; }
		public decimal CoutCarbone { get; set; }
	}
}
