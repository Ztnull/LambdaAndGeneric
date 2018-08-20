using LambdaAndGeneric.IBLL;
using LambdaAndGeneric.IDAL;
using LambdaAndGeneric.Model;
using ReadConfigSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.BLL
{
    public partial class UserBLL : BaseBLL<User>, UserIBLL<User>
    {
        /// <summary>
        /// 通过反射创建数据访问
        /// </summary>
        private UserIDAL _dao = FactoryBLL.FactoryBLL.CreateService(Constant.UserDllName, Constant.UserTypeName) as UserIDAL;
        public List<User> Test()
        {
            return _dao.Test();
        }
    }
}
