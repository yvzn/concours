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
		static int? nombrePoints;
		static IDictionary<int, Point> points;

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				var ligne = Lire(line).ToArray();

				if (!nombrePoints.HasValue)
				{
					nombrePoints = ligne[0];
					InitialiserPoints();
					continue;
				}

				if (ligne.Length == 6)
				{
					var numero = ligne[0];
					var point = points[numero];
					point.QuantiteEau = ligne[1];
					point.FilsGauche = points[ligne[2]];
					point.CoefGauche = ligne[3];
					point.FilsDroit = points[ligne[4]];
					point.CoefDroit = ligne[5];
					continue;
				}

				if (ligne.Length == 2)
				{
					var numero = ligne[0];
					var point = points[numero];
					point.QuantiteEau = ligne[1];
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var racine = points[0];
			var perteMaximale = int.MinValue;
			foreach (var point in points.Values)
			{
				if (point.FilsGauche != null)
				{
					var quantiteGauche = CalculerQuantiteTotale(point.FilsGauche);
					var perteGauche = point.CoefGauche * quantiteGauche;
					if (perteGauche > perteMaximale)
					{
						perteMaximale = perteGauche;
					}
				}
				if (point.FilsDroit != null)
				{
					var quantiteDroit = CalculerQuantiteTotale(point.FilsDroit);
					var perteDroit = point.CoefDroit * quantiteDroit;
					if (perteDroit > perteMaximale)
					{
						perteMaximale = perteDroit;
					}
				}
			}
			Console.WriteLine(perteMaximale);
		}

		private static int CalculerQuantiteTotale(Point point)
		{
			var quantiteTotale = point.QuantiteEau;
			if (point.FilsGauche != null)
			{
				quantiteTotale += CalculerQuantiteTotale(point.FilsGauche);
			}
			if (point.FilsDroit != null)
			{
				quantiteTotale += CalculerQuantiteTotale(point.FilsDroit);
			}
			return quantiteTotale;
		}

		private static void InitialiserPoints()
		{
			if (!nombrePoints.HasValue)
			{
				return;
			}

			points = Enumerable
				.Range(0, nombrePoints.Value + 1)
				.ToDictionary(i => i, i => new Point { Numero = i });
		}

		private static IEnumerable<int> Lire(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(int.Parse);
		}
	}

	class Point
	{
		public int Numero { get; set; }
		public int QuantiteEau { get; set; }
		public Point FilsGauche { get; set; }
		public int CoefGauche { get; set; }
		public Point FilsDroit { get; set; }
		public int CoefDroit { get; set; }
		public int QuantiteMax { get; set; }
	}
}
