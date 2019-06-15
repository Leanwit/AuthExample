namespace WebApi.Application
{
    using System.Threading.Tasks;

    public interface IUserCreate<T>
    {
        Task<T> Execute(T user);
    }
}