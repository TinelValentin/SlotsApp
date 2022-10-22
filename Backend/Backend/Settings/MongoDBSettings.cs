using Backend.Interfaces;

namespace Backend.Settings
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string SlotsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get ; set; }
    }
}
