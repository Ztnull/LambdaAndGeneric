using LambdaAndGeneric.DAL;
using LambdaAndGeneric.IBLL;
using LambdaAndGeneric.IDAL;
using LambdaAndGeneric.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadConfigSetting;

namespace LambdaAndGeneric.BLL
{
    public partial class CompanyBLL : BaseBLL<Company>, CompanyIBLL<Company>
    {
        /// <summary>
        /// 通过反射创建数据访问
        /// </summary>
        private CompanyIDAL _dao = FactoryBLL.FactoryBLL.CreateService<CompanyDAL>(Constant.CompanyDllName, Constant.CompanyTypeName);
        public IEnumerable<Company> GetList()
        {
            return _dao.GetList();
        }
    }
}
