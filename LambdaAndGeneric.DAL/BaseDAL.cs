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
            Type type = typeof(T);
            object oT = Activator.CreateInstance(type);
            StringBuilder builder = new StringBuilder(256);
            StringBuilder builderWhere = new StringBuilder(256);

            foreach (var item in type.GetProperties())
            {
                //字段和值得拼接
                if (!item.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || !item.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase))
                {
                    builder.Append(item.Name + "=" + item.GetValue(oT));
                }
                //获取ID条件
                if (item.Name.Equals("ID", StringComparison.InvariantCultureIgnoreCase) || item.Name.Equals("FID", StringComparison.InvariantCultureIgnoreCase))
                {
                    builderWhere.Clear();
                    builderWhere.Append(item.Name + "=" + item.GetValue(oT));
                }
            }



            string execSql = $" update {type.Name} {builder.ToString()} where {builderWhere.ToString()}";

            return SenctionHelper.ExecuteNonQuery(execSql) > 0; //SenctionHelper<T>.ExecuteNonQuery(execSql) > 0;
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

            string execSql = $" delete from {type.Name} where ID={ID} ";
            return SenctionHelper.ExecuteNonQuery(execSql) > 0;
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
            Type type = typeof(T);
            object oT = Activator.CreateInstance(type);
            PropertyInfo[] propertys = Entity.GetType().GetProperties();
            StringBuilder builder = new StringBuilder();

            //获取字段
            var section = string.Join(",", type.GetProperties().Where(w => !w.Name.Equals("ID", StringComparison.CurrentCultureIgnoreCase) && !w.Name.Equals("FID", StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Name));

            //获取值
            var value = "'" + string.Join("','", propertys.Where(w => !w.Name.Equals("ID", StringComparison.CurrentCultureIgnoreCase) && !w.Name.Equals("FID", StringComparison.CurrentCultureIgnoreCase)).Select(p => p.GetValue(Entity))) + "'";

            string execSql = $" insert into {type.Name}({section}) values({value}) ";

            return SenctionHelper.ExecuteNonQuery(execSql) > 0;
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
            string sql = $" SELECT {SenctionHelper.GetModelSenction<T>()} from {type.Name} ";

            try
            {
                using (SqlDataReader reader = SenctionHelper.ExecuteReader(sql))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return SenctionHelper.MapEntity<T>(reader);
                    }
                    else
                    {
                        return default(T);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }

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
            using (SqlDataReader reader = SenctionHelper.ExecuteReader(sql))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SenctionHelper.MapEntity<W>(reader);
                    }
                }
            }
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
            //string columnString = string.Join(",", type.GetProperties().Select(p => string.Format("[{0}]", p.Name)));
            string sql = string.Format("SELECT {0} FROM [{1}] WHere {2} ",
               SenctionHelper.GetModelSenction<T>(),
                type.Name,
                where);
            using (SqlDataReader reader = SenctionHelper.ExecuteReader(sql))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SenctionHelper.MapEntity<T>(reader);
                    }
                }
            }
        }

        #endregion
    }
}
