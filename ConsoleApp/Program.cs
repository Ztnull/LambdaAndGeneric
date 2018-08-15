using LambdaAndGeneric.BLL;
using LambdaAndGeneric.DAL;
using LambdaAndGeneric.IBLL;
using LambdaAndGeneric.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                CompanyDAL company = new CompanyDAL();
                var xx = company.GetEntity(1);
            }
        }
    }
}
