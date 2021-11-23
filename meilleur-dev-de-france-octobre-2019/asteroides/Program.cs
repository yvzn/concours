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

			AffichageDebug(image, b => b ? "X " : "  ", nbLignes.Value, nbColonnes.Value);

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			var numeroProchainAsteroide = 0;
			var detection = new int?[image.Length];

			for (int position = 0; position < image.Length; ++position)
			{
				if (!image[position])
				{
					continue;
				}

				if (detection[position].HasValue)
				{
					// déjà détecté précédemment
					continue;
				}

				MarquerAsteroide(image, detection, position, nbLignes.Value, nbColonnes.Value, numeroProchainAsteroide);
				++numeroProchainAsteroide;
			}

			AffichageDebug(detection, a => a.HasValue ? string.Format("{0,2}", a.Value) : "  ", nbLignes.Value, nbColonnes.Value);

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

		private static (int ligneCourante, int colonneCourante) PositionVersLigneColonne(int position, int _, int nbColonnes)
		{
			var ligneCourante = position / nbColonnes;
			var colonneCourante = position % nbColonnes;

			return (ligneCourante, colonneCourante);
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

		private static void MarquerAsteroide(bool[] image, int?[] detection, int position, int nbLignes, int nbColonnes, int numeroAsteroide)
		{
			detection[position] = numeroAsteroide;

			var (ligneCourante, colonneCourante) = PositionVersLigneColonne(position, nbLignes, nbColonnes);

			var positionAgauche = LigneColonneVersPosition(ligneCourante, colonneCourante - 1, nbLignes, nbColonnes);
			if (EstMarquageNecessaire(image, detection, positionAgauche))
			{
				MarquerAsteroide(image, detection, positionAgauche.Value, nbLignes, nbColonnes, numeroAsteroide);
			}

			var positionAuDessus = LigneColonneVersPosition(ligneCourante - 1, colonneCourante, nbLignes, nbColonnes);
			if (EstMarquageNecessaire(image, detection, positionAuDessus))
			{
				MarquerAsteroide(image, detection, positionAuDessus.Value, nbLignes, nbColonnes, numeroAsteroide);
			}

			var positionAdroite = LigneColonneVersPosition(ligneCourante, colonneCourante + 1, nbLignes, nbColonnes);
			if (EstMarquageNecessaire(image, detection, positionAdroite))
			{
				MarquerAsteroide(image, detection, positionAdroite.Value, nbLignes, nbColonnes, numeroAsteroide);
			}

			var positionEnDessous = LigneColonneVersPosition(ligneCourante + 1, colonneCourante, nbLignes, nbColonnes);
			if (EstMarquageNecessaire(image, detection, positionEnDessous))
			{
				MarquerAsteroide(image, detection, positionEnDessous.Value, nbLignes, nbColonnes, numeroAsteroide);
			}
		}

		private static bool EstMarquageNecessaire(bool[] image, int?[] detection, int? position)
		{
			return position.HasValue && image[position.Value] && !detection[position.Value].HasValue;
		}
	}
}
