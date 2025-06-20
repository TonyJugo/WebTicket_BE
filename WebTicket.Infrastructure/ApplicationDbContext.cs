﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebTicket.Domain.Constants;
using WebTicket.Domain.Entities;

namespace WebTicket.Infrastructure;

//khóa chính là string
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<string>, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<University> Universities { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //quy định độ dài của các trường trong bảng User
        builder.Entity<User>()
            .Property(u => u.FirstName).HasMaxLength(256);
        
        builder.Entity<User>()
            .Property(u => u.LastName).HasMaxLength(256);
        //Seed data mặc định vào bảng university
        builder.Entity<University>()
            .HasData(new List<University>
            {
                new University
                {
                    Id = "Uni0001", // Id là khóa chính, có thể là Guid hoặc string
                    Name = "Đại học FPT",
                },
                new University
                {
                    Id = "Uni0002",
                    Name = "Đại học Bách Khoa",
                },
                new University
                {
                    Id = "Uni0003",
                    Name = "Đại học Khoa Học Tự Nhiên",
                },
                new University
                {
                    Id = "Uni0004",
                    Name = "Đại học Công Nghệ Thông Tin",
                }
            });

        //Seed data mặc định vào bảng asp.net role
        builder.Entity<IdentityRole<string>>()
            .HasData(new List<IdentityRole<string>>
            {
                new IdentityRole<string>()
                {
                    Id = IdentityRoleConstants.AdminRoleString,
                    Name = IdentityRoleConstants.Admin,
                    NormalizedName = IdentityRoleConstants.Admin.ToUpper()
                },
                new IdentityRole<string>()
                {
                    Id = IdentityRoleConstants.ModeratorRoleString,
                    Name = IdentityRoleConstants.Moderator,
                    NormalizedName = IdentityRoleConstants.Moderator.ToUpper()
                },

                new IdentityRole<string>()
                {
                    Id = IdentityRoleConstants.OrganizerRoleString,
                    Name = IdentityRoleConstants.Organizer,
                    NormalizedName = IdentityRoleConstants.Organizer.ToUpper()
                },

                new IdentityRole<string>()
                {
                    Id = IdentityRoleConstants.StaffRoleString,
                    Name = IdentityRoleConstants.Staff,
                    NormalizedName = IdentityRoleConstants.Staff.ToUpper()
                },

                new IdentityRole<string>()
                {
                    Id = IdentityRoleConstants.UserRoleString,
                    Name = IdentityRoleConstants.User,
                    NormalizedName = IdentityRoleConstants.User.ToUpper()
                }

            });
        builder.Entity<User>()
            .HasOne(u => u.University)
            .WithMany(u => u.Users)
            .HasForeignKey(u => u.UniversityId)
            .OnDelete(DeleteBehavior.SetNull); 


    }
}