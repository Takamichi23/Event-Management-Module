using CMIS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMIS.Data;

public static class DatabaseMigrator
{
    /// <summary>
    /// Ensures new tables exist by executing raw SQL.
    /// This avoids EF Core migration issues with reverse-engineered databases.
    /// </summary>
    public static async Task EnsureTablesAsync(ApplicationDbContext db)
    {
        // Check if the old 'event' table exists with JSON columns and drop it to migrate to relational
        try
        {
            // A better way to check if a column exists in MySQL:
            var columns = await db.Database.SqlQueryRaw<string>("SELECT COLUMN_NAME FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = 'event' AND column_name = 'ProgramSchedules'").ToListAsync();
            if (columns.Any())
            {
                await db.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS `event`;");
            }
        }
        catch { }

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `budget_proposal` (
                `proposal_id` INT NOT NULL AUTO_INCREMENT,
                `proposal_code` VARCHAR(20) NOT NULL,
                `purpose` VARCHAR(255) NOT NULL,
                `description` TEXT NULL,
                `ministry_id` INT NOT NULL,
                `level` VARCHAR(20) NOT NULL DEFAULT 'Ministry',
                `amount` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `status` VARCHAR(20) NOT NULL DEFAULT 'Pending',
                `submitted_by_id` INT NOT NULL,
                `reviewed_by_id` INT NULL,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                `updated_at` TIMESTAMP NULL,
                PRIMARY KEY (`proposal_id`),
                UNIQUE INDEX `proposal_code` (`proposal_code`),
                INDEX `fk_proposal_ministry` (`ministry_id`),
                INDEX `fk_proposal_submitted_by` (`submitted_by_id`),
                INDEX `fk_proposal_reviewed_by` (`reviewed_by_id`),
                CONSTRAINT `fk_proposal_ministry` FOREIGN KEY (`ministry_id`) REFERENCES `ministry` (`ministry_id`),
                CONSTRAINT `fk_proposal_submitted_by` FOREIGN KEY (`submitted_by_id`) REFERENCES `profile` (`profile_id`),
                CONSTRAINT `fk_proposal_reviewed_by` FOREIGN KEY (`reviewed_by_id`) REFERENCES `profile` (`profile_id`) ON DELETE SET NULL
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `transaction` (
                `transaction_id` INT NOT NULL AUTO_INCREMENT,
                `transaction_code` VARCHAR(20) NOT NULL,
                `description` VARCHAR(255) NOT NULL,
                `type` VARCHAR(20) NOT NULL DEFAULT 'Income',
                `budget_allocation_id` INT NULL,
                `budget_label` VARCHAR(150) NULL,
                `amount` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `recorded_by_id` INT NOT NULL,
                `transaction_date` DATE NOT NULL,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                PRIMARY KEY (`transaction_id`),
                UNIQUE INDEX `transaction_code` (`transaction_code`),
                INDEX `fk_transaction_recorded_by` (`recorded_by_id`),
                INDEX `idx_transaction_allocation` (`budget_allocation_id`),
                CONSTRAINT `fk_transaction_recorded_by` FOREIGN KEY (`recorded_by_id`) REFERENCES `profile` (`profile_id`),
                CONSTRAINT `fk_transaction_allocation` FOREIGN KEY (`budget_allocation_id`) REFERENCES `budget_allocation` (`allocation_id`) ON DELETE SET NULL
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `budget` (
                `budget_id` INT NOT NULL AUTO_INCREMENT,
                `name` VARCHAR(150) NOT NULL,
                `level` VARCHAR(50) NOT NULL DEFAULT 'Church',
                `church_id` INT NULL,
                `district_id` INT NULL,
                `fiscal_year` VARCHAR(10) NOT NULL DEFAULT '2025-2026',
                `total_amount` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                PRIMARY KEY (`budget_id`),
                CONSTRAINT `fk_budget_church` FOREIGN KEY (`church_id`) REFERENCES `church` (`church_id`) ON DELETE SET NULL,
                CONSTRAINT `fk_budget_district` FOREIGN KEY (`district_id`) REFERENCES `district` (`district_id`) ON DELETE SET NULL
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `fund` (
                `fund_id` INT NOT NULL AUTO_INCREMENT,
                `name` VARCHAR(150) NOT NULL,
                `description` TEXT NULL,
                `level` VARCHAR(50) NOT NULL DEFAULT 'Church',
                `church_id` INT NULL,
                `district_id` INT NULL,
                `amount` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                PRIMARY KEY (`fund_id`),
                CONSTRAINT `fk_fund_church` FOREIGN KEY (`church_id`) REFERENCES `church` (`church_id`) ON DELETE SET NULL,
                CONSTRAINT `fk_fund_district` FOREIGN KEY (`district_id`) REFERENCES `district` (`district_id`) ON DELETE SET NULL
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `budget_allocation` (
                `allocation_id` INT NOT NULL AUTO_INCREMENT,
                `name` VARCHAR(150) NOT NULL,
                `category` VARCHAR(50) NOT NULL DEFAULT 'Ministry',
                `allocated` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `spent` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                PRIMARY KEY (`allocation_id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `event` (
                `id` INT NOT NULL AUTO_INCREMENT,
                `title` VARCHAR(150) NULL,
                `description` TEXT NULL,
                `venue` VARCHAR(150) NULL,
                `date` DATETIME NOT NULL,
                `category` VARCHAR(100) NULL,
                `image_url` TEXT NULL,
                `start_time` TIME NOT NULL,
                `end_time` TIME NOT NULL,
                `duration` VARCHAR(100) NULL,
                `expected_participants` INT NOT NULL DEFAULT 0,
                `target_audience` VARCHAR(100) NULL,
                `organizing_ministry` VARCHAR(100) NULL,
                `status` VARCHAR(50) NOT NULL DEFAULT 'Under Review',
                `funding_type` VARCHAR(150) NULL,
                `expected_number_of_participants` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `ticketed_price` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `allocated_budget_per_person` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                `updated_at` TIMESTAMP NULL,
                `attendance_count` INT NOT NULL DEFAULT 0,
                `walk_in_count` INT NOT NULL DEFAULT 0,
                `new_members_count` INT NOT NULL DEFAULT 0,
                `report_notes` TEXT NULL,
                `is_rescheduled` TINYINT(1) NOT NULL DEFAULT 0,
                `is_report_generated` TINYINT(1) NOT NULL DEFAULT 0,
                PRIMARY KEY (`id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `event_program_schedule` (
                `schedule_id` INT NOT NULL AUTO_INCREMENT,
                `event_id` INT NOT NULL,
                `start_time` TIME NOT NULL,
                `end_time` TIME NOT NULL,
                `program_title` VARCHAR(255) NULL,
                `display_order` INT NOT NULL DEFAULT 0,
                PRIMARY KEY (`schedule_id`),
                CONSTRAINT `fk_event_schedule` FOREIGN KEY (`event_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `event_guest` (
                `guest_id` INT NOT NULL AUTO_INCREMENT,
                `event_id` INT NOT NULL,
                `guest_type` VARCHAR(100) NULL,
                `full_name` VARCHAR(255) NULL,
                `contact_number` VARCHAR(50) NULL,
                PRIMARY KEY (`guest_id`),
                CONSTRAINT `fk_event_guest` FOREIGN KEY (`event_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `event_personnel` (
                `personnel_id` INT NOT NULL AUTO_INCREMENT,
                `event_id` INT NOT NULL,
                `role_name` VARCHAR(100) NULL,
                `full_name` VARCHAR(255) NULL,
                `contact_number` VARCHAR(50) NULL,
                PRIMARY KEY (`personnel_id`),
                CONSTRAINT `fk_event_personnel` FOREIGN KEY (`event_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `event_equipment` (
                `equipment_id` INT NOT NULL AUTO_INCREMENT,
                `event_id` INT NOT NULL,
                `equipment_name` VARCHAR(255) NULL,
                `remarks` VARCHAR(255) NULL,
                PRIMARY KEY (`equipment_id`),
                CONSTRAINT `fk_event_equipment` FOREIGN KEY (`event_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `event_transportation` (
                `transportation_id` INT NOT NULL AUTO_INCREMENT,
                `event_id` INT NOT NULL,
                `vehicle_type` VARCHAR(255) NULL,
                `remarks` VARCHAR(255) NULL,
                PRIMARY KEY (`transportation_id`),
                CONSTRAINT `fk_event_transportation` FOREIGN KEY (`event_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `event_expense` (
                `expense_id` INT NOT NULL AUTO_INCREMENT,
                `financial_id` INT NOT NULL,
                `item_name` VARCHAR(255) NULL,
                `estimated_cost` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                PRIMARY KEY (`expense_id`),
                CONSTRAINT `fk_event_expense` FOREIGN KEY (`financial_id`) REFERENCES `event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        // District Event Tables
        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `district_event` (
                `id` INT NOT NULL AUTO_INCREMENT,
                `title` VARCHAR(150) NULL,
                `description` TEXT NULL,
                `venue` VARCHAR(150) NULL,
                `date` DATETIME NOT NULL,
                `category` VARCHAR(100) NULL,
                `image_url` TEXT NULL,
                `start_time` TIME NOT NULL,
                `end_time` TIME NOT NULL,
                `duration` VARCHAR(100) NULL,
                `expected_participants` INT NOT NULL DEFAULT 0,
                `target_audience` VARCHAR(100) NULL,
                `organizing_district` VARCHAR(100) NULL,
                `status` VARCHAR(50) NOT NULL DEFAULT 'Planned Event',
                `funding_type` VARCHAR(150) NULL,
                `expected_number_of_participants` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `ticketed_price` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `allocated_budget_per_person` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `assigned_leader_id` INT NULL,
                `assigned_leader_name` VARCHAR(255) NULL,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                `updated_at` TIMESTAMP NULL,
                `attendance_count` INT NOT NULL DEFAULT 0,
                `walk_in_count` INT NOT NULL DEFAULT 0,
                `new_members_count` INT NOT NULL DEFAULT 0,
                `report_notes` TEXT NULL,
                `is_rescheduled` TINYINT(1) NOT NULL DEFAULT 0,
                `is_report_generated` TINYINT(1) NOT NULL DEFAULT 0,
                PRIMARY KEY (`id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `district_event_program_schedule` (
                `schedule_id` INT NOT NULL AUTO_INCREMENT,
                `district_event_id` INT NOT NULL,
                `start_time` TIME NOT NULL,
                `end_time` TIME NOT NULL,
                `program_title` VARCHAR(255) NULL,
                `display_order` INT NOT NULL DEFAULT 0,
                PRIMARY KEY (`schedule_id`),
                CONSTRAINT `fk_district_event_schedule` FOREIGN KEY (`district_event_id`) REFERENCES `district_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `district_event_guest` (
                `guest_id` INT NOT NULL AUTO_INCREMENT,
                `district_event_id` INT NOT NULL,
                `guest_type` VARCHAR(100) NULL,
                `full_name` VARCHAR(255) NULL,
                `contact_number` VARCHAR(50) NULL,
                PRIMARY KEY (`guest_id`),
                CONSTRAINT `fk_district_event_guest` FOREIGN KEY (`district_event_id`) REFERENCES `district_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `district_event_personnel` (
                `personnel_id` INT NOT NULL AUTO_INCREMENT,
                `district_event_id` INT NOT NULL,
                `role_name` VARCHAR(100) NULL,
                `full_name` VARCHAR(255) NULL,
                `contact_number` VARCHAR(50) NULL,
                PRIMARY KEY (`personnel_id`),
                CONSTRAINT `fk_district_event_personnel` FOREIGN KEY (`district_event_id`) REFERENCES `district_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `district_event_equipment` (
                `equipment_id` INT NOT NULL AUTO_INCREMENT,
                `district_event_id` INT NOT NULL,
                `equipment_name` VARCHAR(255) NULL,
                `remarks` VARCHAR(255) NULL,
                PRIMARY KEY (`equipment_id`),
                CONSTRAINT `fk_district_event_equipment` FOREIGN KEY (`district_event_id`) REFERENCES `district_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `district_event_transportation` (
                `transportation_id` INT NOT NULL AUTO_INCREMENT,
                `district_event_id` INT NOT NULL,
                `vehicle_type` VARCHAR(255) NULL,
                `remarks` VARCHAR(255) NULL,
                PRIMARY KEY (`transportation_id`),
                CONSTRAINT `fk_district_event_transportation` FOREIGN KEY (`district_event_id`) REFERENCES `district_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `district_event_expense` (
                `expense_id` INT NOT NULL AUTO_INCREMENT,
                `district_event_id` INT NOT NULL,
                `item_name` VARCHAR(255) NULL,
                `estimated_cost` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                PRIMARY KEY (`expense_id`),
                CONSTRAINT `fk_district_event_expense` FOREIGN KEY (`district_event_id`) REFERENCES `district_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        // BOD Event Tables
        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `bod_event` (
                `id` INT NOT NULL AUTO_INCREMENT,
                `title` VARCHAR(150) NULL,
                `description` TEXT NULL,
                `venue` VARCHAR(150) NULL,
                `date` DATETIME NOT NULL,
                `category` VARCHAR(100) NULL,
                `image_url` TEXT NULL,
                `start_time` TIME NOT NULL,
                `end_time` TIME NOT NULL,
                `duration` VARCHAR(100) NULL,
                `expected_participants` INT NOT NULL DEFAULT 0,
                `target_audience` VARCHAR(100) NULL,
                `organizing_body` VARCHAR(100) NULL,
                `status` VARCHAR(50) NOT NULL DEFAULT 'Planned Event',
                `funding_type` VARCHAR(150) NULL,
                `expected_number_of_participants` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `ticketed_price` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `allocated_budget_per_person` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                `prepared_by` VARCHAR(255) NULL,
                `date_prepared` DATETIME NULL,
                `endorsed_by` VARCHAR(255) NULL,
                `approved_by` VARCHAR(255) NULL,
                `signature_path` VARCHAR(255) NULL,
                `assigned_leader_id` INT NULL,
                `assigned_leader_name` VARCHAR(255) NULL,
                `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                `updated_at` TIMESTAMP NULL,
                `attendance_count` INT NOT NULL DEFAULT 0,
                `walk_in_count` INT NOT NULL DEFAULT 0,
                `new_members_count` INT NOT NULL DEFAULT 0,
                `report_notes` TEXT NULL,
                `is_rescheduled` TINYINT(1) NOT NULL DEFAULT 0,
                `is_report_generated` TINYINT(1) NOT NULL DEFAULT 0,
                PRIMARY KEY (`id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `bod_event_program_schedule` (
                `schedule_id` INT NOT NULL AUTO_INCREMENT,
                `bod_event_id` INT NOT NULL,
                `start_time` TIME NOT NULL,
                `end_time` TIME NOT NULL,
                `program_title` VARCHAR(255) NULL,
                `display_order` INT NOT NULL DEFAULT 0,
                PRIMARY KEY (`schedule_id`),
                CONSTRAINT `fk_bod_event_schedule` FOREIGN KEY (`bod_event_id`) REFERENCES `bod_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `bod_event_guest` (
                `guest_id` INT NOT NULL AUTO_INCREMENT,
                `bod_event_id` INT NOT NULL,
                `guest_type` VARCHAR(100) NULL,
                `full_name` VARCHAR(255) NULL,
                `contact_number` VARCHAR(50) NULL,
                PRIMARY KEY (`guest_id`),
                CONSTRAINT `fk_bod_event_guest` FOREIGN KEY (`bod_event_id`) REFERENCES `bod_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `bod_event_personnel` (
                `personnel_id` INT NOT NULL AUTO_INCREMENT,
                `bod_event_id` INT NOT NULL,
                `role_name` VARCHAR(100) NULL,
                `full_name` VARCHAR(255) NULL,
                `contact_number` VARCHAR(50) NULL,
                PRIMARY KEY (`personnel_id`),
                CONSTRAINT `fk_bod_event_personnel` FOREIGN KEY (`bod_event_id`) REFERENCES `bod_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `bod_event_equipment` (
                `equipment_id` INT NOT NULL AUTO_INCREMENT,
                `bod_event_id` INT NOT NULL,
                `equipment_name` VARCHAR(255) NULL,
                `remarks` VARCHAR(255) NULL,
                PRIMARY KEY (`equipment_id`),
                CONSTRAINT `fk_bod_event_equipment` FOREIGN KEY (`bod_event_id`) REFERENCES `bod_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `bod_event_transportation` (
                `transportation_id` INT NOT NULL AUTO_INCREMENT,
                `bod_event_id` INT NOT NULL,
                `vehicle_type` VARCHAR(255) NULL,
                `remarks` VARCHAR(255) NULL,
                PRIMARY KEY (`transportation_id`),
                CONSTRAINT `fk_bod_event_transportation` FOREIGN KEY (`bod_event_id`) REFERENCES `bod_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        await db.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS `bod_event_expense` (
                `expense_id` INT NOT NULL AUTO_INCREMENT,
                `bod_event_id` INT NOT NULL,
                `item_name` VARCHAR(255) NULL,
                `estimated_cost` DECIMAL(15,2) NOT NULL DEFAULT 0.00,
                PRIMARY KEY (`expense_id`),
                CONSTRAINT `fk_bod_event_expense` FOREIGN KEY (`bod_event_id`) REFERENCES `bod_event` (`id`) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
        ");

        try
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `event` ADD COLUMN `attendance_count` INT NOT NULL DEFAULT 0;");
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `event` ADD COLUMN `walk_in_count` INT NOT NULL DEFAULT 0;");
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `event` ADD COLUMN `new_members_count` INT NOT NULL DEFAULT 0;");
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `event` ADD COLUMN `report_notes` TEXT NULL;");
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `event` ADD COLUMN `is_rescheduled` TINYINT(1) NOT NULL DEFAULT 0;");
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `event` ADD COLUMN `is_report_generated` TINYINT(1) NOT NULL DEFAULT 0;");
        }
        catch { }

        // Add missing columns to pre-existing tables — check information_schema before altering
        var profileAddressCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'profile' AND COLUMN_NAME = 'address'"
        ).ToListAsync();
        if (!profileAddressCols.Any())
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `profile` ADD COLUMN `address` TEXT NULL;");

        var eventStatusCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'event' AND COLUMN_NAME = 'status'"
        ).ToListAsync();
        if (!eventStatusCols.Any())
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `event` ADD COLUMN `status` VARCHAR(50) NOT NULL DEFAULT 'Under Review';");

        var proposalChurchCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget_proposal' AND COLUMN_NAME = 'church_id'"
        ).ToListAsync();
        if (!proposalChurchCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` ADD COLUMN `church_id` INT NULL;");
            await db.Database.ExecuteSqlRawAsync("UPDATE `budget_proposal` bp JOIN `profile` p ON bp.submitted_by_id = p.profile_id SET bp.church_id = p.church_id;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` ADD CONSTRAINT `fk_proposal_church` FOREIGN KEY (`church_id`) REFERENCES `church` (`church_id`) ON DELETE SET NULL;"); } catch { }
        }

        var proposalDistrictCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget_proposal' AND COLUMN_NAME = 'district_id'"
        ).ToListAsync();
        if (!proposalDistrictCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` ADD COLUMN `district_id` INT NULL;");
            await db.Database.ExecuteSqlRawAsync("UPDATE `budget_proposal` bp JOIN `profile` p ON bp.submitted_by_id = p.profile_id JOIN `church` c ON p.church_id = c.church_id SET bp.district_id = c.district_id;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` ADD CONSTRAINT `fk_proposal_district` FOREIGN KEY (`district_id`) REFERENCES `district` (`district_id`) ON DELETE SET NULL;"); } catch { }
        }

        var ministryNullable = await db.Database.SqlQueryRaw<string>(
            "SELECT IS_NULLABLE FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget_proposal' AND COLUMN_NAME = 'ministry_id'"
        ).ToListAsync();
        if (ministryNullable.FirstOrDefault() == "NO")
        {
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` DROP FOREIGN KEY `fk_proposal_ministry`;"); } catch { }
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` MODIFY COLUMN `ministry_id` INT NULL;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` ADD CONSTRAINT `fk_proposal_ministry` FOREIGN KEY (`ministry_id`) REFERENCES `ministry` (`ministry_id`) ON DELETE SET NULL;"); } catch { }
        }

        var proposalReasonCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget_proposal' AND COLUMN_NAME = 'rejection_reason'"
        ).ToListAsync();
        if (!proposalReasonCols.Any())
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_proposal` ADD COLUMN `rejection_reason` TEXT NULL;");

        // Migrate transaction column to budget_allocation_id INT FK (handles all prior DB states)
        // Step 1: drop legacy account_name column if still present
        var txAccountNameCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'transaction' AND COLUMN_NAME = 'account_name'"
        ).ToListAsync();
        if (txAccountNameCols.Any())
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `transaction` DROP COLUMN `account_name`;");

        // Step 2: drop intermediate budget_allocation VARCHAR column if present
        var txBudgetAllocVarchar = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'transaction' AND COLUMN_NAME = 'budget_allocation' AND DATA_TYPE = 'varchar'"
        ).ToListAsync();
        if (txBudgetAllocVarchar.Any())
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `transaction` DROP COLUMN `budget_allocation`;");

        // Step 3: add budget_allocation_id INT FK if not already present
        var txBudgetAllocIdCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'transaction' AND COLUMN_NAME = 'budget_allocation_id'"
        ).ToListAsync();
        if (!txBudgetAllocIdCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `transaction` ADD COLUMN `budget_allocation_id` INT NULL;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `transaction` ADD INDEX `idx_transaction_allocation` (`budget_allocation_id`);"); } catch { }
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `transaction` ADD CONSTRAINT `fk_transaction_allocation` FOREIGN KEY (`budget_allocation_id`) REFERENCES `budget_allocation` (`allocation_id`) ON DELETE SET NULL;"); } catch { }
        }

        // Add level column to budget (for existing tables)
        var budgetLevelCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget' AND COLUMN_NAME = 'level'"
        ).ToListAsync();
        if (!budgetLevelCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget` ADD COLUMN `level` VARCHAR(50) NOT NULL DEFAULT 'Church' AFTER `name`;");
        }

        // Add church_id column to budget (for existing tables)
        var budgetChurchCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget' AND COLUMN_NAME = 'church_id'"
        ).ToListAsync();
        if (!budgetChurchCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget` ADD COLUMN `church_id` INT NULL AFTER `level`;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget` ADD CONSTRAINT `fk_budget_church` FOREIGN KEY (`church_id`) REFERENCES `church` (`church_id`) ON DELETE SET NULL;"); } catch { }
        }

        // Add district_id column to budget (for existing tables)
        var budgetDistrictCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget' AND COLUMN_NAME = 'district_id'"
        ).ToListAsync();
        if (!budgetDistrictCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget` ADD COLUMN `district_id` INT NULL AFTER `church_id`;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget` ADD CONSTRAINT `fk_budget_district` FOREIGN KEY (`district_id`) REFERENCES `district` (`district_id`) ON DELETE SET NULL;"); } catch { }
        }

        // Add budget_id column to budget_allocation (for existing tables)
        var allocationBudgetCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget_allocation' AND COLUMN_NAME = 'budget_id'"
        ).ToListAsync();
        if (!allocationBudgetCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_allocation` ADD COLUMN `budget_id` INT NULL;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget_allocation` ADD CONSTRAINT `fk_allocation_budget` FOREIGN KEY (`budget_id`) REFERENCES `budget` (`budget_id`) ON DELETE SET NULL;"); } catch { }
        }

        var accountChurchCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'account' AND COLUMN_NAME = 'church_id'"
        ).ToListAsync();
        if (!accountChurchCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `account` ADD COLUMN `church_id` INT NULL;");
            await db.Database.ExecuteSqlRawAsync("UPDATE `account` a JOIN `profile` p ON a.profile_id = p.profile_id SET a.church_id = p.church_id;");
            try
            {
                await db.Database.ExecuteSqlRawAsync("ALTER TABLE `account` ADD CONSTRAINT `fk_account_church` FOREIGN KEY (`church_id`) REFERENCES `church` (`church_id`) ON DELETE SET NULL;");
            }
            catch { }
        }

        // Add district_id column to account (for existing tables)
        var accountDistrictCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'account' AND COLUMN_NAME = 'district_id'"
        ).ToListAsync();
        if (!accountDistrictCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `account` ADD COLUMN `district_id` INT NULL;");
            await db.Database.ExecuteSqlRawAsync("UPDATE `account` a JOIN `church` c ON a.church_id = c.church_id SET a.district_id = c.district_id;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `account` ADD CONSTRAINT `fk_account_district` FOREIGN KEY (`district_id`) REFERENCES `district` (`district_id`) ON DELETE SET NULL;"); } catch { }
        }

        // Add fund_id column to budget (for existing tables)
        var budgetFundCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'budget' AND COLUMN_NAME = 'fund_id'"
        ).ToListAsync();
        if (!budgetFundCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget` ADD COLUMN `fund_id` INT NULL;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `budget` ADD CONSTRAINT `fk_budget_fund` FOREIGN KEY (`fund_id`) REFERENCES `fund` (`fund_id`) ON DELETE SET NULL;"); } catch { }
        }

        // Add fund_id column to church (for existing tables)
        var churchFundCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'church' AND COLUMN_NAME = 'fund_id'"
        ).ToListAsync();
        if (!churchFundCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `church` ADD COLUMN `fund_id` INT NULL;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `church` ADD CONSTRAINT `fk_church_fund` FOREIGN KEY (`fund_id`) REFERENCES `fund` (`fund_id`) ON DELETE SET NULL;"); } catch { }
        }

        // Add fund_id column to district (for existing tables)
        var districtFundCols = await db.Database.SqlQueryRaw<string>(
            "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'district' AND COLUMN_NAME = 'fund_id'"
        ).ToListAsync();
        if (!districtFundCols.Any())
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE `district` ADD COLUMN `fund_id` INT NULL;");
            try { await db.Database.ExecuteSqlRawAsync("ALTER TABLE `district` ADD CONSTRAINT `fk_district_fund` FOREIGN KEY (`fund_id`) REFERENCES `fund` (`fund_id`) ON DELETE SET NULL;"); } catch { }
        }
    }
}
