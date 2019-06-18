namespace WebApi.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Domain.DTO;

    public class UserFind : UserService, IUserFind<UserFindDto>
    {
        public UserFind(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
        }

        public IEnumerable<UserFindDto> GetAll()
        {
            var users = _userRepository.GetAll();
            return _mapper.Map<IEnumerable<UserFindDto>>(users);
        }

        public async Task<UserFindDto> GetByUsername(string username)
        {
            var user = (await _userRepository.Get(x => x.Username.Equals(username))).FirstOrDefault();
            return _mapper.Map<UserFindDto>(user);
        }

        public async Task<UserFindDto> GetById(string id)
        {
            var user = (await _userRepository.Get(x => x.Id == id)).FirstOrDefault();
            return _mapper.Map<UserFindDto>(user);
        }
    }
}