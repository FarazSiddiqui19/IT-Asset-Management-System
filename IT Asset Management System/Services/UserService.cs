using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.DTOs.User;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Services.Interfaces;

namespace IT_Asset_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUnitOfWork _unitOfWork;


        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _assignmentRepository = assignmentRepository;
            _unitOfWork = unitOfWork;
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


            await _userRepository.AddAsync(user);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");

            var Added = await _userRepository.GetByIdAsync(user.Id)??
                throw new InternalServerException("Failed to retrieve the created user. Please try again.");


            return Added.ToDto();
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
           
            await _userRepository.UpdateAsync(user);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }

        public async Task DeactivateAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found.");

            if (!user.IsActive) throw new ValidationException("User is already deactivated.");

            if (await _userRepository.HasActiveAssignmentsAsync(id))
                throw new ValidationException("User cannot be deactivated because they have active assignments.");

            if (await _userRepository.HasInProgressTicketsAsync(id))
                throw new ValidationException("User cannot be deactivated because they have in progress tickets.");

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }
    }
}
