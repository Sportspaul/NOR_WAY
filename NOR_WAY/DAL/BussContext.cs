using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace NOR_WAY.DAL
{
    [ExcludeFromCodeCoverage]
    public class Ruter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Linjekode { get; set; } // PK

        public string Rutenavn { get; set; }
        public int Startpris { get; set; }
        public int TilleggPerStopp { get; set; }
        public int Kapasitet { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Stopp
    {
        public int Id { get; set; } // PK
        public string Navn { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class RuteStopp
    {
        public int Id { get; set; } // PK
        public int StoppNummer { get; set; }
        public int MinutterTilNesteStopp { get; set; }

        // Foreign Keys
        virtual public Stopp Stopp { get; set; } // FK

        virtual public Ruter Rute { get; set; } // FK
    }

    [ExcludeFromCodeCoverage]
    public class Avganger
    {
        public int Id { get; set; } // PK
        public DateTime Avreise { get; set; }
        public int SolgteBilletter { get; set; }

        // Foreign Key
        virtual public Ruter Rute { get; set; } // FK
    }

    [ExcludeFromCodeCoverage]
    public class Ordre
    {
        public int Id { get; set; } // PK
        public string Epost { get; set; }
        virtual public Stopp StartStopp { get; set; }
        virtual public Stopp SluttStopp { get; set; }
        public int Sum { get; set; }

        // Foreign Keys
        virtual public Ruter Rute { get; set; } // FK

        virtual public Avganger Avgang { get; set; } // FK
    }

    [ExcludeFromCodeCoverage]
    public class Ordrelinjer
    {
        public int Id { get; set; }

        // Foreign Keys
        virtual public Billettyper Billettype { get; set; } // FK

        virtual public Ordre Ordre { get; set; } // FK
    }

    [ExcludeFromCodeCoverage]
    public class Billettyper
    {
        public int Id { get; set; } // PK
        public string Billettype { get; set; }

        public int Rabattsats { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Brukere
    {
        public int Id { get; set; } //PK

        public string Brukernavn { get; set; }

        public byte[] Passord { get; set; }

        public byte[] Salt { get; set; }

        public string Tilgang { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class BussContext : DbContext
    {
        public BussContext(DbContextOptions<BussContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Ruter> Ruter { get; set; }
        public DbSet<Stopp> Stopp { get; set; }
        public DbSet<RuteStopp> RuteStopp { get; set; }
        public DbSet<Avganger> Avganger { get; set; }
        public DbSet<Ordre> Ordre { get; set; }
        public DbSet<Ordrelinjer> Ordrelinjer { get; set; }
        public DbSet<Billettyper> Billettyper { get; set; }
        public DbSet<Brukere> Brukere { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // må importere pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til"viritual" på de attriuttene som ønskes å lastes automatisk (LazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}