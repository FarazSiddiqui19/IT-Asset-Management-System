using System.Threading.Tasks;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Services;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.User;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Common.Mappers;
using System;

namespace IT_Asset_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAssignmentRepository _assignmentRepository;


        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher,IAssignmentRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto dto)
        {
            // Check uniqueness
            var byEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (byEmail != null) throw new ConflictException("Email is already registered");

            var byUsername = await _userRepository.GetByUsernameAsync(dto.Username);
            if (byUsername != null) throw new ConflictException("User name is already taken");

            var hashedPassword = _passwordHasher.Hash(dto.Password);
            var user = dto.ToEntity(hashedPassword);


            var added = await _userRepository.AddAsync(user);
            return added.ToDto();
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found.");
            return user.ToDto();
        }

        public async Task UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found.");

            if(dto.Username !=null)
            {
                var existing = await _userRepository.GetByUsernameAsync(dto.Username);
                if (existing != null && existing.Id != id) throw new ConflictException("Username is already taken.");
                user.Username = dto.Username;
            }

            if (dto.Password != null) 
            {
                user.PasswordHash = _passwordHasher.Hash(dto.Password);
            }
           
      

            var ok = await _userRepository.UpdateAsync(user);
            if (!ok) throw new ValidationException("Failed to update user.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found.");

            if (await _userRepository.HasActiveAssignmentsAsync(id))
                throw new ValidationException("User cannot be deleted because they have active assignments.");

            if(await _userRepository.HasTicketsAsync(id))
                throw new ValidationException("User cannot be deleted because they have In progress tickets.");


            var okDel = await _userRepository.DeleteAsync(user);
            if (!okDel) throw new ValidationException("Failed to delete user.");
        }
    }
}
