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

                var result = company.ADD(new Company() { CreateTime = System.DateTime.Now, CreatorId = 2, ID = 2, LastModifierId = 2, LastModifyTime = System.DateTime.Now, Name = "xxx" });

                var list=   company.GetEntityListSQl<Company>("  SELECT  * from company ");


                foreach (var item in list)
                {
                    Console.WriteLine(item.ID+ "***" + item.LastModifierId+"**"+item.LastModifyTime+"***" +item.Name);
                }

                Console.WriteLine(result);
                Console.ReadKey();
            }
        }
    }
}
