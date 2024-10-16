using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
