namespace Komis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zdjecia")]
    public partial class Zdjecia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_zdjecia { get; set; }

        public int? Id_samochodu { get; set; }

        [StringLength(50)]
        public string Tytul { get; set; }

        [StringLength(50)]
        public string Nazwa_zdjecia { get; set; }

        public byte[] ImageData { set; get; }

        public virtual Samochody Samochody { get; set; }
    }
}
