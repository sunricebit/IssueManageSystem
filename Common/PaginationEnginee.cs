namespace IMS.Common
{
    public class PaginateEnginee<T, A>
    {
        public int PageIndex { get; set; }
        public int ItemCount { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }

        //Add additional
        public A Additional { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)ItemCount / PageSize);
        public bool CanNextPage => PageIndex < TotalPages;
        public bool CanPreviousPage => PageIndex > 1;
        public bool NoItemsFound => ItemCount == 0;

        private PaginateEnginee(List<T> items, int pageIndex, int pageSize, int itemCount)
        {
            PageSize = pageSize;
            ItemCount = itemCount;
            PageIndex = pageIndex > TotalPages ? 1 : pageIndex;
            Items = items;
        }

        public static PaginateEnginee<T, A> Create(IQueryable<T> source, int pageIndex, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("PageIndex should be greater than 0.", nameof(pageIndex));
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("PageSize should be greater than 0.", nameof(pageSize));
            }
            int itemCount = source.Count();
            List<T> items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginateEnginee<T, A>(items, pageIndex, pageSize, itemCount);
        }
    }
}

