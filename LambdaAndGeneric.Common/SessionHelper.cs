using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace LambdaAndGeneric.Common
{
    /// <summary>
    /// Session操作助手类（简单封装版）
    /// </summary>
    public static class SessionHelper
    {
        /// <summary>
        /// Session状态对象
        /// </summary>
        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static object Get(string key)
        {
            return Session[key];
        }

        public static TResult Get<TResult>(string key) where TResult : class
        {
            return Get(key) as TResult;
        }

        public static void Set(string key, object value)
        {
            Session[key] = value;
        }
    }
}
