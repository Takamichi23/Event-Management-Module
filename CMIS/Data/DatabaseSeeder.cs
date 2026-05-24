using CMIS.Data;
using CMIS.Models;
using CMIS.Services;
using Microsoft.EntityFrameworkCore;

namespace CMIS.Data;

public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds initial data: Roles, a District, a Church, a Profile, and a test Account.
    /// If data already exists, migrates passwords to BCrypt format.
    /// </summary>
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (await db.Roles.AnyAsync())
        {
            // Data already exists — migrate passwords and ensure test data
            await MigratePasswordsToBCryptAsync(db);
            await EnsureTestAccountAsync(db);
            await EnsureSampleDistrictsAsync(db);
            await EnsureSampleChurchesAsync(db);
            await EnsureAllRoleAccountsAsync(db);
            await EnsureSampleProposalsAsync(db);
            await EnsureSampleTransactionsAsync(db);
            await EnsureSampleAllocationsAsync(db);
            await EnsureSampleDistrictEventsAsync(db);
            await EnsureSampleBudgetsAsync(db);
            db.ChangeTracker.Clear();
            await EnsureSampleFundsAsync(db);
            return;
        }

        // 1. Seed Roles
        var roles = new List<Role>
        {
            new() { RoleName = "Head Pastor", Description = "Senior pastor overseeing all church operations" },
            new() { RoleName = "District Head", Description = "Oversees a district of churches" },
            new() { RoleName = "Ministry Head", Description = "Leads a specific ministry" },
            new() { RoleName = "Board of Directors", Description = "Board member with governance authority" },
            new() { RoleName = "Leadership Council", Description = "Member of the leadership council" },
            new() { RoleName = "Member", Description = "Regular church member" },
        };
        db.Roles.AddRange(roles);
        await db.SaveChangesAsync();

        // 2. Seed a District
        var district = new District
        {
            DistrictName = "Metro Manila District",
            DistrictCode = "MMD-001",
            Address = "Metro Manila, Philippines",
            Status = "Active"
        };
        db.Districts.Add(district);
        await db.SaveChangesAsync();

        // 3. Seed a Church
        var church = new Church
        {
            DistrictId = district.DistrictId,
            ChurchName = "Jesus Our Banner Ministry - Main",
            Address = "Manila, Philippines",
            ContactNumber = "09171234567",
            Status = "Active"
        };
        db.Churches.Add(church);
        await db.SaveChangesAsync();

        // 4. Seed a Profile
        var profile = new Profile
        {
            ChurchId = church.ChurchId,
            FirstName = "Admin",
            MiddleName = null,
            LastName = "User",
            Sex = "Male",
            BirthDate = new DateTime(1990, 1, 1),
            ContactNumber = "09171234567",
            Address = "Manila, Philippines",
            ProfileStatus = "Active"
        };
        db.Profiles.Add(profile);
        await db.SaveChangesAsync();

        // 5. Seed a test Account (Head Pastor role)
        var headPastorRole = roles.First(r => r.RoleName == "Head Pastor");
        var account = new Account
        {
            ProfileId = profile.ProfileId,
            RoleId = headPastorRole.RoleId,
            ChurchId = church.ChurchId,
            Username = "admin",
            Email = "admin@jobm.org",
            PasswordHash = AuthService.HashPassword("Admin@123"),
            Status = "Active"
        };
        db.Accounts.Add(account);
        await db.SaveChangesAsync();

        // 6. Seed additional districts and churches
        await EnsureSampleDistrictsAsync(db);
        await EnsureSampleChurchesAsync(db);

        // 7. Seed all role-based accounts
        await EnsureAllRoleAccountsAsync(db);

        // 8. Seed sample data
        await EnsureSampleProposalsAsync(db);
        await EnsureSampleTransactionsAsync(db);
        await EnsureSampleAllocationsAsync(db);
        await EnsureSampleBudgetsAsync(db);
        await EnsureSampleFundsAsync(db);
    }

    /// <summary>
    /// Finds existing accounts whose password_hash is NOT in BCrypt format
    /// and re-hashes the current value as the "plain text" password.
    /// 
    /// BCrypt hashes always start with "$2a$", "$2b$", or "$2y$".
    /// If the stored value doesn't match this pattern, it's treated as a
    /// plain-text (or non-BCrypt) password and gets re-hashed.
    /// </summary>
    private static async Task MigratePasswordsToBCryptAsync(ApplicationDbContext db)
    {
        var accounts = await db.Accounts.ToListAsync();
        var migrated = 0;

        foreach (var account in accounts)
        {
            if (!string.IsNullOrEmpty(account.PasswordHash) &&
                !account.PasswordHash.StartsWith("$2"))
            {
                // Current value is plain text or a non-BCrypt hash — re-hash it
                account.PasswordHash = AuthService.HashPassword(account.PasswordHash);
                migrated++;
            }
        }

        if (migrated > 0)
        {
            await db.SaveChangesAsync();
            Console.WriteLine($"[Seeder] Migrated {migrated} account password(s) to BCrypt.");
        }
    }

    /// <summary>
    /// Ensures a test admin account exists with known credentials.
    /// Email: admin@jobm.org | Username: admin | Password: Admin@123
    /// </summary>
    private static async Task EnsureTestAccountAsync(ApplicationDbContext db)
    {
        // Skip if account already exists
        if (await db.Accounts.AnyAsync(a => a.Email == "admin@jobm.org"))
            return;

        // Get or create the Head Pastor role
        var role = await db.Roles.FirstOrDefaultAsync(r => r.RoleName == "Head Pastor");
        if (role is null)
        {
            role = new Role { RoleName = "Head Pastor", Description = "Senior pastor overseeing all church operations" };
            db.Roles.Add(role);
            await db.SaveChangesAsync();
        }

        // Get or create a district
        var district = await db.Districts.FirstOrDefaultAsync();
        if (district is null)
        {
            district = new District
            {
                DistrictName = "Default District",
                DistrictCode = "DEF-001",
                Address = "Philippines",
                Status = "Active"
            };
            db.Districts.Add(district);
            await db.SaveChangesAsync();
        }

        // Get or create a church
        var church = await db.Churches.FirstOrDefaultAsync();
        if (church is null)
        {
            church = new Church
            {
                DistrictId = district.DistrictId,
                ChurchName = "Jesus Our Banner Ministry - Main",
                Address = "Philippines",
                ContactNumber = "09170000000",
                Status = "Active"
            };
            db.Churches.Add(church);
            await db.SaveChangesAsync();
        }

        // Create a profile for the admin
        var profile = new Profile
        {
            ChurchId = church.ChurchId,
            FirstName = "Admin",
            LastName = "User",
            Sex = "Male",
            BirthDate = new DateTime(1990, 1, 1),
            ProfileStatus = "Active"
        };
        db.Profiles.Add(profile);
        await db.SaveChangesAsync();

        // Create the account
        var account = new Account
        {
            ProfileId = profile.ProfileId,
            RoleId = role.RoleId,
            ChurchId = church.ChurchId,
            Username = "admin",
            Email = "admin@jobm.org",
            PasswordHash = AuthService.HashPassword("Admin@123"),
            Status = "Active"
        };
        db.Accounts.Add(account);
        await db.SaveChangesAsync();

        Console.WriteLine("[Seeder] Test account created — Email: admin@jobm.org | Password: Admin@123");
    }

    /// <summary>
    /// Seeds 3 sample budget proposals matching the design screenshot.
    /// </summary>
    private static async Task EnsureSampleProposalsAsync(ApplicationDbContext db)
    {
        if (await db.BudgetProposals.AnyAsync())
            return;

        // Ensure we have ministries to link to
        var ministryNames = new[] { "Youth Ministry", "Music Ministry", "Outreach Ministry" };
        foreach (var name in ministryNames)
        {
            if (!await db.Ministries.AnyAsync(m => m.MinistryName == name))
            {
                db.Ministries.Add(new Ministry { MinistryName = name, Description = $"{name} department", Status = "Active" });
            }
        }
        await db.SaveChangesAsync();

        var youthMinistry = await db.Ministries.FirstAsync(m => m.MinistryName == "Youth Ministry");
        var musicMinistry = await db.Ministries.FirstAsync(m => m.MinistryName == "Music Ministry");
        var outreachMinistry = await db.Ministries.FirstAsync(m => m.MinistryName == "Outreach Ministry");

        // Get the admin profile as submitter (with church and district)
        var submitter = await db.Profiles.Include(p => p.Church).FirstAsync();
        var submitterChurchId = submitter.ChurchId;
        var submitterDistrictId = submitter.Church?.DistrictId;

        // Ensure a "Local Church Operations" ministry exists for Head Pastor proposals
        if (!await db.Ministries.AnyAsync(m => m.MinistryName == "Local Church Operations"))
        {
            db.Ministries.Add(new Ministry { MinistryName = "Local Church Operations", Description = "Local church operations", Status = "Active" });
            await db.SaveChangesAsync();
        }
        var localChurchMinistry = await db.Ministries.FirstAsync(m => m.MinistryName == "Local Church Operations");

        // Ensure a "District 1 Operations" ministry exists for District Head proposals
        if (!await db.Ministries.AnyAsync(m => m.MinistryName == "District 1 Operations"))
        {
            db.Ministries.Add(new Ministry { MinistryName = "District 1 Operations", Description = "District 1 operations", Status = "Active" });
            await db.SaveChangesAsync();
        }
        var districtMinistry = await db.Ministries.FirstAsync(m => m.MinistryName == "District 1 Operations");

        var proposals = new List<BudgetProposal>
        {
            new()
            {
                ProposalCode = "BP-2026-001",
                Purpose = "Summer Camp Retreat",
                Description = "Annual summer camp retreat for the youth ministry",
                MinistryId = youthMinistry.MinistryId,
                ChurchId = submitterChurchId,
                DistrictId = submitterDistrictId,
                Level = "Ministry",
                Amount = 5000m,
                Status = "Pending",
                SubmittedById = submitter.ProfileId
            },
            new()
            {
                ProposalCode = "BP-2026-002",
                Purpose = "New Audio Equipment",
                Description = "Purchase new audio equipment for worship services",
                MinistryId = musicMinistry.MinistryId,
                ChurchId = submitterChurchId,
                DistrictId = submitterDistrictId,
                Level = "Ministry",
                Amount = 12000m,
                Status = "Approved",
                SubmittedById = submitter.ProfileId
            },
            new()
            {
                ProposalCode = "BP-2026-003",
                Purpose = "Community Food Drive",
                Description = "Organize a community food drive for outreach",
                MinistryId = outreachMinistry.MinistryId,
                ChurchId = submitterChurchId,
                DistrictId = submitterDistrictId,
                Level = "Ministry",
                Amount = 3000m,
                Status = "Disapproved",
                SubmittedById = submitter.ProfileId
            },
            new()
            {
                ProposalCode = "BP-2026-004",
                Purpose = "Facility Renovation - Sanctuary",
                Description = "Major renovation of the church sanctuary",
                MinistryId = localChurchMinistry.MinistryId,
                ChurchId = submitterChurchId,
                DistrictId = submitterDistrictId,
                Level = "Local Church",
                Amount = 45000m,
                Status = "Pending",
                SubmittedById = submitter.ProfileId
            },
            new()
            {
                ProposalCode = "BP-2026-005",
                Purpose = "District Pastors Conference",
                Description = "Annual conference for district pastors",
                MinistryId = districtMinistry.MinistryId,
                ChurchId = submitterChurchId,
                DistrictId = submitterDistrictId,
                Level = "District",
                Amount = 28000m,
                Status = "Pending",
                SubmittedById = submitter.ProfileId
            }
        };

        db.BudgetProposals.AddRange(proposals);
        await db.SaveChangesAsync();
        Console.WriteLine("[Seeder] 5 sample budget proposals created.");
    }

    private static async Task EnsureSampleTransactionsAsync(ApplicationDbContext db)
    {
        if (await db.Transactions.AnyAsync())
            return;

        var profile = await db.Profiles.FirstAsync();
        var allocations = await db.BudgetAllocations.ToListAsync();
        int? IdOf(string name) => allocations.FirstOrDefault(a => a.Name == name)?.AllocationId;

        var transactions = new List<Transaction>
        {
            new() { TransactionCode = "TX-1001", Description = "Sunday Tithes and Offerings", Type = "Income", BudgetAllocationId = IdOf("Local Church Operations"), BudgetLabel = null, Amount = 15000m, RecordedById = profile.ProfileId, TransactionDate = new DateTime(2026, 5, 3) },
            new() { TransactionCode = "TX-1002", Description = "Microphone cables and stands", Type = "Expense", BudgetAllocationId = IdOf("Music Ministry"), BudgetLabel = null, Amount = 1200m, RecordedById = profile.ProfileId, TransactionDate = new DateTime(2026, 5, 5) },
            new() { TransactionCode = "TX-1003", Description = "Pizza for Friday Youth Night", Type = "Expense", BudgetAllocationId = IdOf("Youth Ministry"), BudgetLabel = null, Amount = 450m, RecordedById = profile.ProfileId, TransactionDate = new DateTime(2026, 5, 8) },
            new() { TransactionCode = "TX-1004", Description = "National Tithe Remittance", Type = "Income", BudgetAllocationId = IdOf("National Operations"), BudgetLabel = null, Amount = 75000m, RecordedById = profile.ProfileId, TransactionDate = new DateTime(2026, 4, 30) },
            new() { TransactionCode = "TX-1005", Description = "District training materials", Type = "Expense", BudgetAllocationId = IdOf("District 1"), BudgetLabel = null, Amount = 2500m, RecordedById = profile.ProfileId, TransactionDate = new DateTime(2026, 5, 1) },
        };

        db.Transactions.AddRange(transactions);
        await db.SaveChangesAsync();
        Console.WriteLine("[Seeder] 5 sample transactions created.");
    }

    private static async Task EnsureSampleAllocationsAsync(ApplicationDbContext db)
    {
        if (await db.BudgetAllocations.AnyAsync())
            return;

        var allocations = new List<BudgetAllocation>
        {
            // Ministry budgets
            new() { Name = "Youth Ministry", Category = "Ministry", Allocated = 15000m, Spent = 4500m },
            new() { Name = "Music Ministry", Category = "Ministry", Allocated = 25000m, Spent = 13200m },
            new() { Name = "Outreach Ministry", Category = "Ministry", Allocated = 20000m, Spent = 5000m },
            // Operations budgets
            new() { Name = "Local Church Operations", Category = "Operations", Allocated = 50000m, Spent = 15000m },
            new() { Name = "District 1", Category = "Operations", Allocated = 80000m, Spent = 26000m },
            new() { Name = "National Operations", Category = "Operations", Allocated = 200000m, Spent = 62400m },
            // District-level allocations
            new() { Name = "District 1", Category = "District", Allocated = 100000m, Spent = 32500m },
        };

        db.BudgetAllocations.AddRange(allocations);
        await db.SaveChangesAsync();
        Console.WriteLine("[Seeder] 6 budget allocations created.");
    }

    private static async Task EnsureSampleDistrictEventsAsync(ApplicationDbContext db)
    {
        if (await db.DistrictEvents.AnyAsync())
            return;

        var events = new List<DistrictEventModel>
        {
            new()
            {
                Title = "District Youth Conference 2026",
                Description = "A gathering of all youth groups across the district for worship and leadership training.",
                Venue = "District Convention Center",
                Date = DateTime.Today.AddDays(15),
                Category = "Youth",
                StartTime = new TimeOnly(8, 0),
                EndTime = new TimeOnly(17, 0),
                Duration = "9 Hours",
                ExpectedParticipants = 500,
                OrganizingDistrict = "Metro Manila District",
                Status = "Approved",
                FundingType = "Church Sponsored",
                ExpectedNumberOfParticipants = 500,
                AllocatedBudgetPerPerson = 100,
                DistrictProgramSchedules = new List<DistrictProgramScheduleItem>
                {
                    new() { StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 0), ProgramTitle = "Registration & Morning Worship" },
                    new() { StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0), ProgramTitle = "Leadership Workshop" }
                },
                DistrictGuests = new List<DistrictGuestItem>
                {
                    new() { GuestType = "Keynote Speaker", FullName = "Dr. Robert Smith", ContactNumber = "09123456789" }
                },
                DistrictPersonnel = new List<DistrictPersonnelItem>
                {
                    new() { RoleName = "Event Coordinator", FullName = "Sarah Johnson", ContactNumber = "09987654321" }
                }
            },
            new()
            {
                Title = "Pastors and Leaders Meeting",
                Description = "Strategic planning meeting for all district pastors and leadership council members.",
                Venue = "District Office - Conference Room",
                Date = DateTime.Today.AddDays(7),
                Category = "Fellowship",
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(16, 0),
                Duration = "2 Hours",
                ExpectedParticipants = 50,
                OrganizingDistrict = "Metro Manila District",
                Status = "Approved",
                FundingType = "Fund Raising",
                ExpectedNumberOfParticipants = 50,
                AllocatedBudgetPerPerson = 50,
                DistrictProgramSchedules = new List<DistrictProgramScheduleItem>
                {
                    new() { StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0), ProgramTitle = "Strategic Planning Session" }
                }
            },
            new()
            {
                Title = "District Outreach: Manila 2026",
                Description = "A collaborative outreach mission to reach the underprivileged communities in Manila.",
                Venue = "Various Locations",
                Date = DateTime.Today.AddDays(30),
                Category = "Outreach",
                StartTime = new TimeOnly(7, 0),
                EndTime = new TimeOnly(18, 0),
                Duration = "11 Hours",
                ExpectedParticipants = 200,
                OrganizingDistrict = "Metro Manila District",
                Status = "Planned Event",
                FundingType = "Ticketed Event",
                ExpectedNumberOfParticipants = 200,
                TicketedPrice = 150,
                AllocatedBudgetPerPerson = 120
            }
        };

        db.DistrictEvents.AddRange(events);
        await db.SaveChangesAsync();
        Console.WriteLine("[Seeder] 3 sample district events created.");
    }

    /// <summary>
    /// Seeds 5 accounts — one for each primary role.
    /// All use password: Password@123
    /// </summary>
    private static async Task EnsureAllRoleAccountsAsync(ApplicationDbContext db)
    {
        var church = await db.Churches.FirstOrDefaultAsync();
        if (church is null) return;

        var seedAccounts = new[]
        {
            new { Email = "headpastor@jobm.org", Username = "headpastor", FirstName = "Juan", LastName = "Dela Cruz", Role = "Head Pastor" },
            new { Email = "districthead@jobm.org", Username = "districthead", FirstName = "Maria", LastName = "Santos", Role = "District Head" },
            new { Email = "ministryhead@jobm.org", Username = "ministryhead", FirstName = "Pedro", LastName = "Reyes", Role = "Ministry Head" },
            new { Email = "board@jobm.org", Username = "board", FirstName = "Elena", LastName = "Garcia", Role = "Board of Directors" },
            new { Email = "council@jobm.org", Username = "council", FirstName = "Carlos", LastName = "Mendoza", Role = "Leadership Council" },
        };

        var created = 0;
        foreach (var seed in seedAccounts)
        {
            // Skip if account already exists (by email or username)
            if (await db.Accounts.AnyAsync(a => a.Email == seed.Email || a.Username == seed.Username))
                continue;

            var role = await db.Roles.FirstOrDefaultAsync(r => r.RoleName == seed.Role);
            if (role is null) continue;

            var profile = new Profile
            {
                ChurchId = church.ChurchId,
                FirstName = seed.FirstName,
                LastName = seed.LastName,
                Sex = "Male",
                BirthDate = new DateTime(1985, 6, 15),
                ContactNumber = "09170000000",
                Address = "Manila, Philippines",
                ProfileStatus = "Active"
            };
            db.Profiles.Add(profile);
            await db.SaveChangesAsync();

            var account = new Account
            {
                ProfileId = profile.ProfileId,
                RoleId = role.RoleId,
                ChurchId = church.ChurchId,
                Username = seed.Username,
                Email = seed.Email,
                PasswordHash = AuthService.HashPassword("Password@123"),
                Status = "Active"
            };
            db.Accounts.Add(account);
            await db.SaveChangesAsync();
            created++;
        }

        if (created > 0)
            Console.WriteLine($"[Seeder] {created} role-based accounts created. Password for all: Password@123");
    }

    private static async Task EnsureSampleDistrictsAsync(ApplicationDbContext db)
    {
        var districtSeeds = new[]
        {
            new { Name = "Metro Manila District",        Code = "MMD-001", Address = "Metro Manila, Philippines" },
            new { Name = "North Luzon District",         Code = "NLD-001", Address = "Pampanga, Philippines" },
            new { Name = "South Luzon & Bicol District", Code = "SLB-001", Address = "Naga City, Philippines" },
            new { Name = "Visayas District",             Code = "VSD-001", Address = "Cebu City, Philippines" },
        };

        var added = 0;
        foreach (var d in districtSeeds)
        {
            if (!await db.Districts.AnyAsync(x => x.DistrictCode == d.Code))
            {
                db.Districts.Add(new District { DistrictName = d.Name, DistrictCode = d.Code, Address = d.Address, Status = "Active" });
                added++;
            }
        }

        if (added > 0)
        {
            await db.SaveChangesAsync();
            Console.WriteLine($"[Seeder] {added} district(s) created.");
        }
    }

    private static async Task EnsureSampleChurchesAsync(ApplicationDbContext db)
    {
        var districts = await db.Districts.ToListAsync();
        int? IdOf(string code) => districts.FirstOrDefault(d => d.DistrictCode == code)?.DistrictId;

        var churchSeeds = new[]
        {
            // Metro Manila District
            new { Name = "JOBM Main",                    DistrictCode = "MMD-001", Contact = "09171234567", Address = "Manila, Philippines" },
            new { Name = "JOBM Quezon City Branch",      DistrictCode = "MMD-001", Contact = "09172345678", Address = "Quezon City, Philippines" },
            new { Name = "JOBM Makati Branch",           DistrictCode = "MMD-001", Contact = "09173456789", Address = "Makati, Philippines" },
            // North Luzon District
            new { Name = "JOBM Baguio City",             DistrictCode = "NLD-001", Contact = "09174567890", Address = "Baguio City, Philippines" },
            new { Name = "JOBM Dagupan",                 DistrictCode = "NLD-001", Contact = "09175678901", Address = "Dagupan City, Philippines" },
            // South Luzon & Bicol District
            new { Name = "JOBM Naga City",               DistrictCode = "SLB-001", Contact = "09176789012", Address = "Naga City, Philippines" },
            new { Name = "JOBM Legazpi",                 DistrictCode = "SLB-001", Contact = "09177890123", Address = "Legazpi City, Philippines" },
            // Visayas District
            new { Name = "JOBM Cebu City",               DistrictCode = "VSD-001", Contact = "09178901234", Address = "Cebu City, Philippines" },
            new { Name = "JOBM Iloilo City",             DistrictCode = "VSD-001", Contact = "09179012345", Address = "Iloilo City, Philippines" },
        };

        var added = 0;
        foreach (var c in churchSeeds)
        {
            var districtId = IdOf(c.DistrictCode);
            if (districtId == null) continue;
            if (!await db.Churches.AnyAsync(x => x.ChurchName == c.Name))
            {
                db.Churches.Add(new Church { DistrictId = districtId.Value, ChurchName = c.Name, ContactNumber = c.Contact, Address = c.Address, Status = "Active" });
                added++;
            }
        }

        if (added > 0)
        {
            await db.SaveChangesAsync();
            Console.WriteLine($"[Seeder] {added} church(es) created.");
        }
    }

    private static async Task EnsureSampleBudgetsAsync(ApplicationDbContext db)
    {
        if (await db.Budgets.AnyAsync())
            return;

        var districts = await db.Districts.ToListAsync();
        var churches  = await db.Churches.ToListAsync();

        int? DistrictId(string code) => districts.FirstOrDefault(d => d.DistrictCode == code)?.DistrictId;
        int? ChurchId(string name)   => churches.FirstOrDefault(c => c.ChurchName == name)?.ChurchId;

        var budgets = new List<Budget>
        {
            // National-level budget
            new() { Name = "National Operations Budget",          Level = "National",  ChurchId = null,                       DistrictId = null,                   FiscalYear = "2025-2026", TotalAmount = 500000m,  CreatedAt = DateTime.UtcNow },
            // District-level budgets
            new() { Name = "Metro Manila District Budget",        Level = "District",  ChurchId = null,                       DistrictId = DistrictId("MMD-001"),  FiscalYear = "2025-2026", TotalAmount = 200000m,  CreatedAt = DateTime.UtcNow },
            new() { Name = "North Luzon District Budget",         Level = "District",  ChurchId = null,                       DistrictId = DistrictId("NLD-001"),  FiscalYear = "2025-2026", TotalAmount = 150000m,  CreatedAt = DateTime.UtcNow },
            new() { Name = "South Luzon & Bicol District Budget", Level = "District",  ChurchId = null,                       DistrictId = DistrictId("SLB-001"),  FiscalYear = "2025-2026", TotalAmount = 120000m,  CreatedAt = DateTime.UtcNow },
            new() { Name = "Visayas District Budget",             Level = "District",  ChurchId = null,                       DistrictId = DistrictId("VSD-001"),  FiscalYear = "2025-2026", TotalAmount = 130000m,  CreatedAt = DateTime.UtcNow },
            // Church-level budgets
            new() { Name = "JOBM Main Annual Budget",             Level = "Church",    ChurchId = ChurchId("JOBM Main"),      DistrictId = DistrictId("MMD-001"),  FiscalYear = "2025-2026", TotalAmount = 80000m,   CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Quezon City Annual Budget",      Level = "Church",    ChurchId = ChurchId("JOBM Quezon City Branch"), DistrictId = DistrictId("MMD-001"), FiscalYear = "2025-2026", TotalAmount = 60000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Makati Annual Budget",           Level = "Church",    ChurchId = ChurchId("JOBM Makati Branch"),      DistrictId = DistrictId("MMD-001"), FiscalYear = "2025-2026", TotalAmount = 55000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Baguio City Annual Budget",      Level = "Church",    ChurchId = ChurchId("JOBM Baguio City"),        DistrictId = DistrictId("NLD-001"), FiscalYear = "2025-2026", TotalAmount = 45000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Dagupan Annual Budget",          Level = "Church",    ChurchId = ChurchId("JOBM Dagupan"),            DistrictId = DistrictId("NLD-001"), FiscalYear = "2025-2026", TotalAmount = 40000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Naga City Annual Budget",        Level = "Church",    ChurchId = ChurchId("JOBM Naga City"),          DistrictId = DistrictId("SLB-001"), FiscalYear = "2025-2026", TotalAmount = 38000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Legazpi Annual Budget",          Level = "Church",    ChurchId = ChurchId("JOBM Legazpi"),            DistrictId = DistrictId("SLB-001"), FiscalYear = "2025-2026", TotalAmount = 35000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Cebu City Annual Budget",        Level = "Church",    ChurchId = ChurchId("JOBM Cebu City"),          DistrictId = DistrictId("VSD-001"), FiscalYear = "2025-2026", TotalAmount = 70000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Iloilo City Annual Budget",      Level = "Church",    ChurchId = ChurchId("JOBM Iloilo City"),        DistrictId = DistrictId("VSD-001"), FiscalYear = "2025-2026", TotalAmount = 50000m, CreatedAt = DateTime.UtcNow },
        };

        db.Budgets.AddRange(budgets);
        await db.SaveChangesAsync();
        Console.WriteLine($"[Seeder] {budgets.Count} budgets created.");
    }

    private static async Task EnsureSampleFundsAsync(ApplicationDbContext db)
    {
        if (await db.Funds.AnyAsync())
        {
            var existingFunds = await db.Funds.ToListAsync();
            await BackfillFundBudgetLinksAsync(db, existingFunds);
            return;
        }

        var districts = await db.Districts.ToListAsync();
        var churches  = await db.Churches.ToListAsync();

        int? DistrictId(string code) => districts.FirstOrDefault(d => d.DistrictCode == code)?.DistrictId;
        int? ChurchId(string name)   => churches.FirstOrDefault(c => c.ChurchName == name)?.ChurchId;

        var funds = new List<Fund>
        {
            // National
            new() { Name = "National General Fund",                   Description = "National-level general fund",                    Level = "National",  ChurchId = null,                                   DistrictId = null,                  Amount = 500000m, CreatedAt = DateTime.UtcNow },
            // District funds
            new() { Name = "Metro Manila District Fund",              Description = "General fund for Metro Manila District",         Level = "District",  ChurchId = null,                                   DistrictId = DistrictId("MMD-001"), Amount = 200000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "North Luzon District Fund",               Description = "General fund for North Luzon District",          Level = "District",  ChurchId = null,                                   DistrictId = DistrictId("NLD-001"), Amount = 150000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "South Luzon & Bicol District Fund",       Description = "General fund for South Luzon & Bicol District",  Level = "District",  ChurchId = null,                                   DistrictId = DistrictId("SLB-001"), Amount = 120000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "Visayas District Fund",                   Description = "General fund for Visayas District",              Level = "District",  ChurchId = null,                                   DistrictId = DistrictId("VSD-001"), Amount = 130000m, CreatedAt = DateTime.UtcNow },
            // Church funds
            new() { Name = "JOBM Main General Fund",                  Description = null, Level = "Church", ChurchId = ChurchId("JOBM Main"),                    DistrictId = DistrictId("MMD-001"), Amount = 80000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Quezon City General Fund",           Description = null, Level = "Church", ChurchId = ChurchId("JOBM Quezon City Branch"),      DistrictId = DistrictId("MMD-001"), Amount = 60000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Makati General Fund",                Description = null, Level = "Church", ChurchId = ChurchId("JOBM Makati Branch"),           DistrictId = DistrictId("MMD-001"), Amount = 55000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Baguio City General Fund",           Description = null, Level = "Church", ChurchId = ChurchId("JOBM Baguio City"),             DistrictId = DistrictId("NLD-001"), Amount = 45000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Dagupan General Fund",               Description = null, Level = "Church", ChurchId = ChurchId("JOBM Dagupan"),                 DistrictId = DistrictId("NLD-001"), Amount = 40000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Naga City General Fund",             Description = null, Level = "Church", ChurchId = ChurchId("JOBM Naga City"),               DistrictId = DistrictId("SLB-001"), Amount = 38000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Legazpi General Fund",               Description = null, Level = "Church", ChurchId = ChurchId("JOBM Legazpi"),                 DistrictId = DistrictId("SLB-001"), Amount = 35000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Cebu City General Fund",             Description = null, Level = "Church", ChurchId = ChurchId("JOBM Cebu City"),               DistrictId = DistrictId("VSD-001"), Amount = 70000m, CreatedAt = DateTime.UtcNow },
            new() { Name = "JOBM Iloilo City General Fund",           Description = null, Level = "Church", ChurchId = ChurchId("JOBM Iloilo City"),             DistrictId = DistrictId("VSD-001"), Amount = 50000m, CreatedAt = DateTime.UtcNow },
        };

        db.Funds.AddRange(funds);
        await db.SaveChangesAsync();

        // Back-link: set church.fund_id and district.fund_id to their primary fund
        foreach (var church in churches)
        {
            var fund = funds.FirstOrDefault(f => f.Level == "Church" && f.ChurchId == church.ChurchId);
            if (fund != null) church.FundId = fund.FundId;
        }
        foreach (var district in districts)
        {
            var fund = funds.FirstOrDefault(f => f.Level == "District" && f.DistrictId == district.DistrictId);
            if (fund != null) district.FundId = fund.FundId;
        }
        await db.SaveChangesAsync();

        Console.WriteLine($"[Seeder] {funds.Count} funds created and linked to churches/districts.");
        await BackfillFundBudgetLinksAsync(db, funds);
    }

    private static async Task BackfillFundBudgetLinksAsync(ApplicationDbContext db, List<Fund> allFunds)
    {
        // Use raw SQL for atomic updates to avoid EF Core connection state issues during seeding
        try
        {
            // 1. Link Budgets to Funds
            await db.Database.ExecuteSqlRawAsync(@"
                UPDATE `budget` b
                JOIN `fund` f ON (
                    (b.level = 'National' AND f.level = 'National') OR
                    (b.level = 'District' AND f.level = 'District' AND f.district_id = b.district_id) OR
                    (b.level = 'Church' AND f.level = 'Church' AND f.church_id = b.church_id)
                )
                SET b.fund_id = f.fund_id
                WHERE b.fund_id IS NULL;
            ");

            // 2. Link Allocations to Budgets
            // This links 'National' allocations to national budgets, 'District' to district, etc.
            await db.Database.ExecuteSqlRawAsync(@"
                UPDATE `budget_allocation` ba
                JOIN `budget` b ON (
                    ( (ba.name LIKE '%National%') AND b.level = 'National' ) OR
                    ( (ba.name LIKE '%District%') AND b.level = 'District' ) OR
                    ( (ba.name NOT LIKE '%National%' AND ba.name NOT LIKE '%District%') AND b.level = 'Church' )
                )
                SET ba.budget_id = b.budget_id
                WHERE ba.budget_id IS NULL;
            ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Seeder] Warning: Backfill failed: {ex.Message}");
        }
    }
}

