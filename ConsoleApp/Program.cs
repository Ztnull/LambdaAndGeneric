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
                CompanyIBLL<Company> company = new CompanyBLL();
                var xx = company.GetEntity(1);


                var result = company.Update(new Company() { CreateTime = System.DateTime.Now, CreatorId = 2, ID = 9, LastModifierId = 2, LastModifyTime = System.DateTime.Now, Name = "测试数据", });
                Console.WriteLine(result);

                {
                    Console.WriteLine("*****************************GetEntityListSQl***********************************");
                    var list = company.GetEntityListSQl<Company>("  SELECT  * from company ");
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.ID + "***" + item.LastModifierId + "**" + item.LastModifyTime + "***" + item.Name);
                    }
                }
                Console.Write("\n");
                {
                    Console.WriteLine("*****************************GetEntityList***********************************");
                    var listbll = company.GetEntityList("id>0");
                    foreach (var item in listbll)
                    {
                        Console.WriteLine(item.ID + "***" + item.LastModifierId + "**" + item.LastModifyTime + "***" + item.Name);
                    }
                }

                Console.Write("\n");
                {
                    Console.WriteLine("*****************************Extend***********************************");
                    var listbll = company.GetList();
                    foreach (var item in listbll)
                    {
                        Console.WriteLine(item.ID + "***" + item.LastModifierId + "**" + item.LastModifyTime + "***" + item.Name);
                    }
                }
                Console.ReadKey();
            }
        }
    }
}
