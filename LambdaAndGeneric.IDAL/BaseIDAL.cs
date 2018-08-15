using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.IDAL
{
    public interface BaseIDAL<T> where T:new()
    {
        bool ADD(T Entity);
        bool Del(int ID);
        bool Update(T Entity);
        T GetEntity(int ID);
        IEnumerable<T> GetEntityList(string where);
        IEnumerable<T> GetEntityListSQl(string sql);
    }
}
