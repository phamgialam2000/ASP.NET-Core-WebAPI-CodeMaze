using Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;
        public RepositoryBase(RepositoryContext repositoryContext)
        => RepositoryContext = repositoryContext;

        public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
        RepositoryContext.Set<T>()
        .AsNoTracking() :
        RepositoryContext.Set<T>();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges) =>
        !trackChanges ?
        RepositoryContext.Set<T>()
        .Where(expression)
        .AsNoTracking() :
        RepositoryContext.Set<T>()
        .Where(expression);
        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    }
}

// When it’s set to false (trackChanges), we attach the AsNoTracking method to our query to inform EF Core that it
// doesn’t need to track changes for the required entities. This greatly improves the speed of a query.

//Expression<Func<T, bool>>
// bool: Lọc dữ liệu (Where)
// int: Truy xuất số (Select)
// string: Truy xuất chuỗi (Select)
// object: Ánh xạ đối tượng (Select → DTO) ✅ Dùng Expression<Func<T, TResult>> giúp LINQ dịch trực tiếp thành SQL tối ưu hơn