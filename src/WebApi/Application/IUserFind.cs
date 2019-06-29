namespace WebApi.Application
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserFind<T>
    {
        IEnumerable<T> GetAll();
        Task<T> GetByUsername(string username);
        Task<T> GetById(string id);
    }
}