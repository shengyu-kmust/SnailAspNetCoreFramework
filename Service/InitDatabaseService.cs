using ApplicationCore.IServices;
using Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class InitDatabaseService:IService
    {
        private AppDbContext _db;
        private ILogger _logger;
        public InitDatabaseService(AppDbContext db, ILogger<InitDatabaseService> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public void Invoke()
        {

        }
    }
}
