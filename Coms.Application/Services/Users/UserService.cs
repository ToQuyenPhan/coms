using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Services;
using Coms.Application.Services.Templates;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
                //users = _userRepository.GetUsers().Result;
                var results = new List<UserResult>();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        var result = new UserResult()
                        {
                            Id = user.Id,
                            FullName = user.FullName,
                            Username =user.Username,
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
                //users = _userRepository.GetManagers().Result;
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
        public async Task<ErrorOr<UserResult>> AddUser(string fullName, string username, string email, string password, DateTime dob, string image, int roleId, int status)
        {
            try
            {
                var user = new User
                {
                    FullName = fullName,
                    Username = username,
                    Email = email,
                    Password = password,
                    Image = image,
                    RoleId = roleId,
                    Dob = dob,
                    Status = status,
                };
                await _userRepository.AddUser(user);
                var userCreated = _userRepository.GetUser(user.Id).Result;
                var result = new UserResult
                {
                    Id = userCreated.Id,
                    FullName = userCreated.FullName,
                    Username= userCreated.Username,
                    Email = userCreated.Email,
                    Password = userCreated.Password,
                    Status = userCreated.Status,
                    Dob= userCreated.Dob,
                    Image = userCreated.Image,
                    RoleId = userCreated.RoleId,
                    Role = userCreated.Role.RoleName
                };
                return result;
            }
            catch (Exception ex) 
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<UserResult>> UpdateUser(string fullName, string username, string email, string password, DateTime dob, string image, int roleId, int status, int userId)
        {
            try
            {
                var user = await _userRepository.GetUser(userId);
                if (user is not null)
                {
                    user.FullName = fullName;
                    user.Username = username;
                    user.Email = email;
                    user.Password = password;
                    user.Image = image;
                    user.RoleId = roleId;
                    user.Dob = dob;
                    user.Status = status;
                    await _userRepository.UpdateUser(user);
                    var result = new UserResult
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Username = user.Username,
                        Email = user.Email,
                        Password = user.Password,
                        Status = user.Status,
                        Dob = user.Dob,
                        Image = user.Image,
                        RoleId = user.RoleId,
                        Role = user.Role.RoleName
                    };
                    return result;
                }
            else
            {
                return Error.NotFound("404", "User is not found!");
            } }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<UserResult>> DeleteUser(int userId)
        {
            try
            {
                if (_userRepository.GetUser(userId).Result is not null)
                {
                    var user = await _userRepository.GetUser(userId);
                    if (user.Status == (int)UserStatus.Inactive)
                    {
                        return Error.Failure("400", "User is already inactive!");
                    }
                    user.Status = (int)UserStatus.Inactive;
                    await _userRepository.UpdateUser(user);
                    var result = new UserResult
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Username = user.Username,
                        Email = user.Email,
                        Password = user.Password,
                        Status = user.Status,
                        Dob = user.Dob,
                        Image = user.Image,
                        RoleId = user.RoleId,
                        Role = user.Role.RoleName
                    };
                    return result;
                }
                else
                {
                    return Error.NotFound("404", "User is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
