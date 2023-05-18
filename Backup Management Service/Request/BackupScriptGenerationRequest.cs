using System.ComponentModel.DataAnnotations;
using Backup_Management_Service.Common;

namespace Backup_Management_Service.Request
{
    public class BackupScriptGenerationRequest
    {
        public Guid UserId { get; set; }

        [Required] 
        public string BackupName { get; set; } = null!;

        /// <summary>
        /// Где находится то, что нужно сохранить
        /// </summary>
        [Required]
        public string UserLocalPath { get; set; } = null!;

        /// <summary>
        /// Куда сохранить созданный бэкап
        /// </summary>
        [Required]
        public string UserBackupStoragePath { get; set; } = null!;

        /// <value>
        /// Каждый час по умолчанию
        /// </value>
        [Required]
        public string CronExpression => $"{CronMinute} {CronHour} {CronDay} {CronMonth} {CronWeekDay}";

        [Required] 
        public bool TransferFile { get; set; } = true;

        [Required] 
        public bool KeepFile { get; set; } = false;

        #region Cron properties

        public string CronMinute { get; set; } = "0";
        public string CronHour { get; set; } = "*";
        public string CronDay { get; set; } = "*";
        public string CronMonth { get; set; } = "*";
        public string CronWeekDay { get; set; } = "*";

        #endregion
    }
}