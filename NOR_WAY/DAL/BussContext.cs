using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NOR_WAY.DAL
{
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

    public class Stopp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Navn { get; set; } // PK
    }

    public class RuteStopp
    {
        public int Id { get; set; } // PK
        public int StoppNummer { get; set; }
        public int MinutterTilNesteStopp { get; set; }

        // Foreign Keys
        virtual public Stopp Stopp { get; set; } // FK
        virtual public Ruter Rute { get; set; } // FK
    }

    public class Avganger
    {
        public int Id { get; set; } // PK
        public DateTime Avreise { get; set; }
        public int SolgteBilletter { get; set; }

        // Foreign Key
        virtual public Ruter Rute { get; set; } // FK
    }

    public class Ordre
    {
        public int Id { get; set; } // PK
        public string Epost { get; set; }
        public string StartStopp { get; set; }
        public string SluttStopp { get; set; }
        public int Sum { get; set; }

        // Foreign Keys
        virtual public Ruter Rute { get; set; } // FK
        virtual public Avganger Avgang { get; set; } // FK
    }

    public class Ordrelinjer
    {
        public string Id { get; set; }

        // Foreign Keys
        virtual public Billettyper Billettype { get; set; } // FK
        virtual public Ordre Ordre { get; set; } // FK
    }

    public class Billettyper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Billettype { get; set; } // PK
        public int Rabattsats { get; set; }
    }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // må importere pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til"viritual" på de attriuttene som ønskes å lastes automatisk (LazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
