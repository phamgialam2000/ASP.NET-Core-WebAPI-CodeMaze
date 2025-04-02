using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }
        #region no paging
        //public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges) =>
        //    await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
        //    .OrderBy(e => e.Name)
        //    .ToListAsync(); 
        #endregion
        #region note
        //Phương pháp thứ nhất:
        //Lấy toàn bộ dữ liệu thỏa mãn điều kiện từ database(một danh sách lớn) rồi sau đó thực hiện phân trang trong bộ nhớ.
        //Điều này sẽ tốn nhiều bộ nhớ và hiệu suất sẽ rất kém khi số lượng dữ liệu lớn.

        //Phương pháp thứ hai:
        //Sử dụng Skip và Take để truy vấn chỉ một trang dữ liệu từ database, giúp giảm thiểu lượng dữ liệu cần tải về.
        //Thực hiện riêng truy vấn Count để lấy tổng số dòng, giúp tính toán phân trang một cách hiệu quả.
        //Nhờ đó, chỉ dữ liệu cần thiết cho trang hiện tại được lấy về, cải thiện hiệu suất và sử dụng tài nguyên tốt hơn.
        #endregion
        #region paging with large data
        //public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        //{
        //    var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
        //    .OrderBy(e => e.Name)
        //    .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
        //    .Take(employeeParameters.PageSize)
        //    .ToListAsync();
        //    var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();
        //    return new PagedList<Employee>(employees, count,
        //    employeeParameters.PageNumber, employeeParameters.PageSize);

        //}
        #endregion
        #region paging with little data
        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
             .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
             .Search(employeeParameters.SearchTerm)
             .Sort(employeeParameters.OrderBy)
             //.OrderBy(e => e.Name)
            .ToListAsync();
            return PagedList<Employee>
            .ToPagedList(employees, employeeParameters.PageNumber,
           employeeParameters.PageSize);
        }
        #endregion
        

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
             await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmployee(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);
    }
}
