using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace SiccoApp.DAL
{
    public class SiccoAppConfiguration : DbConfiguration
    {
        public SiccoAppConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());

            ////https://msdn.microsoft.com/en-us/data/dn456835
            ////https://msdn.microsoft.com/en-us/data/jj680699

            ////maximum number of retries to 1 and the maximum delay to 30 seconds
            //SetExecutionStrategy(
            //    "System.Data.SqlClient",
            //    () => new SqlAzureExecutionStrategy(1, TimeSpan.FromSeconds(30)));
        }
    }
}
