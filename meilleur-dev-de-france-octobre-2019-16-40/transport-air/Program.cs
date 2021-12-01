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
			Participant moi = default;
			Participant ami = default;

			string ligne;
			while ((ligne = Console.ReadLine()) != null)
			{
				//
				// Lisez les données et effectuez votre traitement */
				//
				if (moi == null)
				{
					moi = Lire(ligne);
					continue;
				}

				if (ami == null)
				{
					ami = Lire(ligne);
					continue;
				}
			}

			// Vous pouvez aussi effectuer votre traitement ici après avoir lu toutes les données 
			moi.position = moi.gareDepart;
			ami.position = ami.gareDepart;

			while (moi.position != ami.position)
			{
				while (moi.position < ami.position)
				{
					moi.position = (moi.position + moi.ligneEmpruntee) % 37;
				}
				while (ami.position < moi.position)
				{
					ami.position = (ami.position + ami.ligneEmpruntee) % 37;
				}
			}

			Console.WriteLine(moi.position);
		}

		private static Participant Lire(string ligne)
		{
			var split = ligne.Split(' ');
			return new Participant
			{
				gareDepart = int.Parse(split[0]),
				ligneEmpruntee = int.Parse(split[1]),
			};
		}
	}

	class Participant
	{
		public int gareDepart;
		public int ligneEmpruntee;
		public int position;
	}
}
