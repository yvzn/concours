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
		static string message;
		static string nbLignes;
		static List<string> texte = new List<string>();

		static void Main(string[] args)
		{
			string ligne;
			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (message == null)
				{
					message = ligne;
					continue;
				}
				if (nbLignes == null)
				{
					nbLignes = ligne;
					continue;
				}

				texte.Add(ligne);
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			int positionMessage = 0;
			foreach (var ligneTexte in texte)
			{
				foreach (var caractere in ligneTexte)
				{
					if (positionMessage < message.Length && caractere == message[positionMessage])
					{
						positionMessage++;
					}
				}
			}

			var messageTrouve = positionMessage == message.Length;
			Console.WriteLine(messageTrouve ? 1 : 0);
		}
	}
}
