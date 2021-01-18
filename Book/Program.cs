using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Book
{
    class Program
    {
        public class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int AuthorId { get; set; }
            public List<BookReview> BookReviews { get; set; }
        }
        public class BookReview
        {
            public int Id { get; set; }
            public string ReviewerName { get; set; }
            public int Rating { get; set; }
            public Book Book { get; set; }
        }
        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class BooksContext : DbContext
        {
            public DbSet<Book> Books { get; set; }
            public DbSet<BookReview> BookReviews { get; set; }
            public DbSet<Author> Authors { get; set; }
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

                modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired().HasMaxLength(512);
                modelBuilder.Entity<BookReview>().Property(r => r.ReviewerName).IsRequired();
                modelBuilder.Entity<Author>().Property(a => a.Name).IsRequired();

            }
        }

        public class AuthorRepository
        {
            public List<Author> GetAuthorsByRating(int Rating)
            {
                using (BooksContext ctx = new BooksContext())
                {
                    List<Author> authors = new List<Author>();
                    var a = ctx.Authors.Join(ctx.Books,
                        f => f.Id,
                        b => b.AuthorId,
                        (f, b) => new
                        {
                            Id = f.Id,
                            Name = f.Name,
                            BR = b.BookReviews.Average(p => p.Rating)
                        });
                    foreach (var br in a)
                    {
                        var res = br.BR;
                        if (res >= Rating)

                        {
                            Author author = new Author()
                            {
                                Id = br.Id,
                                Name = br.Name
                            };
                            authors.Add(author);
                        }

                    }
                    return authors;
                }
            }
        }
        static void Main(string[] args)
        {
            var repo = new AuthorRepository();
            var authors = repo.GetAuthorsByRating(3);
            foreach (var author in authors)
                Console.WriteLine(author.Name);
        }
    }
}
