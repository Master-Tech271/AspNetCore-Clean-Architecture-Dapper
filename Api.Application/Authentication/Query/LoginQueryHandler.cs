using Api.Application.Authentication.Command;
using Api.Application.Authentication.Handler;
using Api.Application.Common;
using Api.Application.Dtos.Authentication;
using Api.Domain.Entities;
using Api.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Authentication.Query
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEnumExtensions _enumExtension;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginQueryHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IEnumExtensions enumExtension, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _enumExtension = enumExtension;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // Begin the transaction here
            _unitOfWork.BeginReadTransaction();

            var userRepo = _unitOfWork.GetRepository<User>();
            var userRoleRepo = _unitOfWork.GetRepository<UserRole>();


            User? user = await userRepo.GetByColumnAsync(nameof(request.Email), request.Email);
            
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password.");
            }

            var userRole = await userRoleRepo.GetByColumnAsync(nameof(UserRole.UserId), user.Id);

            if(userRole is null)
            {
                throw new Exception("Invalid role.");
            }

            // Commit the transaction
            await _unitOfWork.CompleteAsync();

            var authResponse = new AuthResponse
            {
                User = new UserDto
                {
                    Email = user.Email,
                    Role = _enumExtension.GetEnumInfo((EnumUserRole)userRole.RoleId).Name,
                    Username = user.Username,
                },
                Token = _jwtTokenGenerator.GenerateToken(user, (EnumUserRole)userRole.RoleId)
            };
            return authResponse;

        }
    }
}
