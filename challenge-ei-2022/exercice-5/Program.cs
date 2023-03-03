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
		static ICollection<Liaison> liaisons = new List<Liaison>();
		static IDictionary<int, IEnumerable<IEnumerable<int>>> cacheTrajets = new Dictionary<int, IEnumerable<IEnumerable<int>>>();

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
					continue;
				}

				if (ligne.Length >= 4)
				{
					liaisons.Add(new Liaison
					{
						Depart = ligne[0],
						Arrivee = ligne[1],
						Duree = ligne[2],
						CoutCarbone = ligne[3],
					});
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
				// var (duree, cout) = Estimer(trajet);
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

			var departs = liaisons.Select(l => l.Depart).ToHashSet();

			IList<Trajet> trajetsAtraiter;
			do
			{
				trajetsAtraiter = trajets.Where(t => departs.Contains(t.etapes.Last()) && t.etapes.Last() != numeroEtapeFinale).ToList();

				foreach (var trajet in trajetsAtraiter)
				{
					var suitesPossibles = liaisons.Where(l => l.Depart == trajet.etapes.Last()).ToList();
					if (!suitesPossibles.Any())
					{
						continue;
					}

					trajets.Remove(trajet);
					foreach (var suite in suitesPossibles)
					{
						var nouveauTrajet = new Trajet
						{
							etapes = trajet.etapes.Append(suite.Arrivee).ToList(),
							duree = trajet.duree + suite.Duree,
							coutCarbone = trajet.coutCarbone + suite.CoutCarbone,
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


		private static IEnumerable<IEnumerable<int>> GenererTrajets__()
		{
			return GenererTrajets(0);
		}

		private static IEnumerable<IEnumerable<int>> GenererTrajets(int origine)
		{
			if (cacheTrajets.TryGetValue(origine, out var cache))
			{
				return cache;
			}

			var trajets = liaisons
				.Where(l => l.Depart == origine)
				.Select(l => l.Arrivee)
				.SelectMany(GenererTrajets)
				.ToList();

			if (trajets.Count == 0 && origine == numeroEtapeFinale)
			{
				return new[] { new[] { origine } };
			}

			var trajetsAvecOrigine = trajets.Select(trajet =>
			{
				var result = new List<int>() { origine };
				result.AddRange(trajet);
				return result;
			});
			cacheTrajets[origine] = trajetsAvecOrigine.ToList();
			return trajetsAvecOrigine;
		}

		private static (decimal duree, decimal cout) Estimer(IEnumerable<int> trajet)
		{
			var duree = 0m;
			var cout = 0m;
			var etapePrecedente = -1;
			var etapeCourante = -1;
			var enumerator = trajet.GetEnumerator();
			while (enumerator.MoveNext())
			{
				etapeCourante = enumerator.Current;
				if (etapePrecedente < 0)
				{
					etapePrecedente = etapeCourante;
					continue;
				}

				var liaison = liaisons
					.Where(l => l.Depart == etapePrecedente && l.Arrivee == etapeCourante)
					.FirstOrDefault();
				if (liaison is null)
				{
					return (duree, cout);
				}

				duree += liaison.Duree;
				cout += liaison.CoutCarbone;
				etapePrecedente = etapeCourante;
			}
			return (duree, cout);
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
