﻿using LambdaAndGeneric.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.DAL
{
    public partial class CompanyDAL : BaseDAL<Model.Company>, CompanyIDAL
    {

    }
}