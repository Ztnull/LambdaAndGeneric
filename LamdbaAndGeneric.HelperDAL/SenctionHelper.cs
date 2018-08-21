using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadConfigSetting;

namespace LamdbaAndGeneric.HelperDAL
{
    /// <summary>
    /// 简单封装，针对于数据库的操作，旧的封装SQl见Common层的SqlHelper
    /// </summary>
    public static class SenctionHelper
    {


        #region +Old 公用 +More


        #region 获取映射模型的字段
        /// <summary>
        /// 获得当前对象的所有公共字段
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public static string GetModelSenction<T>()
        {
            Type type = typeof(T);
            return string.Join(",", type.GetProperties().Select(p => string.Format("[{0}]", p.Name)));
        }
        #endregion

        #region ExecuteNonQuery +static int ExecuteNonQuery(string cmdText, params SqlParameter[] parameters)
        /// <summary>
        /// 执行一个非查询的T-SQL语句，返回受影响行数，如果执行的是非增、删、改操作，返回-1
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <exception cref="链接数据库异常"></exception>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, parameters);
        }
        #endregion

        #region ExecuteNonQuery +static int ExecuteNonQuery(string cmdText, CommandType type, params SqlParameter[] parameters)
        /// <summary>
        /// 执行一个非查询的T-SQL语句，返回受影响行数，如果执行的是非增、删、改操作，返回-1
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="type">命令类型</param>
        /// <param name="parameters">参数列表</param>
        /// <exception cref="链接数据库异常"></exception>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, CommandType type, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(Constant.ConnectionStringCustomers))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.CommandType = type;
                    try
                    {
                        conn.Open();
                        int res = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return res;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
        }
        #endregion

        #region ExecuteReader +static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] parameters)
        /// <summary>
        /// 执行一个查询的T-SQL语句, 返回一个SqlDataReader对象, 如果出现SQL语句执行错误, 将会关闭连接通道抛出异常
        ///  ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <exception cref="链接数据库异常"></exception>
        /// <returns>SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] parameters)
        {
            return ExecuteReader(cmdText, CommandType.Text, parameters);
        }
        #endregion

        #region ExecuteReader +static SqlDataReader ExecuteReader(string cmdText, CommandType type, params SqlParameter[] parameters)
        /// <summary>
        /// 执行一个查询的T-SQL语句, 返回一个SqlDataReader对象, 如果出现SQL语句执行错误, 将会关闭连接通道抛出异常
        ///  ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="type">命令类型</param>
        /// <param name="parameters">参数列表</param>
        /// <exception cref="链接数据库异常"></exception>
        /// <returns>SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText, CommandType type, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(Constant.ConnectionStringCustomers);
            using (SqlCommand cmd = new SqlCommand(cmdText, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(parameters);
                }
                cmd.CommandType = type;
                conn.Open();
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return reader;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    //出现异常关闭连接并且释放
                    conn.Close();
                    throw ex;
                }
            }
        }
        #endregion

        #region 将一个SqlDataReader对象转换成一个实体类对象 +static T MapEntity<T>(SqlDataReader reader) where T : class,new()
        /// <summary>
        /// 将一个SqlDataReader对象转换成一个实体类对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="reader">当前指向的reader</param>
        /// <returns>实体对象</returns>
        public static T MapEntity<T>(SqlDataReader reader)
        {
            try
            {
                Type type = typeof(T);
                var props = type.GetProperties();

                object entity = Activator.CreateInstance(type);//创建返回的单个对象
                foreach (var prop in props)
                {
                    if (prop.CanWrite)
                    {
                        try
                        {
                            var index = reader.GetOrdinal(prop.Name);
                            var data = reader.GetValue(index);
                            if (data != DBNull.Value)
                            {
                                prop.SetValue(entity, Convert.ChangeType(data, prop.PropertyType), null);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
                return (T)entity;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        #endregion

        #endregion 


        #region +New 抽象出所有的数据操作。+ static T Excute<T>(string sql, Func<SqlCommand, T> func, params SqlParameter[] parameters)

        /// <summary>
        ///抽象出所有的数据操作。
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">执行的语句</param>
        /// <param name="func">执行的方法</param>
        /// <returns></returns>
        public static T Excute<T>(string sql, Func<SqlCommand, T> func, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(Constant.ConnectionStringCustomers))
            {
                conn.Open();
                //SqlTransaction trans = conn.BeginTransaction(); ///开启事务
                try
                {
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddRange(parameters);
                        }
                        T t = func(command);
                        return t;
                    }
                }
                catch (Exception ex)
                {
                    //trans.Rollback();
                    throw ex;
                }

            }
        }

        #endregion


    }
}
