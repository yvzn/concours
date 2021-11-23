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
		static int? porteeSaut = default;
		static int? nbNenuphars = default;
		static readonly List<Nenuphar> nenuphars = new List<Nenuphar>();
		static readonly GenerateurId generateurId = new GenerateurId();

		static void Main(string[] args)
		{
			Coordonnees? pointDepart = default;

			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nbNenuphars.HasValue)
				{
					(nbNenuphars, porteeSaut) = Lire(line);
					continue;
				}

				if (!pointDepart.HasValue)
				{
					pointDepart = LireCoordonnees(line);
					continue;
				}

				nenuphars.Add(LireNenuphar(line));
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 

			var plusLongParcours = ChercherPlusLongParcours(pointDepart.Value, new List<int>());

			Console.Error.WriteLine(string.Join(" -> ", plusLongParcours));

			Console.WriteLine(plusLongParcours.Count);
		}

		private static List<int> ChercherPlusLongParcours(Coordonnees pointDepart, List<int> dejaVisites)
		{
			var plusLongParcours = dejaVisites;

			foreach (var nouveauNenuphar in nenuphars.Where(n => !dejaVisites.Contains(n.id)).Where(n => PeutSauterVers(pointDepart, n.coordonnees)))
			{
				var parcoursAvecNouveauNenuphar = new List<int>(dejaVisites)
				{
					nouveauNenuphar.id
				};

				var plusLongParcoursDepuisNouveauNenuphar = ChercherPlusLongParcours(nouveauNenuphar.coordonnees, parcoursAvecNouveauNenuphar);
				if (plusLongParcoursDepuisNouveauNenuphar.Count > plusLongParcours.Count)
				{
					plusLongParcours = plusLongParcoursDepuisNouveauNenuphar;
				}
			}

			return plusLongParcours;
		}

		private static bool PeutSauterVers(Coordonnees source, Coordonnees destination)
		{
			if (!porteeSaut.HasValue) return false;

			// compare les carrés pour éviter de calculer la racine carrée
			return AuCarre(destination.x - source.x) + AuCarre(destination.y - source.y) <= AuCarre(porteeSaut.Value);
		}

		private static int AuCarre(int x)
		{
			// evite d'utiliser Math.Pow et de convertir en double
			return x * x;
		}

		private static Nenuphar LireNenuphar(string ligne)
		{
			return new Nenuphar
			{
				id = generateurId.Next(),
				coordonnees = LireCoordonnees(ligne),
			};
		}

		private static Coordonnees LireCoordonnees(string ligne)
		{
			var (x, y) = Lire(ligne);
			return new Coordonnees { x = x, y = y };
		}

		private static (int, int) Lire(string ligne)
		{
			var split = ligne.Split(' ');
			return (int.Parse(split[0]), int.Parse(split[1]));
		}
	}

	struct Nenuphar
	{
		public int id;
		public Coordonnees coordonnees;
	}

	struct Coordonnees
	{
		public int x, y;
	}

	internal class GenerateurId
	{
		int prochainId = 0;
		public int Next()
		{
			return prochainId++;
		}
	}
}
