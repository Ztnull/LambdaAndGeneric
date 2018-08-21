using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<int> vs = new List<int>() { 1, 2, 3, 4, 5, 6, 6, 7, 87, 1, 2, 3, 4, 5, 6, 6, 7, 871, 2, 3, 4, 5, 6, 6, 7, 871, 2, 3, 4, 5, 6, 6, 7, 87,123,123,12,334,334,355634523,4,3241,23,12,31,41 };
                var result = TWhere<int>(vs, f => f > 6);
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 筛选
        /// </summary>
        /// <typeparam name="TSources">数据源</typeparam>
        /// <param name="Source">数据</param>
        /// <param name="func">执行的委托</param>
        /// <returns></returns>
        public static IEnumerable<TSources> TWhere<TSources>(IEnumerable<TSources> Source, Func<TSources, bool> func)
        {
            foreach (var item in Source)
            {
                if (func.Invoke(item))
                {
                    yield return item;
                }
            }
        }
    }
}
