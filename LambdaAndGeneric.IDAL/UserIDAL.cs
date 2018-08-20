using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LambdaAndGeneric.Model;

namespace LambdaAndGeneric.IDAL
{
    public partial interface UserIDAL : BaseIDAL<Model.User>
    {
        List<User> Test();
    }
}
