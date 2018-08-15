using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LambdaAndGeneric.Common
{
    public static class RequestHelper
    {
        /// <summary>
        /// 静态类静态方法 THIS 关键字(将当前类的命名空间和扩展对象类型的命名空间保持一致)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsPostBack(this HttpRequest request)
        {
            return request.HttpMethod.Equals("post", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
