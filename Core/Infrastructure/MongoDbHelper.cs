using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Core.Infrastructure
{
    public static class MongoDbHelper
    {
        public static MongoDatabase GetDb(string connectionString, string dataBase)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(dataBase);
            return database;
        }
    }
}
