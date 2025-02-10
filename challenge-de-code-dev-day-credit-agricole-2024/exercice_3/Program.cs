using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
		static int? W, H;
		static List<string> etats = new List<string>();

		static void Main(string[] args)
		{
			string line;
			string[] reseau = default;
			int h = 0;
			while ((line = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!W.HasValue)
				{
					(W, H) = Lire1(line);
					reseau = new string[H.Value];
					continue;
				}

				reseau[h] = line;
				h++;
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			int? debutCycle = default, dureeCycle = default;

			var etatCourant = reseau;
			etats.Add(Serialiser(etatCourant));
			for (var i = 0; i < 1000; i++)
			{
				string[] etatSuivant = Transition(etatCourant);
				etats.Add(Serialiser(etatSuivant));
				etatCourant = etatSuivant;

				(debutCycle, dureeCycle) = DetecterBoucle();
				if (debutCycle.HasValue)
				{
					break;
				}
			}

			Console.WriteLine(debutCycle.Value);
			Console.WriteLine(dureeCycle.Value);
		}

		private static readonly Regex re = new Regex(@"(.+=)\1+", RegexOptions.Compiled);
		private static (int? debutCycle, int? dureeCycle) DetecterBoucle()
		{
			var tousLesEtats = string.Join("=", etats) + "=";
			var longueurEtat = W.Value * H.Value + 1;
			foreach (Match match in re.Matches(tousLesEtats))
			{
				if (match.Groups[1].Length % longueurEtat == 0)
					return (match.Index / longueurEtat, match.Groups[1].Length / longueurEtat);
			}
			return (default, default);
		}

		private static string[] Transition(string[] etatCourant)
		{
			var resultat = new string[H.Value];
			for (var ligne = 0; ligne < H.Value; ++ligne)
			{
				var c = new char[W.Value];
				for (var colonne = 0; colonne < W.Value; ++colonne)
				{
					c[colonne] = Transition(etatCourant, ligne, colonne);
				}
				resultat[ligne] = string.Concat(c);
			}
			return resultat;
		}

		private static char Transition(string[] etatCourant, int ligne, int colonne)
		{
			var etatCellule = etatCourant[ligne][colonne];
			switch (etatCellule)
			{
				case '5': return '4';
				case '4': return '3';
				case '3': return '2';
				case '2': return '1';
				case '1':
					var voisins = Voisins(etatCourant, ligne, colonne).ToList();
					if (voisins.Contains('5')) return '5';
					if (voisins.Count(v => v == '1') > 1) return '1';
					return '3';
			}
			throw new InvalidOperationException();
		}

		private static IEnumerable<char> Voisins(string[] etatCourant, int ligne, int colonne)
		{
			var possibles = new (int, int)[]
			{
				(ligne -1, colonne), // haut
				(ligne, colonne +1), // droite
				(ligne +1, colonne), // bas
				(ligne, colonne -1), // gauche
			};

			foreach (var (i, j) in possibles)
			{
				if (i < 0 || i >= H.Value) continue;
				if (j < 0 || j >= W.Value) continue;
				yield return etatCourant[i][j];
			}
		}

		private static string Serialiser(string[] etatCourant)
		{
			return string.Join("", etatCourant);
		}

		private static (int? W, int? H) Lire1(string line)
		{
			var s = line.Split(' ');
			return (int.Parse(s[0]), int.Parse(s[1]));
		}
	}
}
