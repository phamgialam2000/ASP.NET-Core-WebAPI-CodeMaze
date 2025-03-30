using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        private readonly Lazy<ICompanyRepository> _companyRepository;
        
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_repositoryContext));
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(_repositoryContext));
        }

        public ICompanyRepository Company => _companyRepository.Value;
        public IEmployeeRepository Employee => _employeeRepository.Value;
        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }

}
