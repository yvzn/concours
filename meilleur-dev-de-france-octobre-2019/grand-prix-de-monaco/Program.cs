using System;

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
			int? piloteFavori = default;
			int? nbEvenements = default;

			var classement = new int[20];
			for (int i = 0; i < classement.Length; i++)
			{
				var identifiantPilote = i + 1;
				classement[i] = identifiantPilote;
			}

			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!piloteFavori.HasValue)
				{
					piloteFavori = int.Parse(line);
					continue;
				}

				if (!nbEvenements.HasValue)
				{
					nbEvenements = int.Parse(line);
					continue;
				}

				var (piloteConcerne, evenement) = Parse(line);
				var positionPiloteConcerne = TrouverPositionPilote(classement, piloteConcerne);
				if (evenement == "D")
				{
					if (positionPiloteConcerne.HasValue && positionPiloteConcerne.Value > 0)
					{
						var piloteDepasse = classement[positionPiloteConcerne.Value - 1];
						classement[positionPiloteConcerne.Value - 1] = piloteConcerne;
						classement[positionPiloteConcerne.Value] = piloteDepasse;
					}
				}
				else if (evenement == "I")
				{
					if (positionPiloteConcerne.HasValue)
					{
						for (var positionPiloteSuivant = positionPiloteConcerne + 1; positionPiloteSuivant < classement.Length; ++positionPiloteSuivant)
						{
							classement[positionPiloteSuivant.Value - 1] = classement[positionPiloteSuivant.Value];
						}
						classement[classement.Length - 1] = -1;
					}
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			if (piloteFavori.HasValue)
			{
				var positionPiloteFavori = TrouverPositionPilote(classement, piloteFavori.Value);
				if (positionPiloteFavori.HasValue)
				{
					Console.WriteLine(positionPiloteFavori.Value + 1);
				}
				else
				{
					Console.WriteLine("KO");
				}
			}
		}

		private static int? TrouverPositionPilote(int[] classement, int piloteConcerne)
		{
			for (int i = 0; i < classement.Length; i++)
			{
				if (classement[i] == piloteConcerne)
				{
					return i;
				}
			}
			return default;
		}

		private static (int, string) Parse(string line)
		{
			var split = line.Split(' ');
			return (int.Parse(split[0]), split[1]);
		}
	}
}
