namespace Komis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wyposazenie")]
    public partial class Wyposazenie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wyposazenie()
        {
            Samochody = new HashSet<Samochody>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_wyposazenia { get; set; }

        [Required]
        [DisplayName("Nazwa wyposa¿enia")]
        [StringLength(50)]
        public string Nazwa_wyposazenia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Samochody> Samochody { get; set; }
    }
}
