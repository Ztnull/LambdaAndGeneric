using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 配置
/// </summary>
namespace ReadConfigSetting
{
    /// <summary>
    /// 提供全局的文件配置
    /// 将Config读取的配置都写到这里
    /// 多数用于反射创建对象
    /// </summary>
    public class Constant
    {


        #region BLL 工厂配置

        //Company
        public static string DbCompany = ConfigurationManager.AppSettings["Company"];
        public static string CompanyDllName = DbCompany.Split(',')[0];
        public static string CompanyTypeName = DbCompany.Split(',')[1];

        //User
        public static string DbUser = ConfigurationManager.AppSettings["User"];
        public static string UserDllName = DbUser.Split(',')[0];
        public static string UserTypeName = DbUser.Split(',')[1];

        #endregion

        #region DAL 配置

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionStringCustomers = ConfigurationManager.ConnectionStrings["Customers"].ConnectionString;

        /// <summary>
        /// 数据库工厂配置
        /// </summary>
        public static string DbConfig = ConfigurationManager.AppSettings["DbConfig"];
        public static string DllName = DbConfig.Split(',')[0];
        public static string TypeName = DbConfig.Split(',')[1];

        #endregion


        #region Other 其他配置

        #endregion


    }
}
