using _3dlogyERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace _3dlogyERP.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Veritabanını oluştur
            await context.Database.MigrateAsync();

            // Kullanıcılar
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Username = "admin",
                        Email = "admin@3dlogy.com",
                        PasswordHash = BC.HashPassword("Admin123!"),
                        Role = "Admin",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "user",
                        Email = "user@3dlogy.com",
                        PasswordHash = BC.HashPassword("User123!"),
                        Role = "User",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            // Malzeme Tipleri
            if (!context.MaterialTypes.Any())
            {
                context.MaterialTypes.AddRange(
                    new MaterialType { Name = "Filament", Description = "3D Yazıcı Filamenti" },
                    new MaterialType { Name = "Reçine", Description = "3D Yazıcı Reçinesi" },
                    new MaterialType { Name = "Toz", Description = "3D Yazıcı Tozu" }
                );
            }

            // Ekipman Tipleri
            if (!context.EquipmentTypes.Any())
            {
                context.EquipmentTypes.AddRange(
                    new EquipmentType { Name = "3D Yazıcı", Description = "3D Baskı Makinesi" },
                    new EquipmentType { Name = "CNC", Description = "CNC Makinesi" },
                    new EquipmentType { Name = "Tarayıcı", Description = "3D Tarayıcı" }
                );
            }

            // Harcama Kategorileri
            if (!context.ExpenseCategories.Any())
            {
                var hammadde = new ExpenseCategory { Name = "Hammadde", Description = "Hammadde Giderleri" };
                var isletme = new ExpenseCategory { Name = "İşletme", Description = "İşletme Giderleri" };
                var personel = new ExpenseCategory { Name = "Personel", Description = "Personel Giderleri" };

                context.ExpenseCategories.AddRange(hammadde, isletme, personel);

                await context.SaveChangesAsync();

                context.ExpenseCategories.AddRange(
                    new ExpenseCategory { Name = "Filament", Description = "Filament Alımları", ParentCategory = hammadde },
                    new ExpenseCategory { Name = "Reçine", Description = "Reçine Alımları", ParentCategory = hammadde },
                    new ExpenseCategory { Name = "Elektrik", Description = "Elektrik Giderleri", ParentCategory = isletme },
                    new ExpenseCategory { Name = "Kira", Description = "Kira Giderleri", ParentCategory = isletme },
                    new ExpenseCategory { Name = "Maaş", Description = "Maaş Ödemeleri", ParentCategory = personel },
                    new ExpenseCategory { Name = "SGK", Description = "SGK Ödemeleri", ParentCategory = personel }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
