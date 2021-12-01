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
		static Sequence? sequence1;
		static Sequence? sequence2;
		static Sequence? maSequence;

		static void Main(string[] args)
		{
			string ligne;
			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (!sequence1.HasValue)
				{
					sequence1 = Lire(ligne);
					continue;
				}
				if (!sequence2.HasValue)
				{
					sequence2 = Lire(ligne);
					continue;
				}
				if (!maSequence.HasValue)
				{
					maSequence = Lire(ligne);
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données
			foreach (var encodage in GenererEncodage(sequence1.Value.chiffres, sequence1.Value.code))
			{
				if (EncoderAvec(encodage, sequence2.Value.chiffres) == sequence2.Value.code)
				{
					Console.WriteLine(EncoderAvec(encodage, maSequence.Value.chiffres));
				}
			}
		}

		private static string EncoderAvec(IEnumerable<Encodage> encodage, string chiffres)
		{
			var sb = new StringBuilder();
			foreach (var c in chiffres)
			{
				var decode = encodage.Where(e => e.chiffre == c).Select(e => e.code).FirstOrDefault();
				if (decode != null)
				{
					sb.Append(decode);
				}
			}
			return sb.ToString();
		}

		private static IEnumerable<IEnumerable<Encodage>> GenererEncodage(string chiffres, string code)
		{
			if (chiffres.Length >= 1)
			{
				if (code.Length >= 3)
				{
					var chiffreCourant = chiffres[0];
					var code3 = code.Substring(0, 3);
					var continuer = false;

					foreach (var encodage in GenererEncodage(chiffres.Substring(1), code.Substring(3)))
					{
						var resultat3 = new List<Encodage>();
						resultat3.Add(new Encodage { chiffre = chiffreCourant, code = code3 });
						resultat3.AddRange(encodage);

						continuer = true;
						yield return resultat3;
					}

					if (!continuer)
					{
						var resultat3 = new List<Encodage>();
						resultat3.Add(new Encodage { chiffre = chiffreCourant, code = code3 });
						yield return resultat3;
					}
				}

				if (code.Length >= 4)
				{
					var chiffreCourant = chiffres[0];
					var code4 = code.Substring(0, 4);
					var continuer = false;

					foreach (var encodage in GenererEncodage(chiffres.Substring(1), code.Substring(4)))
					{
						var resultat4 = new List<Encodage>();
						resultat4.Add(new Encodage { chiffre = chiffreCourant, code = code4 });
						resultat4.AddRange(encodage);

						continuer = true;
						yield return resultat4;
					}

					if (!continuer)
					{
						var resultat4 = new List<Encodage>();
						resultat4.Add(new Encodage { chiffre = chiffreCourant, code = code4 });
						yield return resultat4;
					}
				}
			}
		}



		private static Sequence Lire(string ligne)
		{
			var split = ligne.Split(' ');
			return new Sequence
			{
				chiffres = split[0],
				code = split.Length > 1 ? split[1] : default,
			};
		}
	}

	internal struct Encodage
	{
		public char chiffre;
		public string code;
	}

	internal struct Sequence
	{
		public string chiffres;
		public string code;
	}
}
