using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using System.Linq.Expressions;

namespace IMS.Common
{
    public class Paginate<T>
    {
        private readonly IMSContext _context = new IMSContext();
        private int pageNumber;
        private int pageSize;
        private int numberOfPage;

        public Paginate(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }

        public IEnumerable<T> GetListPaginate<T>(
        Dictionary<string, dynamic> filters = null,
        Dictionary<string, dynamic> searches = null) where T : class
        {
            if (filters == null && searches == null)
            {
                int countObject1 = _context.Set<T>().Count();

                if (countObject1 % pageSize > 0)
                {
                    this.numberOfPage = countObject1 / pageSize + 1;
                }
                else
                {
                    this.numberOfPage = countObject1 / pageSize;
                }

                return _context.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            IQueryable<T> query = null;
            var filterExpressions = new List<Expression>();
            if (filters != null && filters.Any())
            {
                var parameter = Expression.Parameter(typeof(T), "x");

                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(T).GetProperty(filter.Key);

                    if (propertyInfo != null)
                    {
                        var property = Expression.Property(parameter, filter.Key);

                        if (propertyInfo.PropertyType.IsGenericType &&
                            propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            // Xử lý kiểu nullable
                            var hasValueProperty = Expression.Property(property, "HasValue");
                            var equality = Expression.Equal(hasValueProperty, Expression.Constant(true));

                            // Tạo một biểu thức kiểm tra giá trị nếu filter.Value không phải là null
                            var filterValue = Expression.Constant(filter.Value);
                            var valueProperty = Expression.Property(property, "Value");
                            var valueEquality = Expression.Equal(valueProperty, filterValue);

                            // Kết hợp cả hai điều kiện với AND
                            var combinedEquality = Expression.AndAlso(equality, valueEquality);

                            var lambda = Expression.Lambda<Func<T, bool>>(combinedEquality, parameter);

                            filterExpressions.Add(lambda);
                        }
                        else
                        {
                            // Xử lý kiểu không nullable
                            var value = Expression.Constant(filter.Value);
                            var equality = Expression.Equal(property, value);
                            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

                            filterExpressions.Add(lambda);
                        }

                        if (query == null)
                        {
                            query = filterExpressions.Count == 0 ? null :
                            _context.Set<T>().Where((Expression<Func<T, bool>>)filterExpressions.Aggregate(Expression.AndAlso));
                        }
                        else
                        {
                            query = filterExpressions.Count == 0 ? null :
                            query.Where((Expression<Func<T, bool>>)filterExpressions.Aggregate(Expression.AndAlso));
                        }

                            filterExpressions.Clear();
                    }
                }
            }

            var searchExpressions = new List<Expression>();
            if (searches != null && searches.Any())
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                List<IQueryable<T>> queryList = new List<IQueryable<T>>();

                foreach (var search in searches)
                {
                    var propertyInfo = typeof(T).GetProperty(search.Key);
                    IQueryable<T> subQuery;
                    if (propertyInfo != null)
                    {
                        var property = Expression.Property(parameter, propertyInfo);
                        var value = Expression.Constant(search.Value);
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var containsExpression = Expression.Call(property, containsMethod, value);
                        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

                        searchExpressions.Add(lambda);

                        if (query == null)
                        {
                            subQuery = searchExpressions.Count == 0 ? null :
                                _context.Set<T>().Where((Expression<Func<T, bool>>)searchExpressions.Aggregate(Expression.AndAlso));
                        }
                        else
                        {
                            subQuery = searchExpressions.Count == 0 ? query :
                                query.Where((Expression<Func<T, bool>>)searchExpressions.Aggregate(Expression.AndAlso));
                        }
                        queryList.Add(subQuery);
                        searchExpressions.Clear();
                    }
                }

                if (queryList.FirstOrDefault() != null)
                {
                    query = queryList.FirstOrDefault();
                    foreach (var subQuery in queryList.Skip(1))
                    {
                        query = query.Union(query);
                    }
                }

            }

            if (query == null)
            {
                int countObject = _context.Set<T>().Count();

                if (countObject % pageSize > 0)
                {
                    this.numberOfPage = countObject / pageSize + 1;
                }
                else
                {
                    this.numberOfPage = countObject / pageSize;
                }
                return _context.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                int countObject = query.Count();

                if (countObject % pageSize > 0)
                {
                    this.numberOfPage = countObject / pageSize + 1;
                }
                else
                {
                    this.numberOfPage = countObject / pageSize;
                }
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            _context.Dispose();
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
            return _context.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> GetListPaginate(List<T> listObject)
        {
            int countObject = listObject.Count();
            if (countObject % pageSize > 0)
            {
                this.numberOfPage = listObject.Count() / pageSize + 1;
            }
            else
            {
                this.numberOfPage = countObject / pageSize;
            }
            return listObject.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
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



