using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Helpers
    {
        public static int ToInt( this string text )
        {
            return int.Parse(text);
        }

        /// <summary>
        /// Supports retrying a block of code a number of times
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncOperation"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public static async Task<T> OperationWithBasicRetryAsync<T>(Func<Task<T>> asyncOperation, int retryCount)
        {
            int currentRetry = 0;

            while (true)
            {
                try
                {
                    return await asyncOperation();
                }
                catch 
                {
                    currentRetry++;

                    if (currentRetry > retryCount)
                    {
                        // If this is not a transient error or we should not retry re-throw the exception. 
                        throw;
                    }
                }

                // Wait to retry the operation.  
                await Task.Delay(100 * currentRetry);
            }
        }

        /// <summary>
        /// Gets a verbose string representing an error message
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="includeStack"></param>
        /// <returns></returns>
        public static string GetText(this Exception ex, bool includeStack)
        {
            StringBuilder b = new StringBuilder();
            while (ex != null)
            {
                b.Append(ex.Message);
                if (includeStack)
                    b.Append(ex.StackTrace);
                ex = ex.InnerException;
            }
            return b.ToString();
        }
    }
}
