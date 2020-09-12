using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Snail.Common;
using Snail.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
namespace Web
{
    /// <summary>
    /// 序列号
    /// </summary>
    public class ApplicationLicensingService : IApplicationLicensingService
    {
        public static readonly string publicKeySignKeyName = "publicKeySign";
        public static readonly string computerFingerKeyName = "computerFinger";
        private IOptionsMonitor<ApplicationlicensingOption> _options;
        private Dictionary<string, string> _licenseInfo = new Dictionary<string, string>();
        private string _error = "";
        private bool _hasChecked = false;
        private bool _licenseIsRight = false;
        private object locker = new object();
        private ILogger _logger;
        public ApplicationLicensingService(IOptionsMonitor<ApplicationlicensingOption> options,ILogger<ApplicationLicensingService> logger)
        {
            _logger = logger;
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
                        _licenseInfo.Clear();
                        _licenseIsRight = false;
                        _hasChecked = true;
                        var hander = new JsonWebTokenHandler();// JsonWebTokenHandler和JwtSecurityTokenHandler都可以处理jwt，前者是后出的，建议使用
                        var validateResult = hander.ValidateToken(_options.CurrentValue.Token, new TokenValidationParameters
                        {
                            IssuerSigningKey = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPublicPem(_options.CurrentValue.RSAPublicKey)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        });
                        if (!validateResult.IsValid)
                        {
                            _error = "错误码001";//token无效，可能是token格式不对，或是publicKey不对
                            _logger.LogCritical(validateResult.Exception, "license错误码001");
                            return;
                        }
                        var jwtSecurityToken = hander.ReadJsonWebToken(_options.CurrentValue.Token);
                        jwtSecurityToken.Claims.ToList().ForEach(claim =>
                        {
                            _licenseInfo.Add(claim.Type, claim.Value);
                        });
                        if (!_licenseInfo.TryGetValue(publicKeySignKeyName, out string publicKeySign))
                        {
                            _error = "错误码002";//缺失publicKeySign，可能是license颁发出错
                            _logger.LogCritical($"license错误码002，license信息为：{JsonConvert.SerializeObject(_options.CurrentValue)}");
                            return;
                        }
                        if (!_licenseInfo.TryGetValue(computerFingerKeyName, out string computerFinger))
                        {
                            _error = "错误码003";//缺失computerFinger，可能是license颁发出错
                            _logger.LogCritical($"错误码003，license信息为：{JsonConvert.SerializeObject(_options.CurrentValue)}");
                            return;
                        }
                        if (!publicKeySign.Equals(HashHelper.Md5($"{_options.CurrentValue.RSAPublicKey}shengyu"), StringComparison.OrdinalIgnoreCase))
                        {
                            _error = "错误码004";//publicKeySign无效，可能是用户尝试破解，自己生成了一对公私钥，并用自己的私钥生成jwt token
                            _logger.LogCritical($"错误码004，license信息为：{JsonConvert.SerializeObject(_options.CurrentValue)}");
                            return;
                        }
                        if (!computerFinger.Equals(ComputerFinger.GetFinger(), StringComparison.OrdinalIgnoreCase))
                        {
                            _error = "错误码005";//机器码不匹配，可能是用户将已经license放到另一台机子上
                            _logger.LogCritical($"错误码005，license信息为：{JsonConvert.SerializeObject(_options.CurrentValue)}");
                            return;
                        }
                        _licenseIsRight = true;
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
        /// <summary>
        /// jwt token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用于验签的rsa public key
        /// </summary>
        public string RSAPublicKey { get; set; }
    }

    public class ApplicationLicensingMiddleware
    {
        private IApplicationLicensingService _applicationlicensingService;
        private readonly RequestDelegate _next;
        private string _computerFinger;
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
                if (string.IsNullOrEmpty(_computerFinger))
                {
                    _computerFinger = ComputerFinger.GetFinger();
                }
                throw new BusinessException($"应用授权失败，机器码{_computerFinger}，失败信息：{error}");
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
