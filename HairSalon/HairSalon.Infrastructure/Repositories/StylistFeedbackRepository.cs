using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Repositories
{
    public class StylistFeedbackRepository : Repository<StylistFeedback>, IStylistFeedbackRepository
    {
        public StylistFeedbackRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
