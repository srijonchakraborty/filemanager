using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;



namespace FileManagerAPI.Common
{
    public class AppSettings
    {
        [Required]
        public string DefaultDB { get; set; }

        /// <summary>
        /// For Jwt Token
        /// </summary>
        [Required]
        public string CryptoKey { get; set; }

        [Required]
        public string UploadFolderPathSilverlightDocument { get; set; }
        [Required]
        public string DownloadFolderPathSilverlightDocument { get; set; }

        [Required]
        public string UploadFolder { get; set; }
        public string UploadDIFolder { get; set; }
        public string UploadTIFolder { get; set; }

        public string UploadCIFolder { get; set; }
        public string ProfileImageFolder { get; set; }
        public string DownloadFolder { get; set; }

        [Required]
        public string ReportFolder { get; set; }

        [Required]
        public string ErrorFolder { get; set; }

        /// <summary>
        /// For user password
        /// </summary>
        [Required]
        public string PwdSecret { get; set; }

        public string BaseKey { get; set; }

        /// <summary>
        /// For SMS decryption key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// For SMS api url
        /// </summary>
        public string SmsApiUrl { get; set; }

        /// <summary>
        /// For SMS encription
        /// </summary>
        public string SmsAccessInfo { get; set; }
        public string EmailUser { get; set; }
        public int EmailPort { get; set; }
        public bool EmailEnableSsl { get; set; }
        public string EmailHost { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailSenderIp { get; set; }
        public string EmailSenderId { get; set; }
        public string EmailSenderPwd { get; set; }
        public string EmailBcc { get; set; }
        public string CorsSetting { get; set; }


        public string OTPExpireTimeInMin { get; set; }

        public string SiteRef { get; set; }
        public string FbApiKey { get; set; }
        public string FbVapidKey { get; set; }
        public string FbProjectId { get; set; }
        public string FbSenderId { get; set; }
        public string FbStorageBucket { get; set; }
        public string FbAuthDomain { get; set; }
        public string FbDatabaseURL { get; set; }
        public string FirebaseUrl { get; set; }
        public string FbServerKey { get; set; }
        public string AppUrl { get; set; }

        public ADSettings ADConfig { get; set; }

        [Required]
        public ApiSettings API { get; set; }

        [Required]
        public Swagger Swagger { get; set; }

        [Required]
        public List<DbConnectionNode> DbConfig { get; set; }
        public DbConnectionNode DefaultConnection
        {
            get
            {
                if (this.DbConfig == null || this.DbConfig.Count <= 0 || string.IsNullOrEmpty(this.DefaultDB))
                    return null;

                return this.DbConfig.Where(x => x.ConnectionNode.Key == this.DefaultDB).FirstOrDefault();
            }
        }

        public DbConnectionNode GetConnectionNode(string key)
        {
            if (string.IsNullOrEmpty(key))
                key = this.DefaultDB;

            if (this.DbConfig == null || this.DbConfig.Count <= 0 || string.IsNullOrEmpty(key))
                return null;

            return this.DbConfig.Where(x => x.ConnectionNode.Key == key).FirstOrDefault();
        }
    }

    public class DbConnectionNode
    {
        public ConnectionNode ConnectionNode { get; set; }
    }

    public class ADSettings
    {
        public bool Enabled { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
    }

    public class ApiSettings
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ApiContact Contact { get; set; }

        public string TermsOfServiceUrl { get; set; }

        public ApiLicense License { get; set; }
    }

    public class ApiContact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }

    public class ApiLicense
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Swagger
    {
        [Required]
        public bool Enabled { get; set; }
    }

    public class ConnectionNode
    {
        public string Key { get; set; }
        public string Provider { get; set; }
        public string ConnectionString { get; set; }
        public string SqlSyntax { get; set; }

    }

    public static class SettingsExtensions
    {
        public static bool IsValid<T>(this T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var validationResult = new List<ValidationResult>();
            var result = Validator.TryValidateObject(data, new ValidationContext(data), validationResult, false);

            if (!result)
            {
                foreach (var item in validationResult)
                {
                    Debug.WriteLine($"ERROR::{item.MemberNames}:{item.ErrorMessage}");
                }
            }

            return result;
        }
    }
}
