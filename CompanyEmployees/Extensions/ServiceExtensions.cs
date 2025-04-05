using Asp.Versioning;
using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;
using Service.DataShaping;
using Shared.DataTransferObjects;
using static System.Net.WebRequestMethods;
using System;
using Microsoft.AspNetCore.Http;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination"));

    });
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");

            }).AddMvc();
        }

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        
        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();
        
        public static void ConfigureDataShaper(this IServiceCollection services) =>
            services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();


        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new
            CsvOutputFormatter()));

        public static void ConfigureResponseCaching(this IServiceCollection services) =>
            services.AddResponseCaching();

        //public static void ConfigureOutputCaching(this IServiceCollection services) =>
        //    services.AddOutputCache();
        public static void ConfigureOutputCaching(this IServiceCollection services) =>
            services.AddOutputCache(opt =>
            {
                //opt.AddBasePolicy(bp => bp.Expire(TimeSpan.FromSeconds(10)));
                opt.AddPolicy("120SecondsDuration", p => p.Expire(TimeSpan.FromSeconds(120)));

            });
        #region note OutputCache
        //Khi một yêu cầu được gửi đến máy chủ, middleware sẽ kiểm tra xem phản hồi cho yêu cầu đó đã được lưu trữ hay chưa.Nếu đã được lưu trữ, nó sẽ trả về phản hồi đã lưu trữ thay vì xử lý lại yêu cầu.
        //AddBasePolicy:
        //Thêm chính sách cơ bản(BasePolicy) cho tất cả các phản hồi HTTP.
        //Các phản hồi sẽ hết hạn(expire) sau 10 giây.
        //Nghĩa là: trong vòng 10 giây sau một phản hồi được gửi, mọi request trùng lặp sẽ được lấy từ cache(không chạy lại controller/action).
        #endregion
    }
}
