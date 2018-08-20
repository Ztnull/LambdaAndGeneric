using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LambdaAndGeneric.DAL;
using LambdaAndGeneric.IDAL;
using ReadConfigSetting;

namespace LambdaAndGeneric.FactoryDAL
{
    /// <summary>
    /// 数据工厂
    /// </summary>
    public class FactoryDAL
    {

        /// <summary>
        /// 创建提供中间数据访问
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static BaseIDAL<T> GetHelper<T>() where T : new()
        { 
            Assembly assembly = Assembly.Load(Constant.DllName);
            Type dbHelperType = assembly.GetType(Constant.TypeName);
            Type markType = dbHelperType.MakeGenericType(typeof(T));
            object oHelper = Activator.CreateInstance(markType);
            return oHelper as BaseIDAL<T>;
        }
    }
}
