using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Services;
using Coms.Domain.Entities;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ErrorOr<IList<UserResult>>> GetUsers()
        {
            try
            {
                IList<User> users = new List<User>();
                users = _userRepository.GetUsers().Result;
                var results = new List<UserResult>();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        var result = new UserResult()
                        {
                            Id = user.Id,
                            FullName = user.FullName,
                            Username = user.Username,
                            Dob = user.Dob,
                            Email = user.Email,
                            Image = user.Image,
                            Password = user.Password,
                            Status = user.Status,
                            RoleId = user.RoleId,
                            Role = user.Role.RoleName
                        };
                        results.Add(result);
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                return Error.NotFound("Users not found!");
            }
        }

        public async Task<ErrorOr<IList<UserResult>>> GetManagers()
        {
            try
            {
                IList<User> users = new List<User>();
                users = _userRepository.GetManagers().Result;
                var results = new List<UserResult>();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        var result = new UserResult()
                        {
                            Id = user.Id,
                            FullName = user.FullName,
                            Username = user.Username,
                            Dob = user.Dob,
                            Email = user.Email,
                            Image = user.Image,
                            Password = user.Password,
                            Status = user.Status,
                            RoleId = user.RoleId,
                            Role = user.Role.RoleName
                        };
                        results.Add(result);
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                return Error.NotFound("Users not found!");
            }
        }

        public async Task<ErrorOr<IList<UserResult>>> GetStaffs(int userId)
        {
            try
            {
                IList<User> users = new List<User>();
                //users = _userRepository.GetStaffs().Result;
                var results = new List<UserResult>();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if(user.Id != userId) {
                            var result = new UserResult()
                            {
                                Id = user.Id,
                                FullName = user.FullName,
                                Username = user.Username,
                                Dob = user.Dob,
                                Email = user.Email,
                                Image = user.Image,
                                Password = user.Password,
                                Status = user.Status,
                                RoleId = user.RoleId,
                                Role = user.Role.RoleName
                            };
                            results.Add(result);
                        }                     
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                return Error.NotFound("Users not found!");
            }
        }

        public async Task<ErrorOr<UserResult>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user != null)
                {
                    var result = new UserResult()
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Username = user.Username,
                        Dob = user.Dob,
                        Email = user.Email,
                        Image = user.Image,
                        Password = user.Password,
                        Status = user.Status,
                        RoleId = user.RoleId,
                        Role = user.Role.RoleName
                    };
                    return result;
                }
                else
                {
                    return Error.NotFound("404", "User not found!");
                }
                
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
