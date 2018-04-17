using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using KeystrokeAnalysis.Parser;
using System.Runtime.Serialization.Formatters.Binary;

namespace KeystrokeAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            List<UserData> userList = new List<UserData>();
            FunctionsGenerator functions = new FunctionsGenerator();
            kNN kNNCounter = new kNN();

            try
            {
                using (StreamReader sr = new StreamReader("KDS2_data.sql"))
                {
                    while (true)
                    {
                        string line = sr.ReadLine();
                        DataParser dataParser = new DataParser(line);
                        string timeStamp = dataParser.TimeStamp();
                        string userID = dataParser.UserID();
                        string keystrokes = dataParser.KeystrokesData();
                        string ip = dataParser.IP();
                        string browser = dataParser.Browser();

                        //Console.WriteLine(timeStamp);
                        //Console.WriteLine(userID);
                        //Console.WriteLine(keystrokes);
                        //Console.WriteLine(ip);
                        //Console.WriteLine(browser);

                        //KeystrokesParser keystrokeParser = new KeystrokesParser(keystrokes);
                        //List<long> list = keystrokeParser.inputToVector();

                        userList.Add(new UserData(timeStamp, userID, keystrokes, ip, browser));
                    }
                }
            }
            catch
            {
                double percent = 0;

                var rand = new Random();

                userList = userList.OrderBy(x => rand.Next()).ToList();

                // users with ID 0-30
                userList = userList.Where(x => int.Parse(x.userID) < 25).ToList();

                // check how much users
                //var list = userList.GroupBy(u => u.userID).ToList();

                Dictionary<UserData, long> distances = new Dictionary<UserData, long>();

                // users in 3test:7train
                List<UserData> testList = userList.Take(3 * (userList.Count) / 10).ToList();
                List<UserData> trainList = userList.Skip(3 * (userList.Count) / 10).ToList();

                //  users in 1test:1train
                //List<UserData> testList = userList.Take((userList.Count) / 2).ToList();
                //List<UserData> trainList = userList.Skip((userList.Count) / 2).ToList();


                Dictionary<string, long?> dict =  kNNCounter.countThresholdPerUser(trainList);

                int notIdentified = 0;
                for (int i = 0; i < testList.Count; ++i)
                {
                    List<UserData> neigh = kNNCounter.getNeighbours(trainList, testList[i], 5, out distances);
                    string mostOften = kNNCounter.checkThresholds(distances, dict);

                    //string mostOften = neigh[0].userID;
                    //string mostOften = neigh.GroupBy(x => x.userID).OrderByDescending(g => g.Count()).Select(g => g.Key).First();

                    if (mostOften == null)
                    {
                        notIdentified++;
                        continue;
                    }
                    if (mostOften == testList[i].userID) percent++;
                }

                percent = percent / (testList.Count - notIdentified);
                Console.WriteLine(percent);

            }
        }
    }
}
