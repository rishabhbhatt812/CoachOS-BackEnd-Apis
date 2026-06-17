using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Application.Interfaces.Services;
using CoachOS.Domain.Tenancy;
using CoachOS.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachOS.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/branches")]
    [Authorize(Roles = "INSTITUTE_ADMIN,GLOBAL_ADMIN,SUPER_ADMIN")]
    public class BranchesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public BranchesController(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var instId = _currentUserService.InstituteId;
            if (instId == null) return BadRequest(ApiResponse<List<BranchDto>>.Fail("Institute context missing."));

            var branches = await _unitOfWork.Repository<Branch>().GetAllAsync();
            var dtos = branches.Select(b => new BranchDto
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code,
                Address = b.Address,
                ContactNumber = b.ContactNumber,
                IsActive = b.IsActive
            }).ToList();

            return Ok(ApiResponse<List<BranchDto>>.Ok(dtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranch(Guid id)
        {
            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
            if (branch == null) return NotFound(ApiResponse<BranchDto>.Fail("Branch not found."));

            var dto = new BranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Code = branch.Code,
                Address = branch.Address,
                ContactNumber = branch.ContactNumber,
                IsActive = branch.IsActive
            };

            return Ok(ApiResponse<BranchDto>.Ok(dto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDto request)
        {
            var instId = _currentUserService.InstituteId;
            if (instId == null) return BadRequest(ApiResponse<BranchDto>.Fail("Institute context missing."));

            var branch = new Branch
            {
                InstituteId = instId.Value,
                Name = request.Name,
                Code = request.Code,
                Address = request.Address,
                ContactNumber = request.ContactNumber,
                IsActive = true
            };

            await _unitOfWork.Repository<Branch>().AddAsync(branch);
            await _unitOfWork.SaveChangesAsync();

            var dto = new BranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Code = branch.Code,
                Address = branch.Address,
                ContactNumber = branch.ContactNumber,
                IsActive = branch.IsActive
            };

            return Ok(ApiResponse<BranchDto>.Ok(dto, "Branch created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(Guid id, [FromBody] UpdateBranchDto request)
        {
            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
            if (branch == null) return NotFound(ApiResponse<BranchDto>.Fail("Branch not found."));

            branch.Name = request.Name;
            branch.Code = request.Code;
            branch.Address = request.Address;
            branch.ContactNumber = request.ContactNumber;
            branch.IsActive = request.IsActive;

            _unitOfWork.Repository<Branch>().Update(branch);
            await _unitOfWork.SaveChangesAsync();

            var dto = new BranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Code = branch.Code,
                Address = branch.Address,
                ContactNumber = branch.ContactNumber,
                IsActive = branch.IsActive
            };

            return Ok(ApiResponse<BranchDto>.Ok(dto, "Branch updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(Guid id)
        {
            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
            if (branch == null) return NotFound(ApiResponse<bool>.Fail("Branch not found."));

            _unitOfWork.Repository<Branch>().Remove(branch);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.Ok(true, "Branch deleted successfully."));
        }
    }

    public class BranchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateBranchDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class UpdateBranchDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
