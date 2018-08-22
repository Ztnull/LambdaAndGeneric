using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LambdaAndGeneric.Common
{
    /// <summary>
    /// Thread扩展
    /// </summary>
    public class ThreadExtend
    {
        /// <summary>
        /// 执行多线程，并返回一个结果
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="func">方法</param>
        /// <returns></returns>
        public static Func<T> ThreadResult<T>(Func<T> func)
        {
            T t = default(T);
            ThreadStart threadStart = () =>
            {
                t = func.Invoke();
            };
            Thread thread = new Thread(threadStart);
            thread.Start();
            return () =>
            {
                while (thread.ThreadState != System.Threading.ThreadState.Stopped)
                {
                    Thread.Sleep(100);
                }
                return t;
            };
        }
    }
}
