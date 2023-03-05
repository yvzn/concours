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
		static int? nombreParcelles = default;

		static IList<Parcelle> parcelles = new List<Parcelle>();

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nombreParcelles.HasValue)
				{
					nombreParcelles = Lire(line).First();
					continue;
				}

				var ligne = Lire(line).ToList();
				parcelles.Add(new Parcelle
				{
					x = ligne[0],
					y = ligne[1],
					w = ligne[2],
					h = ligne[3],
				});
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var nombreVoisinsParcelle = parcelles.Select(CalculNombreVoisins).ToList();

			var maximumVoisins = nombreVoisinsParcelle.Max();
			var parcellesMaximumVoisins = parcelles
				.Select((parcelle, i) => nombreVoisinsParcelle[i] == maximumVoisins ? i + 1 : default(int?))
				.Where(i => i.HasValue)
				.ToList();

			Console.WriteLine(parcellesMaximumVoisins.Count + " " + maximumVoisins);
			Console.WriteLine(string.Join(" ", parcellesMaximumVoisins));
		}

		private static int CalculNombreVoisins(Parcelle parcelle)
		{
			return parcelles.Where(autreParcelle => autreParcelle != parcelle && SontVoisines(parcelle, autreParcelle)).Count();
		}

		private static bool SontVoisines(Parcelle parcelle1, Parcelle parcelle2)
		{
			var voisinageVertical = Insersection(parcelle1.x, parcelle1.x + parcelle1.w, parcelle2.x, parcelle2.x + parcelle2.w)
				&& (parcelle1.y + parcelle1.h == parcelle2.y || parcelle1.y == parcelle2.y + parcelle2.h);
			var voisinageHorizontal = Insersection(parcelle1.y, parcelle1.y + parcelle1.h, parcelle2.y, parcelle2.y + parcelle2.h)
				&& (parcelle1.x + parcelle1.w == parcelle2.x || parcelle1.x == parcelle2.x + parcelle2.w);
			return voisinageVertical || voisinageHorizontal;
		}

		private static bool Insersection(int l1, int r1, int l2, int r2)
		{
			// https://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
			return l1 < r2 && l2 < r1;
		}

		private static IEnumerable<int> Lire(string ligne)
		{
			var split = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return split.Select(int.Parse);
		}
	}

	internal class Parcelle
	{
		public int x { get; set; }
		public int y { get; set; }
		public int w { get; set; }
		public int h { get; set; }
	}
}
