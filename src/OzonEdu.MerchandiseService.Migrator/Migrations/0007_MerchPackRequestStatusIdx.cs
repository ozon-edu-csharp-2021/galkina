using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(7, TransactionBehavior.None)]
    public class MerchPackRequestStatusIdx : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE INDEX CONCURRENTLY merchpack_request_status_idx ON merchpacks (merch_request_status)"
            );
        }
    }
}