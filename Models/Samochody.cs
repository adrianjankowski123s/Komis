namespace Komis
{
    using Komis.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Samochody")]
    public partial class Samochody
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Samochody()
        {
            Zdjecia = new HashSet<Zdjecia>();
            Wyposazenie1 = new HashSet<Wyposazenie>();
        }
 

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_samochodu { get; set; }

        [DisplayName("Tytu³*")]
        [Required]
        [StringLength(50)]
        public string Tytul { get; set; }

        [DisplayName("Podtytu³*")]
        [Required]
        [StringLength(50)]
        public string Podtytul { get; set; }

        public int? Rok { get; set; }

        [DisplayName("Przebieg(km)")]
        public int? Przebieg { get; set; }

        [DisplayName("Pojemnoœæ(cm3)")]
        public int? Pojemnosc { get; set; }

        [DisplayName("Rodzaj paliwa")]
        [StringLength(200)]
        public string Rodzaj_paliwa { get; set; }

        [StringLength(200)]
        public string Kategoria { get; set; }


        [DisplayName("Marka*")]
        [Required]
        [StringLength(200)]
        public string Marka { get; set; }


        [DisplayName("Model*")]
        [Required]
        [StringLength(200)]
        public string Model { get; set; }

        [StringLength(200)]
        public string Wersja { get; set; }

        [StringLength(200)]
        public string Generacja { get; set; }

        [DisplayName("Moc(KM)")]
        [StringLength(200)]
        public string Moc { get; set; }

        [DisplayName("Skrzynia biegów")]
        [StringLength(200)]
        public string Skrzynia_biegow { get; set; }

        [DisplayName("Napêd")]
        [StringLength(200)]
        public string Naped { get; set; }

        [StringLength(200)]
        public string Vin { get; set; }

        [DisplayName("Emisja CO2")]
        [StringLength(200)]
        public string Emisja_co2 { get; set; }

        [StringLength(200)]
        public string Typ { get; set; }

        [DisplayName("Iloœæ drzwi")]
        public int? Liczba_drzwi { get; set; }

        [DisplayName("Liczba miejsc")]
        public int? Liczba_miejsc { get; set; }

        [StringLength(200)]
        public string Kolor { get; set; }

        [DisplayName("Data pierwszej rejestracji")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? Pierwsza_rejestracja { get; set; }

        [DisplayName("Numer rejestracyjny")]
        [StringLength(200)]
        public string Numer_rejestracyjny { get; set; }

        [DisplayName("Czy pojazd jest zarejestrowany w Polsce?")]
        [StringLength(200)]
        public string Zarejestrowany_w_polsce { get; set; }

        [StringLength(200)]
        public string Stan { get; set; }

        [DisplayName("Kraj pochodzenia")]
        [StringLength(200)]
        public string Kraj_pochodzenia { get; set; }

        [StringLength(200)]
        public string Opis { get; set; }

        [DisplayName("Wyposa¿enie")]
        [StringLength(200)]
        public string Wyposazenie { get; set; }


        [DisplayName("Cena(z³)")]
        public int? Kwota { get; set; }

        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zdjecia> Zdjecia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wyposazenie> Wyposazenie1 { get; set; }

       
    }

}
