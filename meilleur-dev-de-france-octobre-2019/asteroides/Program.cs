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
		static void Main(string[] args)
		{
			int? nbLignes = default;
			int? nbColonnes = default;

			bool[] image = default;
			int ligneCourante = 0;
			int colonneCourante;

			string line;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!nbLignes.HasValue)
				{
					(nbLignes, nbColonnes) = Parse(line);
					image = new bool[nbLignes.Value * nbColonnes.Value];
					continue;
				}

				for (colonneCourante = 0; colonneCourante < line.Length && colonneCourante < nbColonnes.Value; ++colonneCourante)
				{
					if (line[colonneCourante] == 'X')
					{
						var position = LigneColonneVersPosition(ligneCourante, colonneCourante, nbLignes.Value, nbColonnes.Value);
						image[position.Value] = true;
					}
				}

				++ligneCourante;
			}

			AffichageDebug(image, b => b ? "X" : " ", nbLignes.Value, nbColonnes.Value);

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var numeroProchainAsteroide = 0;
			var detection = new int?[image.Length];

			for (ligneCourante = 0; ligneCourante < nbLignes; ++ligneCourante)
			{
				for (colonneCourante = 0; colonneCourante < nbColonnes; ++colonneCourante)
				{
					var position = LigneColonneVersPosition(ligneCourante, colonneCourante, nbLignes.Value, nbColonnes.Value);
					if (!position.HasValue)
					{
						continue;
					}

					if (!image[position.Value])
					{
						continue;
					}

					var positionAgauche = LigneColonneVersPosition(ligneCourante, colonneCourante - 1, nbLignes.Value, nbColonnes.Value);
					if (positionAgauche.HasValue && detection[positionAgauche.Value].HasValue)
					{
						var numeroAsteroideAgauche = detection[positionAgauche.Value].Value;
						detection[position.Value] = numeroAsteroideAgauche;
						continue;
					}

					var positionAuDessus = LigneColonneVersPosition(ligneCourante - 1, colonneCourante, nbLignes.Value, nbColonnes.Value);
					if (positionAuDessus.HasValue && detection[positionAuDessus.Value].HasValue)
					{
						var numeroAsteroideAuDessus = detection[positionAuDessus.Value].Value;
						detection[position.Value] = numeroAsteroideAuDessus;
						continue;
					}

					detection[position.Value] = numeroProchainAsteroide;
					++numeroProchainAsteroide;
				}
			}

			AffichageDebug(detection, i => i.HasValue ? i.Value.ToString() : " ", nbLignes.Value, nbColonnes.Value);

			var nbAsteroides = numeroProchainAsteroide;

			for(int asteroideCourant = 0; asteroideCourant < nbAsteroides; ++asteroideCourant)
			{
				var asteroidesConnexes = AsteroidesConnexes(detection, asteroideCourant, nbLignes.Value, nbColonnes.Value).ToHashSet();
				foreach(var asteroideConnexe in asteroidesConnexes)
				{
					FusionnerAsteroides(detection, asteroideCourant, asteroideConnexe);
				}
			}

			AffichageDebug(detection, i => i.HasValue ? i.Value.ToString() : " ", nbLignes.Value, nbColonnes.Value);

			Console.WriteLine(detection.Where(i => i.HasValue).Distinct().Count());
		}

		private static (int, int) Parse(string line)
		{
			var split = line.Split(' ');
			return (int.Parse(split[0]), int.Parse(split[1]));
		}

		private static int? LigneColonneVersPosition(int ligneCourante, int colonneCourante, int nbLignes, int nbColonnes)
		{
			if (ligneCourante < 0 || ligneCourante >= nbLignes || colonneCourante < 0 || colonneCourante >= nbColonnes)
			{
				return default;
			}

			return ligneCourante * nbColonnes + colonneCourante;
		}

		private static void AffichageDebug<T>(T[] tableau, Func<T, string> formater, int _, int nbColonnes)
		{
#if DEBUG
			var sb = new StringBuilder();
			for (int position = 0; position < tableau.Length; ++position)
			{
				if (position % nbColonnes == 0 && sb.Length > 0)
				{
					Console.Error.WriteLine(sb.ToString());
					sb.Clear();
				}
				sb.Append(formater.Invoke(tableau[position]));
			}
			if (sb.Length > 0)
			{
				Console.Error.WriteLine(sb.ToString());
			}
#endif
		}

		private static IEnumerable<int> AsteroidesConnexes(int?[] detection, int asteroideCourant, int nbLignes, int nbColonnes)
		{
			for (var ligneCourante = 0; ligneCourante < nbLignes; ++ligneCourante)
			{
				for (var colonneCourante = 0; colonneCourante < nbColonnes; ++colonneCourante)
				{
					var position = LigneColonneVersPosition(ligneCourante, colonneCourante, nbLignes, nbColonnes);
					if (!position.HasValue)
					{
						continue;
					}

					if (!detection[position.Value].HasValue || detection[position.Value].Value != asteroideCourant)
					{
						continue;
					}

					var positionAgauche = LigneColonneVersPosition(ligneCourante, colonneCourante - 1, nbLignes, nbColonnes);
					if (positionAgauche.HasValue && detection[positionAgauche.Value].HasValue)
					{
						yield return detection[positionAgauche.Value].Value;
					}

					var positionAuDessus = LigneColonneVersPosition(ligneCourante - 1, colonneCourante, nbLignes, nbColonnes);
					if (positionAuDessus.HasValue && detection[positionAuDessus.Value].HasValue)
					{
						yield return detection[positionAuDessus.Value].Value;
					}

					var positionAdroite = LigneColonneVersPosition(ligneCourante, colonneCourante + 1, nbLignes, nbColonnes);
					if (positionAdroite.HasValue && detection[positionAdroite.Value].HasValue)
					{
						yield return detection[positionAdroite.Value].Value;
					}

					var positionEnDessous = LigneColonneVersPosition(ligneCourante + 1, colonneCourante, nbLignes, nbColonnes);
					if (positionEnDessous.HasValue && detection[positionEnDessous.Value].HasValue)
					{
						yield return detection[positionEnDessous.Value].Value;
					}
				}
			}
		}

		private static void FusionnerAsteroides(int?[] detection, int asteroideCourant, int asteroideConnexe)
		{
			if (asteroideCourant == asteroideConnexe)
			{
				return;
			}

			for(var position = 0; position < detection.Length; position++)
			{
				if (detection[position].HasValue && detection[position].Value == asteroideConnexe)
				{
					detection[position] = asteroideCourant;
				}
			}
		}
	}
}
