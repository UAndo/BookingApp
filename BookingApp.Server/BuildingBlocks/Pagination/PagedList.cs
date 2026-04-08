namespace BookingApp.Server.BuildingBlocks.Pagination
{
    public class PagedList<T>(List<T> items, int count, int pageNumber, int pageSize)
    {
        public List<T> Items { get; } = items;
        public int PageNumber { get; } = pageNumber;
        public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
        public int TotalCount { get; } = count;
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct)
        {
            var count = await source.CountAsync(ct);
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
