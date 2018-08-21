using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.Common
{
    public class ModelHelper
    {


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
            catch
            {
                return default(T);
            }
        }
        #endregion


        #region 实体辅助帮助类 ：DataTable转Model    + static IList<T> DtToModel<T>(DataTable dt) where T : class, new()
        /// <summary>
        /// 实体辅助帮助类 ：DataTable转Model
        /// </summary>
        /// <param name="dt">转换的DataTable，数据源</param>
        /// <returns></returns>
        public static IList<T> DtToModel<T>(DataTable dt) where T : class, new()
        {
            if (dt.Rows.Count < 1) return null;

            //定义集合
            IList<T> ts = new List<T>();
            //获得此模型的类型
            Type type = typeof(T);
            string tempName = string.Empty;
            //循环遍历数据源
            foreach (DataRow dr in dt.Rows)
            {
                //实例化传入对象
                T t = new T();
                //获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    tempName = pi.Name;
                    //检查DataTable是否包含此列

                    if (dt.Columns.Contains(tempName))
                    {
                        try
                        {
                            //判断此列是否有【保护字】
                            if (!pi.CanWrite) continue;

                            var value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                //pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType, CultureInfo.CurrentCulture), null);
                                pi.SetValue(t, value, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                }
                ts.Add(t);
            }
            return ts;
        } 
        #endregion


    }
}
