using System.ComponentModel.DataAnnotations;

namespace PackageCopycat.Models.DBModels
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }
        [DataType("nvarchar(256)")]
        public string ClientName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueId { get; set; }
    }
}
