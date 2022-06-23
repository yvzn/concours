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
		static int? nbColis = default;
		static readonly Dictionary<string, int> colis = new Dictionary<string, int>();

		static void Main(string[] args)
		{
			string ligne;
			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nbColis.HasValue)
				{
					nbColis = int.Parse(ligne);
					continue;
				}

				var (numeroColis, volume) = Lire(ligne);
				colis.Add(numeroColis, volume);
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 

			int meilleureMasqueRepartition = 1;
			int meilleureDifferenceVolume = int.MaxValue;

			for (int masqueRepartition = 0; masqueRepartition < (1 << nbColis); masqueRepartition++)
			{
				var differenceVolume = CaculerDifferenceVolume(masqueRepartition);
				if (differenceVolume < meilleureDifferenceVolume)
				{
					meilleureMasqueRepartition = masqueRepartition;
					meilleureDifferenceVolume = differenceVolume;
				}
			}

			var premiereExpedition = colis.Keys.Where((numeroColis, index) => (meilleureMasqueRepartition & CalculerMasqueColis(index)) != 0);
			var secondeExpedition = colis.Keys.Where((numeroColis, index) => (meilleureMasqueRepartition & CalculerMasqueColis(index)) == 0);

			Console.WriteLine(string.Join(" ", premiereExpedition));
			Console.WriteLine(string.Join(" ", secondeExpedition));

			//var volumePremiereExpedition = colis.Values.Where((volumeColis, index) => (meilleureMasqueRepartition & CalculerMasqueColis(index)) != 0).Sum();
			//var volumeSecondeExpedition = colis.Values.Where((volumeColis, index) => (meilleureMasqueRepartition & CalculerMasqueColis(index)) == 0).Sum();

			//Console.Error.WriteLine(volumePremiereExpedition);
			//Console.Error.WriteLine(volumePremiereExpedition);
		}

		private static int CaculerDifferenceVolume(int masqueRepartition)
		{
			var volumePremiereExpedition = colis.Values.Where((volumeColis, index) => (masqueRepartition & CalculerMasqueColis(index)) != 0).Sum();
			var volumeSecondeExpedition = colis.Values.Where((volumeColis, index) => (masqueRepartition & CalculerMasqueColis(index)) == 0).Sum();
			return Math.Abs(volumePremiereExpedition - volumeSecondeExpedition);
		}

		private static int CalculerMasqueColis(int indexColis)
		{
			return 1 << (indexColis + 1);
		}

		private static (string, int) Lire(string ligne)
		{
			var split = ligne.Split(' ');
			return (split[0], int.Parse(split[1]));
		}
	}
}
