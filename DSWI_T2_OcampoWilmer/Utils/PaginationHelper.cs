public class PaginationHelper
{
    public static (int TotalPages, int Offset, int ItemsPerPage) CalculatePagination(IEnumerable<object> items, int currentPage)
    {
        int itemsPerPage = 15;
        int totalItems = items.Count();
        int totalPages = totalItems % itemsPerPage == 0 ? totalItems / itemsPerPage : totalItems / itemsPerPage + 1;
        int offset = itemsPerPage * currentPage;

        return (totalPages, offset, itemsPerPage);
    }
}
