using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LambdaAndGeneric.IBLL;
using ReadConfigSetting;

namespace LambdaAndGeneric.FactoryBLL
{
    /// <summary>
    ///Service工厂
    /// </summary>
    public class FactoryBLL
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="DllDalName"></param>
        /// <param name="DllDalTypeName"></param>
        /// <returns></returns>
        public static T CreateService<T>(string DllDalName, string DllDalTypeName) 
        {
            Assembly assembly = Assembly.Load(DllDalName);
            Type dbHelperType = assembly.GetType(DllDalTypeName);
            object oHelper = Activator.CreateInstance(dbHelperType);
            return (T)oHelper;
        }
    }
}
