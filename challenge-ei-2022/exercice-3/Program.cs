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
		static string parcelle1 = default;
		static string parcelle2 = default;
		static int? somme = default;
		static char[] tousLesChiffres = "123456789".ToArray();

		static void Main(string[] args)
		{
			string ligne;
			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (parcelle1 is null)
				{
					parcelle1 = ligne;
					continue;
				}
				if (parcelle2 is null)
				{
					parcelle2 = ligne;
					continue;
				}
				if (!somme.HasValue)
				{
					somme = int.Parse(ligne);
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			var plusPetitEncodage = int.MaxValue;
			var caracteres = parcelle1.Union(parcelle2).Distinct().ToArray();
			foreach (var encodage in GenererEncodage(caracteres))
			{
				var encodageParcelle1 = Encoder(parcelle1, encodage);
				var encodageParcelle2 = Encoder(parcelle2, encodage);
				if (encodageParcelle1 + encodageParcelle2 == somme && encodageParcelle1 < plusPetitEncodage)
				{
					plusPetitEncodage = encodageParcelle1;
				}
			}

			Console.WriteLine(plusPetitEncodage);
		}

		private static IEnumerable<IDictionary<char, char>> GenererEncodage(ICollection<char> caracteres)
		{
			foreach (var permutation in Permutations(tousLesChiffres, caracteres.Count))
			{
				var encodage = new Dictionary<char, char>();
				foreach (var zip in caracteres.Zip(permutation, (lettre, code) => new { lettre = lettre, code = code }))
				{
					encodage[zip.lettre] = zip.code;
				}
				yield return encodage;
			}
		}

		private static int Encoder(string parcelle, IDictionary<char, char> encodage)
		{
			var sb = new StringBuilder();
			foreach (var code in parcelle.Select(lettre => encodage[lettre]))
			{
				sb.Append(code);
			}
			return int.Parse(sb.ToString());
		}

		private static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length) where T : IComparable
		{
			if (length == 0) return Array.Empty<IEnumerable<T>>();
			if (length == 1) return list.Select(t => new T[] { t });

			return Permutations(list, length - 1)
				.SelectMany(t => list.Where(e => !t.Contains(e)),
					(t1, t2) => t1.Concat(new T[] { t2 }));
		}

		private static bool NextCombination(IList<int> num, int n, int k)
		{
			bool finished;

			var changed = finished = false;

			if (k <= 0) return false;

			for (var i = k - 1; !finished && !changed; i--)
			{
				if (num[i] < n - 1 - (k - 1) + i)
				{
					num[i]++;

					if (i < k - 1)
						for (var j = i + 1; j < k; j++)
							num[j] = num[j - 1] + 1;
					changed = true;
				}
				finished = i == 0;
			}

			return changed;
		}

		private static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> elements, int k)
		{
			var elem = elements.ToArray();
			var size = elem.Length;

			if (k > size) yield break;

			var numbers = new int[k];

			for (var i = 0; i < k; i++)
				numbers[i] = i;

			do
			{
				yield return numbers.Select(n => elem[n]);
			} while (NextCombination(numbers, size, k));
		}
	}
}
