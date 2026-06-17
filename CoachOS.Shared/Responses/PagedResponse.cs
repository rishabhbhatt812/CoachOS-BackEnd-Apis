using System;
using System.Collections.Generic;
using System.Text;

namespace CoachOS.Shared.Responses
{
    public class PagedResponse<T> : ApiResponse<List<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public static PagedResponse<T> Create(List<T> items, int pageNumber, int pageSize, int totalRecords, string message = "Success")
        {
            return new PagedResponse<T>
            {
                Success = true,
                Message = message,
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = pageSize == 0 ? 0 : (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }
    }
}
