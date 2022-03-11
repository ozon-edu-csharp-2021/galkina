using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Implementation
{
    public class MerchPackRepository : IMerchPackRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private const int Timeout = 10;

        public MerchPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }
        
        public async Task<MerchPack> AddMerchPackAsync(MerchPack merchPackToCreate, CancellationToken cancellationToken)
        {
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            
            const string sql = @"
                INSERT INTO merchpacks (merch_type_id, employee_id, merch_request_status, clothing_size, request_date, issue_date)
                VALUES (@MerchTypeId, @EmployeeId, @MerchRequestStatus, @ClothingSize, @RequestDate, @IssueDate);
                INSERT INTO employees (id, first_name, middle_name, last_name, email)
                VALUES (@EmployeeId, @FirstName, @MiddleName, @LastName, @Email)
                ON CONFLICT DO NOTHING;";

            var parameters = new
            {
                MerchTypeId = merchPackToCreate.Type.Id,
                MerchRequestStatus = merchPackToCreate.Status.Id,
                ClothingSize = merchPackToCreate?.ClothingSize?.Id,
                RequestDate = merchPackToCreate.RequestDate,
                IssueDate = merchPackToCreate.IssueDate,
                EmployeeId = merchPackToCreate.Employee.Id,
                FirstName = merchPackToCreate.Employee.FirstName.Value,
                MiddleName = merchPackToCreate.Employee.MiddleName.Value,
                LastName = merchPackToCreate.Employee.LastName.Value,
                Email = merchPackToCreate.Employee.Email.Value
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(merchPackToCreate);
            return merchPackToCreate;
        }

        public async Task<MerchPack> UpdateMerchPackAsync(MerchPack merchPackToUpdate, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE merchpacks
                SET merch_request_status = @MerchRequestStatus, issue_date = @IssueDate
                WHERE id = @MerchPackId;";

            var parameters = new
            {
                MerchPackId = merchPackToUpdate.Id,
                MerchRequestStatus = merchPackToUpdate.Status.Id,
                IssueDate = merchPackToUpdate.IssueDate
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(merchPackToUpdate);
            return merchPackToUpdate;
        }

        public async Task<List<MerchPack>> GetIssuedMerchPacksToEmployeeAsync(long employeeId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merchpacks.id, merchpacks.merch_type_id, merchpacks.employee_id, merchpacks.merch_request_status, 
                       merchpacks.clothing_size, merchpacks.request_date, merchpacks.issue_date,
                       employees.id, employees.first_name, employees.middle_name, employees.last_name, employees.email,
                       clothing_sizes.id, clothing_sizes.name       
                FROM merchpacks
                INNER JOIN employees on merchpacks.employee_id = employees.id
                INNER JOIN merch_request_statuses on merch_request_statuses.id = merchpacks.merch_request_status
                LEFT JOIN clothing_sizes on clothing_sizes.id = merchpacks.clothing_size
                WHERE employee_id = @EmployeeId AND merch_request_status = 5;";
            
            var parameters = new
            {
                EmployeeId = employeeId,
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var merchPacks = await connection.QueryAsync<
                Models.MerchPack, Models.Employee, Models.ClothingSize, MerchPack>(commandDefinition,
                (merchPack, employee, clothingSize) => new MerchPack(
                    merchPack.Id,
                    MerchType.Parse(merchPack.MerchTypeId),
                    merchPack.ClothingSize is not null ? ClothingSize.Parse(clothingSize.Name) : null,
                    new Employee(employee.Id, employee.FirstName, employee.LastName, employee.MiddleName, employee.Email),
                    merchPack.RequestDate,
                    merchPack.IssueDate));

            var result = merchPacks.ToList();

            return result;
        }

        public async Task<List<MerchPack>> GetMerchPacksAwaitedDeliveryAsync(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merchpacks.id, merchpacks.merch_type_id, merchpacks.employee_id, merchpacks.merch_request_status,
                       merchpacks.clothing_size, merchpacks.request_date,
                       employees.id, employees.first_name, employees.middle_name, employees.last_name, employees.email,
                       clothing_sizes.id, clothing_sizes.name       
                FROM merchpacks
                INNER JOIN employees on merchpacks.employee_id = employees.id
                LEFT JOIN clothing_sizes on clothing_sizes.id = merchpacks.clothing_size
                WHERE merch_request_status = 4;";
            
            var commandDefinition = new CommandDefinition(
                sql,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var merchPacks = await connection.QueryAsync<
                Models.MerchPack, Models.Employee, Models.ClothingSize, MerchPack>(commandDefinition,
                (merchPack, employee, clothingSize) => new MerchPack(
                    merchPack.Id,
                    MerchType.Parse(merchPack.MerchTypeId),
                    merchPack.ClothingSize is not null ? ClothingSize.Parse(clothingSize.Name) : null,
                    new Employee(employee.Id, employee.FirstName, employee.LastName, employee.MiddleName, employee.Email),
                    merchPack.RequestDate));

            var result = merchPacks.ToList();

            return result;
        }
    }
}