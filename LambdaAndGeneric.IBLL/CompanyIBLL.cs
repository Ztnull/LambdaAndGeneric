using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.IBLL
{
    public partial interface CompanyIBLL<T> : BaseIBLL<T> where T : new()
    {
        IEnumerable<Model.Company> GetList();
    }
}
