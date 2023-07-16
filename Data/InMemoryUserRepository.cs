using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Data
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> entities = new List<User>();

        public InMemoryUserRepository() 
        {
            User user = new User()
            {
                Id = "admin",
                Password = "admin",
                IsOnline = true,
                Score = 0,
            };

            entities.Add(user);
        }
        public void Add(User entity)
        {
            entities.Add(entity);
        }

        public void Delete(string id)
        {
            var entity = entities.FirstOrDefault(x => x.Id == id);
            if (entity != null) 
            {
                entities.Remove(entity);
            }
        }

        public User Get(string id)
        {
            return entities.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<User> GetQuery()
        {
            return entities.AsQueryable();
        }

        public ushort GetTotalCount()
        {
            return (ushort)entities.Count(x => x.IsOnline);
        }

        public void SetOffline(string id)
        {
            entities.FirstOrDefault(x => x.Id == id).IsOnline = false;
        }

        public void SetOnline(string id)
        {
            entities.FirstOrDefault(x => x.Id == id).IsOnline = true;
        }

        public void Update(User entity)
        {
            int entityIndex = entities.FindIndex(e => e.Id == entity.Id);
            entities[entityIndex] = entity;
        }
    }
}
