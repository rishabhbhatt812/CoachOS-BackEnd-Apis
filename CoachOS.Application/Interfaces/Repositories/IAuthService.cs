using CoachOS.Application.Features.Auth.Dtos;
using CoachOS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterInstituteResponse>> RegisterInstituteAsync(RegisterInstituteRequest request);
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<CreateUserResponse>> CreateUserAsync(CreateUserRequest request);
    }
}
