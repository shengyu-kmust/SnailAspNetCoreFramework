using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities;
using Service;
using Snail.Common;
using Snail.Core;
using Snail.Web.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class CommonUseController : ControllerBase
    {
        private CommonUseService _service;
        public CommonUseController(CommonUseService commonUseService)
        {
            _service = commonUseService;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileStreamResult ExportExcel()
        {
            var stream = _service.ExportExcel();
            stream.Position = 0;
            return File(stream, "application/octet-stream","exportExcel.xls");
        }

        /// <summary>
        /// 文件上传，form-data里的key必须为formFiles才能正确绑定文件到对象
        /// </summary>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ExcelTestDto> ImportExcel(IFormFileCollection formFiles)
        {
            if (formFiles.Any())
            {
                var stream = formFiles[0].OpenReadStream();
                return _service.ImportExcel(stream);

            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 生成license
        /// </summary>
        /// <param name="licenseInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public string GenerateLicense(Dictionary<string,string> licenseInfo)
        {
            if (!licenseInfo.TryGetValue(ApplicationLicensingService.computerFingerKeyName,out string computerFinger))
            {
                throw new BusinessException($"缺少机器码参数，参数名为{ApplicationLicensingService.computerFingerKeyName}");
            }
            if (!licenseInfo.TryGetValue("rsaPrivateKey", out string rsaPrivateKey))
            {
                throw new BusinessException($"缺少rsaPrivateKey");
            }
            if (!licenseInfo.TryGetValue("rsaPublicKey", out string rsaPublicKey))
            {
                throw new BusinessException($"缺少rsaPublicKey");
            }
            if (!licenseInfo.TryGetValue("expTimeSpan", out string expTimeSpanStr))
            {
                throw new BusinessException($"缺少expTimeSpan");
            }
            if (!TimeSpan.TryParse(expTimeSpanStr,out TimeSpan expTimeSpan))
            {
                throw new BusinessException($"expTimeSpan参数值格式不正确");
            }

            licenseInfo.Remove("rsaPrivateKey");
            licenseInfo.Remove("rsaPublicKey");
            licenseInfo.Add(ApplicationLicensingService.publicKeySignKeyName, HashHelper.Md5($"{rsaPublicKey}shengyu"));
            var hander = new JsonWebTokenHandler();
            var tokenDes = new SecurityTokenDescriptor() { Expires=DateTime.UtcNow.Add(expTimeSpan) };
            tokenDes.Claims = new Dictionary<string, object>();
            foreach (var item in licenseInfo)
            {
                tokenDes.Claims.Add(new KeyValuePair<string, object>(item.Key, item.Value));
            }
            tokenDes.SigningCredentials = new SigningCredentials(new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(rsaPrivateKey)), SecurityAlgorithms.RsaSha256);
            var token = hander.CreateToken(tokenDes);
            return token;
        }

        /// <summary>
        /// 获取机器码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetComputerFinger()
        {
            return ComputerFinger.GetFinger();
        }
    }
}
