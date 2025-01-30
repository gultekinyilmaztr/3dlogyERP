using _3dlogyERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BC = BCrypt.Net.BCrypt;

namespace _3dlogyERP.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

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
                await context.SaveChangesAsync();
            }

            // Malzemeler
            if (!context.Materials.Any())
            {
                var materialTypes = await context.MaterialTypes.ToListAsync();

                context.Materials.AddRange(
                     new Material
                     {
                         Name = "PLA Filament",
                         Brand = "Ultimaker",
                         MaterialTypeId = materialTypes.First(mt => mt.Name == "Filament").Id,
                         Color = "Beyaz",
                         UnitCost = 450.00m,
                         CurrentStock = 50,
                         MinimumStock = 10,
                         SKU = "FIL-PLA-001",
                         Location = "Depo-A1",
                         IsActive = true,
                         BatchNumber = "BATCH001",
                         Specifications = "Çap: 1.75mm, Sıcaklık: 180-220°C, Yoğunluk: 1.24 g/cm³"
                     },
        new Material
        {
            Name = "PETG Filament",
            Brand = "Prusament",
            MaterialTypeId = materialTypes.First(mt => mt.Name == "Filament").Id,
            Color = "Siyah",
            UnitCost = 520.00m,
            CurrentStock = 30,
            MinimumStock = 5,
            SKU = "FIL-PETG-001",
            Location = "Depo-A2",
            IsActive = true,
            BatchNumber = "BATCH002",
            Specifications = "Çap: 1.75mm, Sıcaklık: 230-250°C, Yoğunluk: 1.27 g/cm³"
        },
        new Material
        {
            Name = "Standard Reçine",
            Brand = "Formlabs",
            MaterialTypeId = materialTypes.First(mt => mt.Name == "Reçine").Id,
            Color = "Gri",
            UnitCost = 2800.00m,
            CurrentStock = 10,
            MinimumStock = 2,
            SKU = "RES-STD-001",
            Location = "Depo-B1",
            IsActive = true,
            BatchNumber = "BATCH003",
            Specifications = "Viskozite: 850-900 cPs @ 25°C, Yoğunluk: 1.09-1.12 g/cm³, Dalga Boyu: 405nm"
        }
    );
                await context.SaveChangesAsync();
            }

            // Ekipman Tipleri
            if (!context.EquipmentTypes.Any())
            {
                context.EquipmentTypes.AddRange(
                    new EquipmentType { Name = "3D Yazıcı", Description = "3D Baskı Makinesi" },
                    new EquipmentType { Name = "CNC", Description = "CNC Makinesi" },
                    new EquipmentType { Name = "Tarayıcı", Description = "3D Tarayıcı" }
                );
                await context.SaveChangesAsync(); // Ekipman tiplerini kaydet
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
                await context.SaveChangesAsync();
            }
        }
    }
}