using Microsoft.EntityFrameworkCore;
using NoteApi.Models;

namespace NoteApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {   Database.EnsureCreated(); }

        public DbSet<Note> Notes { get; set; }
    }
}