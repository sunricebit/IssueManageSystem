using IMS.ViewModels.Paginate;

namespace IMS.Common
{
    public class Paginate<T>
    {
        IEnumerable<T> listObject;
        int pageNumber;
        int pageSize;
        int numberOfPage;
        
        public Paginate(IEnumerable<T> objects, int pageNumber, int pageSize)
        {
            this.listObject = objects;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            if (this.listObject.Count() % pageSize > 0)
            {
                this.numberOfPage = this.listObject.Count() / pageSize + 1;
            }
            else
            {
                this.numberOfPage = this.listObject.Count() / pageSize;
            }
        }

        public IEnumerable<T> GetListPaginate()
        {
            int startIndex = (pageNumber - 1) * pageSize;
            int endIndex = pageNumber == numberOfPage ? listObject.Count() - 1 : pageNumber * pageSize - 1;
            return this.listObject.ToList().GetRange(startIndex, endIndex - startIndex + 1);
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
