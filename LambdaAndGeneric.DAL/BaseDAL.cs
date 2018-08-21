using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using LambdaAndGeneric.IDAL;
using LamdbaAndGeneric.HelperDAL;

namespace LambdaAndGeneric.DAL
{
    /// <summary>
    /// 说明： BaseDAL 的封装
    /// 时间：2018年8月15日
    /// 作用：用于被继承，减少代码
    /// 作者：null
    /// </summary>
    /// <typeparam name="T">约束对象</typeparam>
    public class BaseDAL<T> : BaseIDAL<T> where T : new()
    {

        #region 根据Model更新一条数据

        /// <summary>
        /// 根据Model更新一条数据
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool Update(T Entity)
        {
            string sqlText = "UPDATE  [{0}] SET {1} where {2}";

            Type type = typeof(T);
            object oT = Activator.CreateInstance(Entity.GetType());


            #region old
            //var oEntity = Entity.GetType().GetProperties();//
            //StringBuilder builder = new StringBuilder(256);
            //StringBuilder builderWhere = new StringBuilder(256);

            //foreach (var item in oEntity)
            //{
            //    //字段和值得拼接
            //    if (!item.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) && !item.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        builder.Append(item.Name + "=" + item.GetValue(oT));
            //    }
            //    //获取ID条件
            //    if (item.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || item.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        builderWhere.Clear();
            //        builderWhere.Append(item.Name + "=" + item.GetValue(oT));
            //    }
            //} 

            //string execSql = $" update {type.Name} {builder.ToString()} where {builderWhere.ToString()}";
            #endregion

            //
            string columString = string.Join(",", type.GetProperties().Where(item => !item.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) && !item.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase)).Select(s => string.Format("{0}=@{1}", s.Name, s.Name)));

            var where = type.GetProperties().Where(p => p.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || p.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase)).Select(s => string.Format("[{0}]={1}", s.Name, s.GetValue(Entity))).FirstOrDefault();

            sqlText = string.Format(sqlText, type.Name, columString, where);

            SqlParameter[] sqlParameters = type.GetProperties().Where(item => !item.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) && !item.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase)).Select(s => new SqlParameter(string.Format("@{0}", s.Name), s.GetValue(Entity) ?? DBNull.Value)).ToArray();


            return SenctionHelper.Excute<bool>(sqlText, t =>
               {
                   return t.ExecuteNonQuery() > 0;
               }, sqlParameters);

            // return true;//SenctionHelper.ExecuteNonQuery(sqlText, sqlParameters) > 0;//old
        }

        #endregion

        #region 根据ID删除一条数据

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Del(int ID)
        {
            Type type = typeof(T);

            string where = type.GetProperties().Where(w => w.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || w.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase)).Select(p => string.Format("[{0}]=@{1}", p.Name, p.Name)).FirstOrDefault();

            SqlParameter[] sqlParameters = type.GetProperties().Where(w => w.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || w.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase)).Select(p => new SqlParameter(string.Format("@{0}", p.Name), ID)).ToArray();

            string execSql = $" DELETE FROM [{type.Name}] where {where} ";

            return SenctionHelper.Excute<bool>(execSql, t =>
             {
                 return t.ExecuteNonQuery() > 0;
             }, sqlParameters);

            // return SenctionHelper.ExecuteNonQuery(execSql) > 0;//old
        }

        #endregion

        #region 插入一条数据
        /// <summary>
        /// 执行一个插入操作
        /// </summary>
        /// <typeparam name="W"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool ADD(T Entity)
        {

            string sqlText = "Insert into [{0}] ({1}) values({2}) ";

            var type = typeof(T);
            object oT = Activator.CreateInstance(type);
            //PropertyInfo[] propertys = Entity.GetType().GetProperties();

            //获取字段
            var columString = string.Join(",", type.GetProperties().Where(w => !w.Name.Equals("ID", StringComparison.CurrentCultureIgnoreCase) && !w.Name.Equals("FID", StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Name));

            //参数化
            var valuePars = string.Join(",", type.GetProperties().Where(w => !w.Name.Equals("ID", StringComparison.CurrentCultureIgnoreCase) && !w.Name.Equals("FID", StringComparison.CurrentCultureIgnoreCase)).Select(s => string.Format("@{0}", s.Name)));

            SqlParameter[] sqlParameters = type.GetProperties().Where(w => !w.Name.Equals("ID", StringComparison.CurrentCultureIgnoreCase) && !w.Name.Equals("FID", StringComparison.CurrentCultureIgnoreCase)).Select(p => new SqlParameter(string.Format("@{0}", p.Name), p.GetValue(Entity) ?? DBNull.Value)).ToArray();

            sqlText = string.Format(sqlText, type.Name, columString, valuePars);

            return SenctionHelper.Excute<bool>(sqlText, s =>
            {
                return s.ExecuteNonQuery() > 0;
            }, sqlParameters);
            //return SenctionHelper.ExecuteNonQuery(sqlText, sqlParameters) > 0;
        }

        #endregion

        #region 获得单个实体
        /// <summary>
        /// 获得单个实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetEntity(int ID)
        {
            Type type = typeof(T);
            string where = type.GetProperties().Where(w => w.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || w.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase)).Select(p => string.Format("[{0}]=@{1}", p.Name, p.Name)).FirstOrDefault();

            SqlParameter[] sqlParameters = type.GetProperties().Where(w => w.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || w.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase)).Select(p => new SqlParameter(string.Format("@{0}", p.Name), ID)).ToArray();

            string sql = $" SELECT {SenctionHelper.GetModelSenction<T>()} from [{type.Name}] where {where} ";

            return SenctionHelper.Excute<T>(sql, s =>
             {
                 using (SqlDataReader reder = s.ExecuteReader())
                 {
                     if (reder.HasRows)
                     {
                         if (reder.Read())
                         {
                             return SenctionHelper.MapEntity<T>(reder);
                         }
                         else
                         {
                             return default(T);
                         }
                     }
                     else
                     {
                         return default(T);
                     }
                 }
             }, sqlParameters);

            #region old
            //try
            //{
            //    using (SqlDataReader reader = SenctionHelper.ExecuteReader(sql))
            //    {
            //        if (reader.HasRows)
            //        {
            //            reader.Read();
            //            return SenctionHelper.MapEntity<T>(reader);
            //        }
            //        else
            //        {
            //            return default(T);
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return default(T);
            //} 
            #endregion

        }
        #endregion

        #region  根据传入的条件执行Sql语句，并返回一个IEnumerable<T>类型的集合

        /// <summary>
        ///  根据传入的SQL语句执行查询，并返回一个IEnumable<T>类型的集合
        ///  注意 W 必须约束为 where W : class, new()）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<W> GetEntityListSQl<W>(string sql)
        { 
            IList<W> list = new List<W>();
            return SenctionHelper.Excute<IList<W>>(sql, s =>
                 {
                     using (SqlDataReader reader = s.ExecuteReader())
                     {
                         if (reader.HasRows)
                         {
                             while (reader.Read())
                             {

                                 list.Add(SenctionHelper.MapEntity<W>(reader));
                             }
                         }
                         else
                         {
                             return default(List<W>);
                         }
                     }
                     return list;
                 });

        }

        #endregion

        #region  根据传入的条件执行Sql语句，并返回一个IEnumerable<T>类型的集合

        /// <summary>
        /// 根据传入的条件执行查询，并返回一个IEnumerable<T>类型的集合
        /// （注意传入的 T 必须约束为 where T : class, new()）
        /// </summary>
        /// <typeparam name="T">类型：【 约束为 where T : class, new() 】</typeparam>
        /// <param name="where">查询的条件，请省略 Where 关键字</param>
        /// <returns></returns>
        public IEnumerable<T> GetEntityList(string where)
        {
            Type type = typeof(T);
            //遍历获得字段 

            string sql = string.Format("SELECT {0} FROM [{1}] WHere {2} ",
               SenctionHelper.GetModelSenction<T>(),
                type.Name,
                where);


            IList<T> list = new List<T>();

            return SenctionHelper.Excute<IList<T>>(sql, s =>
            {
                using (SqlDataReader reader = s.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            list.Add(SenctionHelper.MapEntity<T>(reader));
                        }
                    }
                    else
                    {
                        return default(List<T>);
                    }
                }
                return list;
            });
        }

        #endregion
    }
}
