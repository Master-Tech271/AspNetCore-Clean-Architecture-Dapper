using Api.Application.Common;
using Api.Application.Dtos.Authentication;
using Api.Domain.Entities;
using Api.Domain.Enums;
using MediatR;

namespace Api.Application.Authentication.Command
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEnumExtensions _enumExtension;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IEnumExtensions enumExtension, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _enumExtension = enumExtension;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Begin the transaction here
            _unitOfWork.BeginTransaction();

            var userRepo = _unitOfWork.GetRepository<User>();
            var userRoleRepo = _unitOfWork.GetRepository<UserRole>();

            //check user is already exists!
            var existingUserByEmail = await userRepo.GetByColumnAsync(nameof(request.Email), request.Email);
            var existingUserByUsername = await userRepo.GetByColumnAsync(nameof(request.Username), request.Username);
            if (existingUserByEmail is not null || existingUserByUsername is not null)
            {
                throw new Exception("User Already Exists!");
            }
            // Create a new user
            var user = new User
            {
                Email = request.Email,
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = _dateTimeProvider.UtcNow,
                UpdatedAt = _dateTimeProvider.UtcNow,
            };



            // Add the user to the repository
            int userId = await userRepo.AddAsync(user);

            //Assign Role
            var userRole = new UserRole
            {
                RoleId = 1,
                UserId = userId
            };

            await userRoleRepo.AddAsync(userRole);

            var createdUser = await userRepo.GetByIdAsync(userId);

            // Commit the transaction
            await _unitOfWork.CompleteAsync();

            if (createdUser == null)
            {
                throw new Exception("User is not Registered!");
            }


            var authResponse = new AuthResponse
            {
                User = new UserDto
                {
                    Email = createdUser.Email,
                    Role = _enumExtension.GetEnumInfo(EnumUserRole.User).Name,
                    Username = createdUser.Username,
                },
                Token = _jwtTokenGenerator.GenerateToken(user, EnumUserRole.User)
            };
            return authResponse;

        }
    }
}
