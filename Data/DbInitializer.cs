using Asandului_Oana_Maria_Lab2.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Asandului_Oana_Maria_Lab2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Asandului_Oana_Maria_Lab2Context
           (serviceProvider.GetRequiredService
            <DbContextOptions<Asandului_Oana_Maria_Lab2Context>>()))
            {
                if (context.Book.Any())
                {
                    return; // BD a fost creata anterior
                }

                var authors = new Author[]
                   {
                      new Author { FirstName = "Mihail", LastName = "Sadoveanu" },
                      new Author { FirstName = "George", LastName = "Calinescu" },
                      new Author { FirstName = "Mircea", LastName = "Eliade" }
                      };

                context.Book.AddRange(
                       new Book
               {
                          Title = "Baltagul",
                          Price = 22m,
                          AuthorID = authors[0].ID
                         },
                        new Book
                         {
                           Title = "Enigma Otiliei",
                           Price = 18m,
                           AuthorID = authors[1].ID
                             },
                            new Book
                                {
                               Title = "Maytrei",
                                Price = 27m,
                               AuthorID = authors[2].ID
                              }
                       );

                context.SaveChanges();

                context.Genre.AddRange(
               new Genre { Name = "Roman" },
               new Genre { Name = "Nuvela" },
               new Genre { Name = "Poezie" }
                );
                context.Customer.AddRange(
                new Customer
                {
                    Name = "Popescu Marcela",
                    Adress = "Str. Plopilor, nr. 24",
                    BirthDate = DateTime.Parse("1979-09-01")
                },
                new Customer
                {
                    Name = "Mihailescu Cornel",
                    Adress = "Str. Bucuresti, nr.45, ap. 2",BirthDate=DateTime.Parse("1969 - 07 - 08")}
               
                );

                context.SaveChanges();
            }
        }
    }
}