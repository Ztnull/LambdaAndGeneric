using LambdaAndGeneric.IBLL;
using LambdaAndGeneric.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.BLL
{
    public partial class CompanyBLL : BaseBLL<Company>, CompanyIBLL<Company>
    {
    }
}
