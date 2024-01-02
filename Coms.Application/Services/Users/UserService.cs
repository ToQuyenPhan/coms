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

        public async Task<ErrorOr<PagingResult<UserResult>>> GetUsers(string fullName, string email, int? roleId, int? status, 
                int currentPage, int pageSize)
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
                    if (!string.IsNullOrEmpty(fullName))
                    {
                        results = results.Where(u => u.FullName.Contains(fullName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                                .ToList();
                    }
                    if (!string.IsNullOrEmpty(email))
                    {
                        results = results.Where(u => u.Email.Contains(email.Trim(), StringComparison.CurrentCultureIgnoreCase))
                                .ToList();
                    }
                    if(roleId.HasValue)
                    {
                        results = results.Where(u => u.RoleId.Equals(roleId)).ToList();
                    }
                    if (status.HasValue)
                    {
                        results = results.Where(u => u.Status.Equals(status)).ToList();
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

        public async Task<ErrorOr<UserResult>> InactiveUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user is not null)
                {
                    user.Status = (int)UserStatus.Inactive;
                    await _userRepository.UpdateUser(user);
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

        public async Task<ErrorOr<UserResult>> ActiveUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user is not null)
                {
                    user.Status = (int)UserStatus.Active;
                    await _userRepository.UpdateUser(user);
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

        public async Task<ErrorOr<UserResult>> AddUser(string fullName, string username, DateTime dob, string image, 
                string password, int roleId, string email, string position, string phone)
        {
            try
            {
                var existingUsername = await _userRepository.GetUserByUsername(username);
                if(existingUsername is not null)
                {
                    return Error.Conflict("409", "Username already exists!");
                }
                var existingEmail = await _userRepository.GetByEmail(email);
                if (existingEmail is not null)
                {
                    return Error.Conflict("409", "Email already exists!");
                }
                var existingPhone = await _userRepository.GetByPhone(phone);
                if (existingPhone is not null)
                {
                    return Error.Conflict("409", "Phone number already exists!");
                }
                var user = new User()
                {
                    FullName = fullName,
                    Username = username,
                    Dob = dob,
                    Email = email,
                    Image = image,
                    Password = password,
                    Status = (int) UserStatus.Active,
                    RoleId = roleId,
                    Position = position,
                    Phone = phone
                };
                await _userRepository.AddUser(user);
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
                    Phone = user.Phone
                };
                return result;
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
