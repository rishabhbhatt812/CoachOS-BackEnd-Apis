using CoachOS.Domain.Communication;
using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoachOS.Shared.Responses;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CoachOS.Api.Controllers.Teacher
{
    [ApiController]
    [Route("api/teacher/notifications")]
    [Authorize(Roles = "TEACHER")]
    public class TeacherNotificationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public TeacherNotificationsController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = _currentUserService.UserId;
            var allNotifications = await _unitOfWork.Repository<Notification>().GetAllAsync();
            var myNotifications = allNotifications.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt).ToList();

            return Ok(ApiResponse<List<Notification>>.Ok(myNotifications, "Notifications retrieved"));
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);
            if (notification != null && notification.UserId == _currentUserService.UserId)
            {
                notification.IsRead = true;
                _unitOfWork.Repository<Notification>().Update(notification);
                await _unitOfWork.SaveChangesAsync();
            }
            return Ok(ApiResponse<bool>.Ok(true));
        }
    }
}
