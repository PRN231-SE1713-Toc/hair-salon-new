﻿using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;
using Microsoft.Identity.Client;

namespace HairSalon.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
