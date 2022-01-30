using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Core.Helpers
{
    public static class CoreHelpers
    {
        /// <summary>
        /// A function that allows us to capture if any of the entered inputs contain one of the words used for SQL injection.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool SqlInjectionChecker(string input)
        {
            bool isSQLInjection = false;

            string[] sqlCheckList = { " or ", "1=1", "=", " and ", "--", ";--", ";", "/*", "*/", "@@", "@", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update" };
            string CheckString = input.Replace("'", "''");

            for (int i = 0; i <= sqlCheckList.Length - 1; i++)
            {
                if ((CheckString.IndexOf(sqlCheckList[i], StringComparison.OrdinalIgnoreCase) >= 0))
                    isSQLInjection = true;
            }

            return isSQLInjection;
        }

        /// <summary>
        /// Calculate registered share's total price
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="pricePerShare"></param>
        /// <returns></returns>
        public static double GetTotalSharePrice(double amount, double pricePerShare)
        {
            return Math.Round((Double)(amount * pricePerShare),2);
        }


    }
}
