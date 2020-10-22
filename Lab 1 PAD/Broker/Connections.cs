using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broker
{
    static class Connections
    {
        private static List<ConnInfo> connInfos;
        private static object locker;

        static Connections()
        {
            connInfos = new List<ConnInfo>();
            locker = new object();
        }

    public static void Add(ConnInfo connInfo)
        {
            lock (locker)
            {
                connInfos.Add(connInfo);
            }
        }

    public static void Remove(string address)
        {
            lock (locker)
            {
                connInfos.RemoveAll(c => c.Address == address);
            }
        }

    public static List<ConnInfo> GetConnInfosByCategory(string category)
        {
            List<ConnInfo> connectionsBytopic;

            lock (locker)
            {
                connectionsBytopic = connInfos.Where(c => c.newsCategory == category).ToList();
            }
            return connectionsBytopic;
        }
    }
}
