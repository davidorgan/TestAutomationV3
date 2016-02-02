using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{
    public class DateVars
    {
        protected static DateTime today { get { return DateTime.Now; } }
        protected static DateTime futureDateTo { get { return DateTime.Now.AddDays(30); } }
        protected static DateTime futureDateFrom { get { return DateTime.Now.AddDays(90); } }
        protected static DateTime pastDateFrom { get { return DateTime.Now.AddDays(-90); } }
        public static string todayString { get { return today.ToString(Settings.Default.DateFormat); } }
        public static string futureDateToString { get { return futureDateTo.ToString(Settings.Default.DateFormat); } }
        public static string futureDateFromString { get { return futureDateFrom.ToString(Settings.Default.DateFormat); } }
        public static string pastDateFromString { get { return pastDateFrom.ToString(Settings.Default.DateFormat); } }
    }
}
