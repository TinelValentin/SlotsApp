namespace Backend.Interfaces
{
    public interface IUserService<T>
    {
        List<T> GetAll();
    }
}
