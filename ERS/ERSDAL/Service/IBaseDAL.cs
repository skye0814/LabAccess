using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSDAL.Service
{
    interface IBaseDAL<Entity, KeyType>
    {
        Entity Insert(Entity e);
        bool Update(Entity e);
        bool Delete(Entity e);
        Entity Get(Entity e);
        IEnumerable<Entity> GetList();
    }
}
