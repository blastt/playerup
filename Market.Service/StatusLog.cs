using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service
{

    public interface IStatusLogService
    {
        IEnumerable<StatusLog> GetStatusLogs();
        StatusLog GetStatusLog(int id);
        void CreateStatusLog(StatusLog message);
        void SaveStatusLog();
    }

    public class StatusLogService : IStatusLogService
    {
        private readonly IStatusLogRepository statusLogsRepository;
        private readonly IUnitOfWork unitOfWork;

        public StatusLogService(IStatusLogRepository statusLogsRepository, IUnitOfWork unitOfWork)
        {
            this.statusLogsRepository = statusLogsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IStatusLogService Members

        public IEnumerable<StatusLog> GetStatusLogs()
        {
            var statusLogs = statusLogsRepository.GetAll();
            return statusLogs;
        }


        public StatusLog GetStatusLog(int id)
        {
            var statusLog = statusLogsRepository.GetById(id);
            return statusLog;
        }


        public void CreateStatusLog(StatusLog statusLog)
        {
            statusLogsRepository.Add(statusLog);
        }

        public void SaveStatusLog()
        {
            unitOfWork.Commit();
        }

        #endregion

    }

}
