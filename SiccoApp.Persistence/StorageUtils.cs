using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

namespace SiccoApp.Persistence
{
    static class StorageUtils
    {
        public static CloudStorageAccount StorageAccount
        {
            get
            {
                string account = Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageAccountName");
                // This enables the storage emulator when running locally using the Azure compute emulator.
                if (account == "{StorageAccountName}")
                {
                    return CloudStorageAccount.DevelopmentStorageAccount;
                }

                string key = Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageAccountAccessKey");
                string connectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", account, key);
                return CloudStorageAccount.Parse(connectionString);
            }
        }
    }
}
