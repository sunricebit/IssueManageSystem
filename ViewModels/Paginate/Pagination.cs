namespace IMS.ViewModels.Paginate
{
    public class Pagination
    {
        public int NumberOfPage { get; set; }
        public int PageNumber { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        //public Dictionary<string, dynamic> x;
        //public Dictionary<string, dynamic> y;
    }
}
