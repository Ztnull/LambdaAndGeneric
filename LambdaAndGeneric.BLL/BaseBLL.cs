using System.Collections.Generic;
using LambdaAndGeneric.DAL;
using LambdaAndGeneric.IBLL;
using LambdaAndGeneric.IDAL;

namespace LambdaAndGeneric.BLL
{
    /// <summary>
    /// 说明： BaseBLL 的封装
    /// 时间：2018年8月15日11:11:31
    /// 作用：用于被继承，减少代码
    /// 作者：null
    /// </summary>
    /// <typeparam name="T">约束对象</typeparam>
    public class BaseBLL<T> : BaseIBLL<T> where T : new()
    {
        private BaseIDAL<T> _dao = FactoryDAL.FactoryDAL.GetHelper<T>();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="Entity">传入的增加的实体Model</param>
        /// <returns></returns>
        public bool ADD(T Entity)
        {
            return _dao.ADD(Entity);
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="ID">删除ID</param>
        /// <returns></returns>
        public bool Del(int ID)
        {
            return _dao.Del(ID);
        }

        /// <summary>
        ///根据实体更新一条数据
        /// </summary>
        /// <param name="Entity">传入更新的实体Model</param>
        /// <returns></returns>
        public bool Update(T Entity)
        {
            return _dao.Update(Entity);
        }

        /// <summary>
        /// 获得单个实体
        /// </summary>
        /// <param name="ID">查询ID</param>
        /// <returns></returns>
        public T GetEntity(int ID)
        {
            return _dao.GetEntity(ID);
        }

        /// <summary> 
        ///  根据传入的条件执行查询，并返回一个IEnumerable<T>类型的集合
        ///  （注意传入的 T 必须约束为 where T : class, new()）
        /// </summary>
        /// <param name="where">查询条件，请省略 where 关键字</param>
        /// <returns></returns>
        public IEnumerable<T> GetEntityList(string where)
        {
            return _dao.GetEntityList(where);
        }

        /// <summary>
        /// 根据传入的SQL语句执行查询，并返回一个IEnumable<T>类型的集合
        /// 注意 T 必须约束为 where W : class, new()）
        /// </summary>
        /// <typeparam name="W">返回的单个Model实体</typeparam>
        /// <param name="sql">执行的SQL语句</param>
        /// <returns></returns>
        public IEnumerable<W> GetEntityListSQl<W>(string sql)
        {
            return _dao.GetEntityListSQl<W>(sql);
        }
    }
}
