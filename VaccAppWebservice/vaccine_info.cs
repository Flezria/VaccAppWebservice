namespace VaccAppWebservice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vaccine_info
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vaccine_info()
        {
            vaccinations = new HashSet<vaccinations>();
        }

        [Key]
        public int vaccineinfo_id { get; set; }

        [Required]
        [StringLength(20)]
        public string name { get; set; }

        [Required]
        public string general_info { get; set; }

        [Required]
        [StringLength(15)]
        public string injection_spot { get; set; }

        [Required]
        public string side_effects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vaccinations> vaccinations { get; set; }
    }
}
