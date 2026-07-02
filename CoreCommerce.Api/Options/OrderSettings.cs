using System.ComponentModel.DataAnnotations;

namespace CoreCommerce.Api.Options
{
    public class OrderSettings
    {
        // The exact name of the section in appsettings.json
        public const string SectionName = "OrderSettings";

        [Required(AllowEmptyStrings = false, ErrorMessage = "MerchantName is a required deployment configuration.")]
        public string MerchantName { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "MaxItemsPerOrder must be strictly between 1 and 100.")]
        public int MaxItemsPerOrder { get; set; }
        public bool EnableInternationalShipping { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a valid 3-letter ISO code.")]
        public string DefaultCurrency { get; set; } = string.Empty;

    }
}
