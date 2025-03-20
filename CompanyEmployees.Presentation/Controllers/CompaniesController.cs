using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetCompanies()
        {
            //try
            //{
            //throw new Exception("Exception"); // testing exception middleware
                                              //result: {"StatusCode": 500,
                                                        //"Message": "Internal Server Error."}

            var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
                return Ok(companies);
            //}
            //catch (Exception)
            //{
            //    return StatusCode(500, "Internal server error");

            //}
            //xóa trycatch vì nếu có lỗi đã có ExceptionMiddlewareExtensions đăng kí ở Program  

        }
    }
}

