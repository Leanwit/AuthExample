using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain;

namespace WebApi.Application
{

    public interface IFinder<T>
    {
        IEnumerable<User> GetAll();
    }
    
    public class UserFinder : IFinder<User>
    {
        private IUserRepository _userRepository;

        public UserFinder(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return this._userRepository.GetAll();
        }
    }
}