using IMS.ViewModels.Paginate;

namespace IMS.Common
{
    public class Paginate<T>
    {
        private readonly IMSContext _context = new IMSContext();

        int pageNumber;
        int pageSize;
        int numberOfPage;
        
        public Paginate(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }

        public IEnumerable<T> GetListPaginate<T>() where T : class
        {
            int countObject = _context.Set<T>().Count();
            if (countObject % pageSize > 0)
            {
                this.numberOfPage = _context.Set<T>().Count() / pageSize + 1;
            }
            else
            {
                this.numberOfPage = countObject / pageSize;
            }
            int startIndex = (pageNumber - 1) * pageSize;
            int endIndex = pageNumber == numberOfPage ? countObject - 1 : pageNumber * pageSize - 1;
            return _context.Set<T>().ToList().GetRange(startIndex, endIndex - startIndex + 1);
        }

        public Pagination GetPagination()
        {
            Pagination p = new Pagination();
            if (pageNumber <= 1)
            {
                p.HasPreviousPage = false;
            }
            else
            {
                p.HasPreviousPage = true;
            }
            
            if (pageNumber == numberOfPage)
            {
                p.HasNextPage = false;
            }
            else
            {
                p.HasNextPage = true;
            }

            p.NumberOfPage = this.numberOfPage;
            p.PageNumber = this.pageNumber;
            return p;
        }
    }
}
