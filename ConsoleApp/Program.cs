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
                //var xx = company.GetEntity(1);


                foreach (var item in company.GetEntityList("id>0"))
                {
                    Console.WriteLine(item.ID + "***" + item.LastModifierId + "**" + item.LastModifyTime + "***" + item.Name);
                }
                Console.ReadKey();

                 

                //var result = company.Update(new Company() { CreateTime = System.DateTime.Now, CreatorId = 2, ID = 9, LastModifierId = 2, LastModifyTime = System.DateTime.Now, Name = "测试数据", });
                //Console.WriteLine(result);

                //{
                //    Console.WriteLine("*****************************GetEntityListSQl***********************************");
                //    var list = company.GetEntityListSQl<Company>("  SELECT  * from company ");
                //    foreach (var item in list)
                //    {
                //        Console.WriteLine(item.ID + "***" + item.LastModifierId + "**" + item.LastModifyTime + "***" + item.Name);
                //    }
                //}


                //Console.Write("\n");
                //{
                //    Console.WriteLine("*****************************GetEntityList***********************************");
                //    var listbll = company.GetEntityList("id>0");


                //    Console.WriteLine(string.Join("  ", typeof(Company).GetProperties().Select(c => c.Name)));

                //    foreach (var item in listbll)
                //    {
                //        foreach (var i in item.GetType().GetProperties())
                //        {
                //            Console.Write(i.GetValue(item) + "  ");
                //        }
                //        Console.WriteLine();
                //    }
                //}

                //Console.Write("\n");
                //{

                //    Console.WriteLine("*****************************Del***********************************");
                //    Console.WriteLine(company.Del(1));

                //}



                //Console.Write("\n");
                //{
                //    Console.WriteLine("*****************************Extend***********************************");
                //    var listbll = company.GetList();
                //    foreach (var item in listbll)
                //    {
                //        Console.WriteLine(item.ID + "***" + item.LastModifierId + "**" + item.LastModifyTime + "***" + item.Name);
                //    }
                //}

            }



            {//user
                Console.Write("\n");
                UserBLL userBLL = new UserBLL();
                {
                    Console.WriteLine("*****************************User Update***********************************");
                    var state = userBLL.Update(new User() { Account = "测试AccountUpdate", CompanyId = 1, CreateTime = System.DateTime.Now, CreatorId = 2, Emaile = "123.@qq.com", ID = 10, LastLoginTime = System.DateTime.Now, LastModifierId = 2, LastModifierTime = System.DateTime.Now, Name = "测试Name Update", Password = "123321", State = 1, UserType = 2 });
                    Console.WriteLine(state);
                }



                {
                    Console.Write("\n");
                    Console.WriteLine("*****************************User GetEntityListSQl***********************************");
                    var state = userBLL.GetEntityListSQl<User>("Select * from [User]");

                    Console.WriteLine(string.Join("  ", typeof(User).GetProperties().Select(c => c.Name)));

                    foreach (var item in state)
                    {
                        foreach (var i in item.GetType().GetProperties())
                        {
                            Console.Write(i.GetValue(item) + "  ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine(state);
                }



                {
                    Console.Write("\n");
                    Console.WriteLine("*****************************User GetEntity***********************************");
                    var state = userBLL.GetEntity(6);
                    Console.WriteLine(typeof(User).GetProperties().Select(u => string.Format("{0}     ", u.Name)).FirstOrDefault());
                    foreach (var item in state.GetType().GetProperties())
                    {
                        foreach (var i in item.GetType().GetProperties())
                        {
                            Console.WriteLine(i.GetValue(item) + "        ");
                        }
                    }
                    Console.WriteLine(state);
                }

                {
                    Console.Write("\n");
                    Console.WriteLine("*****************************User List***********************************");
                    var list = userBLL.GetEntityList("id>0");
                    Console.WriteLine(typeof(User).GetProperties().Select(u => string.Format("{0}    ", u.Name)).FirstOrDefault());
                    foreach (var item in list)
                    {
                        foreach (var model in item.GetType().GetProperties())
                        {
                            Console.Write(model.GetValue(item) + "    ");
                        }
                        Console.WriteLine();
                    }
                }



                {
                    Console.Write("\n");
                    Console.WriteLine("*****************************泛型命令封装 Excute<T>***********************************");

                    var list = userBLL.Test();
                    Console.WriteLine(typeof(User).GetProperties().Select(u => string.Format("{0}    ", u.Name)).FirstOrDefault());
                    foreach (var item in list)
                    {
                        foreach (var model in item.GetType().GetProperties())
                        {
                            Console.Write(model.GetValue(item) + "    ");
                        }
                        Console.WriteLine();
                    }

                }


            }

            Console.ReadKey();
        }
    }
}
