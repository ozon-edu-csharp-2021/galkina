using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(8)]
    public class FillDictionaries:ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO clothing_sizes (id, name)
                VALUES 
                    (42, 'XS'),
                    (44, 'S'),
                    (46, 'M'),
                    (48, 'L'),
                    (50, 'XL'),
                    (52, 'XXL')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO item_types (id, name)
                VALUES 
                    (1, 'TShirt'),
                    (2, 'Sweatshirt'),
                    (3, 'Notepad'),
                    (4, 'CardHolder'),
                    (5, 'Pen'),
                    (6, 'Socks')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO merch_request_statuses (id, name)
                VALUES 
                    (1, 'Submitted'),
                    (2, 'Validated'),
                    (3, 'StockConfirmed'),
                    (4, 'StockAwaitedDelivery'),
                    (5, 'StockReserved'),
                    (6, 'Cancelled')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO merch_types (id, name)
                VALUES 
                    (10, 'Welcome'),
                    (20, 'ConferenceListener'),
                    (30, 'ConferenceSpeaker'),
                    (40, 'Starter'),
                    (50, 'Veteran')
                ON CONFLICT DO NOTHING
            ");
        }
    }
}