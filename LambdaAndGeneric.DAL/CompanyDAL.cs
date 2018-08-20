using LambdaAndGeneric.IDAL;
using LambdaAndGeneric.Model;
using LamdbaAndGeneric.HelperDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaAndGeneric.DAL
{
    public partial class CompanyDAL : BaseDAL<Model.Company>, CompanyIDAL
    {
        public IEnumerable<Company> GetList()
        {
            string sql = "select * from Company ";
            using (SqlDataReader reader = SenctionHelper.ExecuteReader(sql))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SenctionHelper.MapEntity<Company>(reader);
                    }
                }

            }
        }
    }
}
