using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(1)]
    public class MerchPackTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists merchpacks(
                    id BIGSERIAL PRIMARY KEY,
                    merch_type_id INT NOT NULL,
                    employee_id INT NOT NULL,
                    merch_request_status INT NOT NULL,
                    clothing_size INT,
                    request_date TIMESTAMP NOT NULL,
                    issue_date TIMESTAMP);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists merchpacks;");
        }
    }
}