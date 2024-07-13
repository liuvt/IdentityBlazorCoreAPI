using IdentityBlazorCoreAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityBlazorCoreAPI.Data;

public partial class IdentityBlazorCoreAPIDbContext : IdentityDbContext<AppUser>
{
    //Default constructor
    public IdentityBlazorCoreAPIDbContext()
    {

    }

    //Constructor with parameter
    public IdentityBlazorCoreAPIDbContext(DbContextOptions<IdentityBlazorCoreAPIDbContext> options) : base(options)
    {
        //Models - Etityties
    }

    //Config to connection mysql server
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //This my Connection string to mysql server
        string myLocalhost = "server=localhost;user=root;password=123456aA@;Port=3306;database=BManagerDatabase";
        string hosting = "server=sql.freedb.tech;user=freedb_adminmanagerbusiness;password=DRjd#vxe&%y5TRZ;Port=3306;database=freedb_managerbusinessdatabase";

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(myLocalhost, ServerVersion.Parse("8.0.31-mysql"));
        }
    }

    //Data seeding
    protected override async void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Seeding Data bảng Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole[] {
                    new IdentityRole
                    {
                        Id = "1",
                        Name = "Owner",
                        NormalizedName = """Owner""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
                    new IdentityRole
                    {
                        Id = "2",
                        Name = "Administrator",
                        NormalizedName = """Administructor""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
                    new IdentityRole
                    {
                        Id = "3",
                        Name = "Manager",
                        NormalizedName = """Manager""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
                    new IdentityRole
                    {
                        Id = "4",
                        Name = "Seller",
                        NormalizedName = """Seller""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
                    new IdentityRole
                    {
                        Id = "5",
                        Name = "Buyer",
                        NormalizedName = """Buyer""".ToUpper(),
                        ConcurrencyStamp = Convert.ToString(DateTime.Now)
                    },
            }
        );
        //Seeding Data bảng User
        var passwordHasher = new PasswordHasher<AppUser>();
        //Owner
        var userOwner = new AppUser
        {
            Id = "OWNER-AUGCENTER-2023",
            UserName = "owner@augcenter.com",
            Email = "owner@augcenter.com",
            NormalizedUserName = "OWNER@AUGCENTER.COM",
            FirstName = "Aug",
            LastName = "Center",
            NormalizedEmail = "OWNER@AUGCENTER.COM",
            PhoneNumber = "0868752251",
            LockoutEnabled = false
        };
        userOwner.PasswordHash = passwordHasher.HashPassword(userOwner, "Owner@123");
        //Admin
        var userAdmin = new AppUser
        {
            Id = "ADMIN-AUGCENTER-2023",
            UserName = "administructor@augcenter.com",
            Email = "administructor@augcenter.com",
            NormalizedUserName = "ADMINISTRUCTOR@AUGCENTER.COM",
            FirstName = "Aug",
            LastName = "Center",
            NormalizedEmail = "ADMINISTRUCTOR@AUGCENTER.COM",
            PhoneNumber = "0868752251",
            LockoutEnabled = false
        };
        userAdmin.PasswordHash = passwordHasher.HashPassword(userAdmin, "Admin@123");
        //Seller
        var userSeller = new AppUser
        {
            Id = "SELLER-AUGCENTER-2023",
            UserName = "seller@augcenter.com",
            Email = "seller@augcenter.com",
            NormalizedUserName = "SELLER@AUGCENTER.COM",
            FirstName = "Aug",
            LastName = "Center",
            NormalizedEmail = "SELLER@AUGCENTER.COM",
            PhoneNumber = "0868752251",
            LockoutEnabled = false
        };
        userSeller.PasswordHash = passwordHasher.HashPassword(userSeller, "Seller@123");
        //Buyer
        var userBuyer = new AppUser
        {
            Id = "BUYER-AUGCENTER-2023",
            UserName = "buyer@augcenter.com",
            Email = "buyer@augcenter.com",
            NormalizedUserName = "BUYER@AUGCENTER.COM",
            FirstName = "Aug",
            LastName = "Center",
            NormalizedEmail = "BUYER@AUGCENTER.COM",
            PhoneNumber = "0868752251",
            LockoutEnabled = false
        };
        userBuyer.PasswordHash = passwordHasher.HashPassword(userBuyer, "Buyer@123");
        modelBuilder.Entity<AppUser>().HasData(new AppUser[] { userOwner, userAdmin, userSeller, userBuyer });

        //Seeding Data bảng UserRole
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>[] {
                    new IdentityUserRole<string>
                    {
                        UserId = "OWNER-AUGCENTER-2023",
                        RoleId = "1",
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = "ADMIN-AUGCENTER-2023",
                        RoleId = "2",
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = "SELLER-AUGCENTER-2023",
                        RoleId = "4",
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = "BUYER-AUGCENTER-2023",
                        RoleId = "5",
                    }
            }
        );

        //Do something
        base.OnModelCreating(modelBuilder);
    }
}

//Create mirations: dotnet ef migrations add Init -o Data/Migrations
//Create database: dotnet ef database update
//Publish project: dotnet publish -c Release --output ./Publish IdentityBlazorCoreAPI.csproj