using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Data
{
    public interface IUserRepository : IRepository<User>
    {
        void SetOnline(string id);
        void SetOffline(string id);
    }
}
