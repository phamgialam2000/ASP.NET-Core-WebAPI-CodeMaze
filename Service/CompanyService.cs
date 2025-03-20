using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            //try
            //{
                var companies = _repository.Company.GetAllCompanies(trackChanges);
                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                //var companiesDto = companies.Select(c =>
                //            new CompanyDto(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country)))
                //            .ToList();
                return companiesDto;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)}service method {ex}");
            //    throw;
            //}
            //xóa trycatch vì nếu có lỗi đã có ExceptionMiddlewareExtensions đăng kí ở Program  
        }
    }
}
