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
			var basket = new List<Item>();
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!header)
				{
					var data = line.Split(' ');

					basket.Add(new Item
					{
						Quantite = int.Parse(data[0]),
						PrixUnitaire = int.Parse(data[1]),
						Poids = int.Parse(data[2]),
					});
				}
				header = false;
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var prixArticles = basket.Select(item => item.Quantite * item.PrixUnitaire).Sum();
			var fraisPort = 495 + basket.Select(item => item.Quantite * item.Poids * 125 / 1000m).Sum();

			var prixPanier = Math.Floor(prixArticles + fraisPort);

			Console.WriteLine(prixPanier);
		}
	}

	class Item
	{
		public int Quantite { get; set; }
		public int PrixUnitaire { get; set; }
		public int Poids { get; set; }
	}
}
