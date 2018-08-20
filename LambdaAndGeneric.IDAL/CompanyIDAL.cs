using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.IDAL
{
    public partial interface CompanyIDAL : BaseIDAL<Model.Company>
    {
        IEnumerable<Model.Company> GetList();
    }
}
