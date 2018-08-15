using System.Collections.Generic;

namespace LambdaAndGeneric.IBLL
{
    /// <summary>
    /// 说明： BaseIBLL 的封装
    /// 时间：2018年8月15日11:11:31
    /// 作用：用于被继承，减少代码
    /// 作者：null
    /// </summary>
    /// <typeparam name="T">约束对象</typeparam>
    public interface BaseIBLL<T> where T: new()
    {
        /// <summary>
        /// 执行一个插入操作
        /// </summary>
        /// <typeparam name="W"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool ADD(T Entity);
        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool Del(int ID);
        /// <summary>
        /// 根据Model更新一条数据
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool Update(T Entity);
        /// <summary>
        /// 获得单个实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        T GetEntity(int ID);

        /// <summary>
        /// 根据条件查询,并返回一个IEnumerable<T>类型的集合
        ///  （注意传入的 T 必须约束为 where T : class, new()）
        /// </summary>
        /// <param name="where"> 参数省略【where】关键字</param>
        /// <returns></returns>
        IEnumerable<T> GetEntityList(string where); 

        /// <summary>
        ///  执行传入的Sql语句，并返回一个IEnumerable<T>类型的集合
        ///  （注意传入的 T 必须约束为 where T : class, new()）
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <returns></returns>
        IEnumerable<W> GetEntityListSQl<W>(string sql);
    }
}
