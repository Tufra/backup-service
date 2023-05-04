using Backup_Management_Service.Common;

namespace Backup_Management_Service.Request
{
    public class BackupScriptGenerationRequest
    {
        public Guid? UserId { get; set; }
        public string? UserLocalPath { get; set; }
        //каждый час по умолчанию
        public string Cron_Minute { get; set; } = "0";
        public string Cron_Hour { get; set; } = "*";
        public string Cron_Day { get; set; } = "*";
        public string Cron_Month { get; set; } = "*";
        public string Cron_WeekDay { get; set; } = "*";
        public string CronExpression => $"{Cron_Minute} {Cron_Hour} {Cron_Day} {Cron_Month} {Cron_WeekDay}";
    }
}
