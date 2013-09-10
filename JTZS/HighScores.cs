using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace JTZS
{
    public class HighScores
    {
        private string[] names;
        private int[] kills;


        private HighScores()
        {
            names = new string[20];
            kills = new int[20];
        }

        public string[] Names
        {
            get { return names; }
            set { names = value;  }
        }

        public int[] Kills
        {
            get { return kills; } 
            set { kills = value; } 
        }


        public void Sort()
        {

            int tempKills = kills[0];
            string tempName = names[0];

            // järjestetään lista bubble sortilla
            for (int i = 1; i < 19; i++)
            {
                for (int j = i+1; j < 20; j++)
                {
                    if (kills[j] > kills[i])
                    {
                        tempKills = kills[i];
                        kills[i] = kills[j];
                        kills[j] = tempKills;

                        tempName = names[i];
                        names[i] = names[j];
                        names[j] = tempName;
                    }
                }
            }
            
        }

        /// <summary>
        /// Lisätään uusi tulos
        /// </summary>
        /// <param name="newName">pelaajan nimi</param>
        /// <param name="newKills">pelaajan tapot</param>
        public void AddScore(string newName, int newKills)
        {
            this.Sort();
            for (int i = 0; i < 20; i++)
            {
                if (newKills > this.kills[i])
                {
                    names[names.GetUpperBound(0)] = names[i];
                    kills[kills.GetUpperBound(0)] = kills[i];
                    names[i] = newName;
                    kills[i] = newKills;
                    i = 20;
                    this.Sort();
                }
            }
        }

        public void OpenAndSerialize(string filename)
        {
            Stream stream = File.Create(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(HighScores));
            serializer.Serialize(stream, this);
            stream.Close();
        }

        /// <summary>
        /// Tallennetaan highscore -tiedosto
        /// </summary>
        /// <param name="filename">tiedoston nimi</param>
        public void Save(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {  
                    File.Delete(filename);
                    OpenAndSerialize(filename);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not open file for writing.");
                }
            }
            else if (!File.Exists(filename))
            {   
                try
                {
                    OpenAndSerialize(filename);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not open file for writing.");
                }
            }
        }

        /// <summary>
        /// Vertaa onko tulos tarpeeksi hyvä highscore -listaan
        /// </summary>
        /// <param name="score">tulos jota verrataan listan viimeiseen tulokseen</param>
        /// <returns>true, jos on kelpo listalle. Muuten palauttaa false</returns>
        public bool IsHighScore(int score)
        {
            if (score > this.kills[19])
            {
                return true;
            }else
                return false;
        }

        /// <summary>
        /// Ladataan highscore -listan tulokset
        /// </summary>
        /// <param name="filename">tiedoston nimi</param>
        /// <returns>highscore-listan</returns>
        public static HighScores Load(string filename)
        {
            if (File.Exists(filename))
            {
                Stream stream = File.OpenRead(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(HighScores));
                HighScores Scores = (HighScores)serializer.Deserialize(stream);
                stream.Close();       
                return Scores;
            }
            else
            {
                //alustetaan lista, jos aikaisempaa listaa ei ole olemassa
                HighScores defaultScores = new HighScores();

                for (int i = 0; i < 20; i++)
                {
                    defaultScores.names[i] = "John Doe";
                    defaultScores.kills[i] = 0;
                }
                return defaultScores;
                
                
            }
        }



    }
}
