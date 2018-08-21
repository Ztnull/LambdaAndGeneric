using System;
using System.Reflection;
using LambdaAndGeneric.IDAL;
using ReadConfigSetting;

namespace LambdaAndGeneric.FactoryBLL
{
    /// <summary>
    /// 数据工厂【主要针对于基类】
    /// </summary>
    public class FactoryBaseBLL
    {

        /// <summary>
        /// 创建提供中间数据访问，BaseDAL
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
