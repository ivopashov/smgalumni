using Mongo.Data.Entities;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Data
{
    public class MongoContext
    {
        private readonly MongoRepository<User> _users;

        public MongoContext(MongoRepository<User> users)
        {
            _users = users;
        }

        public MongoRepository<User> Users { get { return _users; } }
    }
}
