namespace WebApi.Application
{
    using System.Threading.Tasks;

    public interface IUserDelete<T>
    {
        Task<bool> Execute(string id);
    }
}