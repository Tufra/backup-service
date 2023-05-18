using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Backup_Management_Service.Common;

namespace Backup_Management_Service.Request
{
    public class BackupScriptGenerationRequest
    {
        public Guid UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty backup name")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BackupName { get; set; } = "";

        /// <summary>
        /// Где находится то, что нужно сохранить
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty source path")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserLocalPath { get; set; } = "";

        /// <summary>
        /// Куда сохранить созданный бэкап
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserBackupStoragePath { get; set; } = "";

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