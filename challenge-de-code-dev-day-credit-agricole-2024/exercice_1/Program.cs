using System;
using System.Collections.Generic;
using System.Globalization;
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
		static int? N = default;
		static List<Modele> modeles = new List<Modele>();

		static void Main(string[] args)
		{
			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!N.HasValue)
				{
					N = Lire1(line);
					continue;
				}

				var modele = Lire2(line);
				modeles.Add(modele);
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var bestModele = modeles
				.Select(m => (m.Nom, m.Scores.Average()))
				.OrderByDescending(m => m.Item2)
				.First();
			Console.WriteLine(bestModele.Nom);
		}

		private static Modele Lire2(string line)
		{
			var champs = line.Split(' ');
			return new Modele
			{
				Nom = champs[0],
				Scores = champs.Skip(1).Select(ParseDecimal).ToList()
			};
		}

		private static decimal ParseDecimal(string c)
		{
			return decimal.Parse(c, CultureInfo.InvariantCulture);
		}

		private static int Lire1(string line)
		{
			return int.Parse(line);
		}
	}

	internal class Modele
	{
		public string Nom { get; internal set; }
		public List<decimal> Scores { get; internal set; }
	}
}
