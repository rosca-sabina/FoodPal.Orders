using System.Collections.Generic;

namespace FoodPal.Orders.DTOs
{
    public class PagedResultSetDTO<T> where T: class
    {
        public IList<T> Data { get; set; }
        public PaginationInfoDTO PaginationInfo { get; set; }
    }

    public class PaginationInfoDTO
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
