namespace Backend.Interfaces
{
    public interface IMongoDBSettings
    {
        string SlotsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

    }
}
