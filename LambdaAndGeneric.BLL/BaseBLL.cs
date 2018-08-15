using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LambdaAndGeneric.IDAL;
using LambdaAndGeneric.DAL;
using LambdaAndGeneric.IBLL;

namespace LambdaAndGeneric.BLL
{
    public class BaseBLL<T> : BaseIBLL<T> where T : new()
    {
        private BaseIDAL<T> dao = new BaseDAL<T>();
        public bool ADD(T Entity)
        {
            return dao.ADD(Entity);
        }
        public bool Del(int ID)
        {
            return dao.Del(ID);
        }
        public bool Update(T Entity)
        {
            return dao.Update(Entity);
        }
        public T GetEntity(int ID)
        {
            return dao.GetEntity(ID);
        }
        public IEnumerable<T> GetEntityList(string where)
        {
            return dao.GetEntityList(where);
        }
        public IEnumerable<W> GetEntityListSQl<W>(string sql)
        {
            return dao.GetEntityListSQl<W>(sql);
        }
    }
}
