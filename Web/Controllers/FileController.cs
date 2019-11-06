using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snail.FileStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {
        /// <summary>
        /// 文件提供程序 
        /// </summary>
        public IFileProvider _fileProvider { get; set; }

        public FileController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
 
        /// <summary>
        /// 上传单个或多个文件，上传的参数有：文件、数据ID
        /// </summary>
        /// <param name="formCollection">包含dataId,文件附件</param>
        /// <returns>返回一个或多个文件的附件对象</returns>
        [HttpPost]
        public void UploadFile(IFormCollection formCollection)
        {
            if (formCollection.Files.Count < 1)
            {
                throw new BusinessException("文件不能为空");
            }

            if (!formCollection.ContainsKey("relateDataId") || !formCollection.ContainsKey("relateDataType"))
            {
                throw new BusinessException("缺少参数，relateDataId、relateDataType");
            }
            var relateDataId = formCollection["relateDataId"];
            var relateDataType = formCollection["relateDataType"];


            var result = new List<Snail.FileStore.FileInfo>();
            foreach (var formCollectionFile in formCollection.Files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    formCollectionFile.CopyTo(memoryStream);
                    var fileName = formCollectionFile.FileName;//文件名，和上传的文件原始名字一样
                    var fileAliasName = formCollectionFile.FileName;//文件别名，和上传的文件原始名字一样
                    var fileInfo = new Snail.FileStore.FileInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FileData = memoryStream.ToArray(),
                        FileName = formCollectionFile.FileName,
                        FileSuffix=Path.GetExtension(formCollectionFile.FileName),
                        Length=memoryStream.Length,
                        RelateDataId= relateDataId,
                        RelateDataType= relateDataType
                    };
                    _fileProvider.Add(fileInfo);
                }
            }
        }


        /// <summary>
        /// 根据数据ID，获取此数据的附件文件的基本信息
        /// </summary>
        /// <param name="dataId">数据id</param>
        /// <returns>此数据的附件文件的基本信息列表</returns>
        [HttpGet]
        public List<Snail.FileStore.FileInfo> GetFileInfos(string relateDataType,string relateDataId)
        {
            return _fileProvider.GetFileStore().GetRelateDataFileInfo(relateDataType, relateDataId);
        }


        /// <summary>
        /// 根据ID，下载文件
        /// </summary>
        /// <param name="id">id，为accessory的id</param>
        /// <returns></returns>
        [HttpGet]
        public FileStreamResult GetFile(string id)
        {
            var fileInfo = _fileProvider.Get(id);
            return File(new MemoryStream(fileInfo.FileData), "application/octet-stream",
                    fileInfo.FileName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ids">要删除的文件的id,为accessory的id</param>
        /// <returns></returns>
        [HttpDelete]
        public void DeleteFiles(List<string> ids)
        {
            ids.ForEach(id =>
            {
                _fileProvider.GetFileStore().Delete(id);
            });
        }
    }
}
