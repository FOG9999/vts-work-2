using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Entities;
namespace Utilities
{
    public class ImplementLoggingFuntion
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void LOGFALL(Exception ex)
        {
            Log.Fatal(ex);        
            Log.Error(ex);
            Log.Warn(ex);
            Log.Info(ex);
            Log.Debug(ex);
     
        }
    }
}
