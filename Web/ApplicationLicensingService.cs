using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Snail.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Web
{
    public class ApplicationLicensingService : IApplicationLicensingService
    {
        private IOptionsMonitor<ApplicationlicensingOption> _options;
        private Dictionary<string, string> _licenseInfo;
        private string _error = "";
        private bool _hasChecked = false;
        private bool _licenseIsRight = false;
        private object locker = new object();
        public ApplicationLicensingService(IOptionsMonitor<ApplicationlicensingOption> options)
        {
            _options = options;
            _options.OnChange(opt =>
            {
                _hasChecked = false;
                EnsureLicenseChecked();
            });
        }

        public Dictionary<string, string> GetLicenseInfo()
        {
            EnsureLicenseChecked();
            return _licenseInfo;
        }

        public bool LicenseIsRight(out string error)
        {
            EnsureLicenseChecked();
            error = _error;
            return _licenseIsRight;
        }

        private void EnsureLicenseChecked()
        {
            if (!_hasChecked)
            {
                lock (locker)
                {
                    if (!_hasChecked)
                    {
                        if (_options.CurrentValue.Token == "123456")
                        {
                            _error = "";
                            _licenseIsRight = true;
                        }
                        else
                        {
                            _error = "token错误";
                            _licenseIsRight = false;
                        }
                        _hasChecked = true;
                    }
                }
            }
        }
    }

    public interface IApplicationLicensingService
    {
        bool LicenseIsRight(out string error);
        Dictionary<string, string> GetLicenseInfo();
    }

    public class ApplicationlicensingOption
    {
        public string Token { get; set; }
        public string RSAPublicKey { get; set; }
    }

    public class ApplicationLicensingMiddleware
    {
        private IApplicationLicensingService _applicationlicensingService;
        private readonly RequestDelegate _next;
        public ApplicationLicensingMiddleware(IApplicationLicensingService applicationlicensingService, RequestDelegate next)
        {
            _applicationlicensingService = applicationlicensingService;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (_applicationlicensingService.LicenseIsRight(out string error))
            {
                await _next(context);
            }
            else
            {
                throw new BusinessException($"应用授权失败，失败信息：{error}");
            }
        }
    }

    public static class ApplicationLicensingServiceExtenssion
    {
        public static void AddApplicationLicensing(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IApplicationLicensingService, ApplicationLicensingService>();
            services.Configure<ApplicationlicensingOption>(configuration);
        }
        public static IApplicationBuilder UseApplicationLicensing(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            return app.UseMiddleware<ApplicationLicensingMiddleware>();
        }
    }
}
