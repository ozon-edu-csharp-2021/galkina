using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(6)]
    public class EmployeesTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists employees(
                    id BIGINT PRIMARY KEY,
                    first_name TEXT NOT NULL,
                    middle_name TEXT NOT NULL,
                    last_name TEXT NOT NULL,
                    email TEXT NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists employees;");
        }
    }
}