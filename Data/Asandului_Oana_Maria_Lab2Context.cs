using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Asandului_Oana_Maria_Lab2.Models;

namespace Asandului_Oana_Maria_Lab2.Data
{
    public class Asandului_Oana_Maria_Lab2Context : DbContext
    {
        public Asandului_Oana_Maria_Lab2Context (DbContextOptions<Asandului_Oana_Maria_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Asandului_Oana_Maria_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Asandului_Oana_Maria_Lab2.Models.Customer> Customer { get; set; } = default!;
        public DbSet<Asandului_Oana_Maria_Lab2.Models.Genre> Genre { get; set; } = default!;
        public DbSet<Asandului_Oana_Maria_Lab2.Models.Author> Author { get; set; } = default!;
        public DbSet<Asandului_Oana_Maria_Lab2.Models.Order> Order { get; set; } = default!;

    }
}
