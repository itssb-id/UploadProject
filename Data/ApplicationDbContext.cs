using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UploadProject.Helper;
using UploadProject.Models;

namespace UploadProject.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=app.db");
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Username = "admin",
                Password = "WeLoveITSSBIndonesia",
                IsAdmin = true
            },
            new User
            {
                Username = "admin2",
                Password = "WeLoveITSSBIndonesia",
                IsAdmin = true
            });

        var userCount = 50;
        Guid id = Guid.NewGuid();
        for (int i = 1; i <= userCount; i++)
        {
            //if(i == 1)
            //{
            //    id = new Guid("00000000 - 0000 - 0000 - 0000 - 000000000000");
            //}

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    //ID = id,
                    Username = $"PC{i.ToString().PadLeft(2, '0')}",
                    Password = PasswordGenerator.GetRandomPassword(),
                    IsAdmin = false
                });
        }

        //var idSessionDesktop = Guid.NewGuid();
        //var idSessionMobile = Guid.NewGuid();

        //modelBuilder.Entity<CompetitionSession>().HasData(new CompetitionSession
        //{ 
        //    //ID = idSessionDesktop,
        //    DayNumber = 1,
        //    Name = "Desktop 1",
        //    StartDateTime = new DateTime(2023, 05, 23, 12, 30, 0),
        //    EndDateTime = new DateTime(2023, 05, 23, 15, 30, 0)
        //});

        //modelBuilder.Entity<CompetitionSession>().HasData(new CompetitionSession
        //{
        //    DayNumber = 2,
        //    Name = "Desktop 2",
        //    StartDateTime = new DateTime(2023, 05, 24, 8, 30, 0),
        //    EndDateTime = new DateTime(2023, 05, 24, 11, 0, 0)
        //}); 

        //modelBuilder.Entity<CompetitionSession>().HasData(new CompetitionSession
        //{
        //    //ID = idSessionMobile,
        //    DayNumber = 2,
        //    Name = "Android",
        //    StartDateTime = new DateTime(2023, 05, 24, 12, 00, 00),
        //    EndDateTime = new DateTime(2023, 05, 24, 16, 00, 00)
        //});

        //modelBuilder.Entity<CompetitorUploadedFile>().HasData(
        //    new CompetitorUploadedFile
        //    {
        //        UserID = id,
        //        FileName = "BooKu.zip",
        //        CompetitionSessionID = idSessionDesktop
        //    },
        //    new CompetitorUploadedFile{
        //        UserID = id,
        //        FileName = "WSC Special Edition 2022 - Session 5.zip",
        //        CompetitionSessionID = idSessionMobile
        //    });

    }

    public virtual DbSet<User> Users { get; set; } = null!;

    public virtual DbSet<AdminUploadedFile> AdminUploadedFiles { get; set; } = null!;

    public virtual DbSet<CompetitorUploadedFile> CompetitorUploadedFiles { get; set; } = null!;

    public virtual DbSet<CompetitionSession> CompetitionSessions { get; set; } = null!;
}