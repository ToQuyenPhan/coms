using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<PagingResult<UserResult>>> GetUsers(int currentPage, int pageSize)
        {
            try
            {
                var users = _userRepository.GetUsers();
                if(users is not null)
                {
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
                    int total = results.Count();
                    if (currentPage > 0 && pageSize > 0)
                    {
                        results = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return new PagingResult<UserResult>(results, total, currentPage, pageSize);
                }
                else
                {
                    return new PagingResult<UserResult>(new List<UserResult>(), 0, currentPage, pageSize);
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
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
    }
}
