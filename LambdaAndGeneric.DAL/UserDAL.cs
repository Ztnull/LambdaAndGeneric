using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LambdaAndGeneric.IDAL;
using LambdaAndGeneric.Model;
using LamdbaAndGeneric.HelperDAL;
using ReadConfigSetting;

namespace LambdaAndGeneric.DAL
{
    public partial class UserDAL : BaseDAL<Model.User>, UserIDAL
    {
        public List<User> Test()
        {
            return SenctionHelper.Excute<List<User>>("Select * From [User]", u =>
               {
                   List<User> userList = new List<User>();

                   using (SqlDataReader reader = u.ExecuteReader())
                   {
                       if (reader.HasRows)
                       {
                           while (reader.Read())
                           { 
                               userList.Add(SenctionHelper.MapEntity<User>(reader));
                           }
                       }
                       else
                       {
                           return default(List<User>);
                       }
                   }

                   return userList;
               });
        }
    }
}
