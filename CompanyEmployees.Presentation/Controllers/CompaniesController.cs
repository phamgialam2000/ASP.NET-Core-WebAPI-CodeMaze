using Asp.Versioning;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
    //[ResponseCache(CacheProfileName = "120SecondsDuration")]
    [OutputCache(PolicyName = "120SecondsDuration")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service)
        {
            _service = service;
        }
        [HttpGet(Name = "GetCompanies")]
        [EnableRateLimiting("SpecificPolicy")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCompanies()
        {
            #region testing exception
            //try
            //{
            //throw new Exception("Exception");
            //result: {"StatusCode": 500,
            //"Message": "Internal Server Error."}
            #endregion
            var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
                return Ok(companies);
            #region ExceptionMiddlewareExtensions
            //}
            //catch (Exception)
            //{
            //    return StatusCode(500, "Internal server error");

            //}
            //xóa trycatch vì nếu có lỗi đã có ExceptionMiddlewareExtensions đăng kí ở Program  
            #endregion

        }

        //The OutputCache attribute contains the properties that ResponseCache has
        [HttpGet("{id:guid}", Name = "CompanyById")]
        //[OutputCache(NoStore = true)] if you want to disable caching
        //[ResponseCache(Duration = 60)]
        [OutputCache(Duration = 60)]
        [DisableRateLimiting]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
            var etag = $"\"{Guid.NewGuid():n}\""; //Caching Revalidation
            HttpContext.Response.Headers.ETag = etag;
            return Ok(company);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            #region using ValidationFilterAttribute should not use this method anymore
            //if (company is null)
            //{
            //    return BadRequest("CompanyForCreationDto object is null");
            //}
            #endregion
            var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType =
        typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(companies);
        }


        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            #region using ValidationFilterAttribute should not use this method anymore
            if (company is null)
            {
                return BadRequest("CompanyForUpdateDto object is null");
            }
            #endregion
            await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");
            return Ok();
        }

    }
}

