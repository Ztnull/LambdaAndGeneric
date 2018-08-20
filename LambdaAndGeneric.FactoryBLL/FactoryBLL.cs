using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ReadConfigSetting;

namespace LambdaAndGeneric.FactoryBLL
{
    /// <summary>
    ///Service工厂
    /// </summary>
    public class FactoryBLL
    {
        public static T CreateService<T>(string DllDalName, string DllDalTypeName) where T : new()
        {
            Assembly assembly = Assembly.Load(DllDalName);
            Type dbHelperType = assembly.GetType(DllDalTypeName);
            //Type markType = dbHelperType.MakeGenericType(typeof(T));//创建泛型
            object oHelper = Activator.CreateInstance(dbHelperType);
            return (T)oHelper;
        }
    }
}
