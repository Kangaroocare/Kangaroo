using System;
using System.Collections.Generic;

namespace Kangaroo.Helpers
{
    public class Log
    {

        /// <summary>
        /// report the exception to xamarin insights dashboard.
        /// </summary>
        /// <param name="ex">exception</param>
        public static void Report(Exception ex)
        {
            // Insights.Report(ex);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        /// <summary>
        /// report the exception with custom values and this overload is important to add descriptive data
        /// </summary>
        /// <param name="ex">exception</param>
        /// <param name="info">Information</param>
        public static void Report(Exception ex, Dictionary<string, string> info)
        {
            // Insights.Report(ex, info);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

    }
}
