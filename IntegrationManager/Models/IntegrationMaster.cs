using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegrationManager.Models
{

    [Table("IntegrationMaster")]
    public class IntegrationMaster
    {
        [Key]
        public int Id { get; set; }

        public string IntegrationName { get; set; }
        public string IntegrationType { get; set; }

        public string Market { get; set; }
        public string LegalEntity { get; set; }
        public string Environment { get; set; }

        public string TransformationFunction { get; set; }
        public string CommonUpload { get; set; }
        public string? ProcessingLayer { get; set; }
        public string? Dynamics { get; set; }
        public string? BYOL { get; set; }
        public long FileSize { get; set; }
        public string? FDDLink { get; set; }
        public string? TDDLink { get; set; }

    }
}
