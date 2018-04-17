using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeystrokeAnalysis.Parser;

namespace KeystrokeAnalysis
{
    public class kNN
    {
        public List<UserData> getNeighbours(List<UserData> trainSet, UserData testInstance, int k, out Dictionary<UserData, long> distances)
        {
            FunctionsGenerator functions = new FunctionsGenerator();
            distances = new Dictionary<UserData, long>();
            List<UserData> neighbours = new List<UserData>();
            for (int i = 0; i < trainSet.Count; ++i)
            {
                long dist = functions.ManhattanDistance(testInstance.userVector, trainSet[i].userVector);

                // additional weights (better results)
                if (trainSet[i].ip == testInstance.ip) dist -= 1000;
                if (trainSet[i].browser == testInstance.browser) dist -= 1000;

                distances.Add(trainSet[i], dist);
            }

            distances = distances.OrderBy(x => x.Value).Take(k).ToDictionary(x => x.Key, x => x.Value);

            for (int it = 0; it < k; ++it)
            {
                neighbours.Add(distances.Keys.ElementAt(it));
            }

            return neighbours;
        }

        public Dictionary<string, long?> countThresholdPerUser(List<UserData> users)
        {
            kNN kNNCounter = new kNN();
            Dictionary<string, long?> dict = new Dictionary<string, long?>();
            var usersGrouped = users.GroupBy(x => x.userID).Select(g => new { UserID = g.Key, UsersList= g.ToList() }).OrderBy(x => int.Parse(x.UserID)).ToList();

            //// one user compere version (faster) O(n)
            for (int i = 0; i < usersGrouped.Count; ++i)
            {

                FunctionsGenerator functions = new FunctionsGenerator();
                long dist = 0;
                Random rand = new Random();
                int random = rand.Next(usersGrouped[i].UsersList.Count);
                var concateList = usersGrouped[i].UsersList.Take(random).ToList().Concat(usersGrouped[i].UsersList.Skip(random + 1)).ToList();

                for (int j = 0; j < usersGrouped[i].UsersList.Count - 1; ++j)
                {
                    dist += functions.ManhattanDistance(usersGrouped[i].UsersList[random].userVector, concateList[j].userVector);
                }

                if (usersGrouped[i].UsersList.Count == 1) dict.Add(usersGrouped[i].UserID, null);
                else dict.Add(usersGrouped[i].UserID, dist / (usersGrouped[i].UsersList.Count - 1));
            }

            // all users compere version (slower) O(n^2)
            //for (int i = 0; i < usersGrouped.Count; ++i)
            //{
            //    FunctionsGenerator functions = new FunctionsGenerator();
            //    Dictionary<UserData, long?> distances = new Dictionary<UserData, long?>();
            //    long dist = 0;

            //    for (int j = 0; j < usersGrouped[i].UsersList.Count; ++j)
            //    {
            //        for (int k = 0; k < usersGrouped[i].UsersList.Count - 1; ++k)
            //        {
            //            var concateList = usersGrouped[i].UsersList.Take(j).ToList().Concat(usersGrouped[i].UsersList.Skip(j + 1)).ToList();
            //            dist += functions.ManhattanDistance(usersGrouped[i].UsersList[j].userVector, concateList[k].userVector);
            //        }
            //    }

            //    if (usersGrouped[i].UsersList.Count == 1)
            //    {
            //        dict.Add(usersGrouped[i].UserID, null);
            //    }
            //    else dict.Add(usersGrouped[i].UserID, dist / ((usersGrouped[i].UsersList.Count - 1) * (usersGrouped[i].UsersList.Count - 1)));
            //}

            return dict;
        }

        public string checkThresholds(Dictionary<UserData, long> neigh, Dictionary<string, long?> thresholds )
        {
            string mostOften = "";
            bool flag = true;

            while (true)
            {
                flag = false;
                if (neigh.Count == 0) return null;

                var counter = neigh.GroupBy(x => x.Key.userID , r => r.Value).OrderByDescending(g => g.Count()).Select(g => new { UserID = g.Key, Count = g.Count() , Avarange = g.Average() }).ToList();
                mostOften = counter.OrderBy(x => x.Avarange).OrderByDescending(x => x.Count).Select(g => g.UserID).First();

                for (int i = 0; i < neigh.Count; ++i)
                {
                    if (thresholds[neigh.ElementAt(i).Key.userID] == null) break;

                    // possible threshold modifications
                    if (neigh.ElementAt(i).Value > thresholds[neigh.ElementAt(i).Key.userID])
                    {
                        neigh.Remove(neigh.ElementAt(i).Key);
                        i--;
                        flag = true;
                    }
                }

                if (flag == false) break;
            }

            return mostOften;
        }

    }
}
