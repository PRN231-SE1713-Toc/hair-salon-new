using HairSalon.Core;
using HairSalon.Core.Contracts.Repositories;
using HairSalon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure
{
    public class UnitOfWork(HairSalonDbContext dbContext) : IUnitOfWork
    {
        private readonly HairSalonDbContext _dbContext = dbContext;
        
        private IAppointmentRepository _appointmentRepository;
        private IAppointmentServiceRepository _appointmentServiceRepository;
        private ICustomerRepository _customerService;
        private IEmployeeRepository _employeeRepository;
        private IEmployeeScheduleRepository _employeeScheduleRepository;
        private IServiceRepository _serviceRepository;
        private IStylistFeedbackRepository _stylistFeedbackRepository;
        private ITransactionRepository _transactionRepository;


        public IAppointmentRepository AppointmentRepository
        {
            get
            {
                if (_appointmentRepository is null)
                    _appointmentRepository = new AppointmentRepository(_dbContext);
                return _appointmentRepository;
            }
        }

        public IAppointmentServiceRepository AppointmentServiceRepository 
        {
            get
            {
                if (_appointmentServiceRepository is null)
                    _appointmentServiceRepository = new AppointmentServiceRepository(_dbContext);
                return _appointmentServiceRepository;
            }
        }

        public ICustomerRepository CustomerRepository 
        {
            get
            {
                if (_customerService is null)
                    _customerService = new CustomerRepository(_dbContext);
                return _customerService;
            }
        }

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (_employeeRepository is null)
                    _employeeRepository = new EmployeeRepository(_dbContext);
                return _employeeRepository;
            }
        }

        public IEmployeeScheduleRepository EmployeeScheduleRepository
        {
            get
            {
                if (_employeeScheduleRepository is null)
                    _employeeScheduleRepository = new EmployeeScheduleRepository(_dbContext);
                return _employeeScheduleRepository;
            }
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                if (_serviceRepository is null)
                    _serviceRepository = new ServiceRepository(_dbContext);
                return _serviceRepository;
            }
        }

        public IStylistFeedbackRepository StylistFeedbackRepository
        {
            get
            {
                if (_stylistFeedbackRepository is null)
                    _stylistFeedbackRepository = new StylistFeedbackRepository(_dbContext);
                return _stylistFeedbackRepository;
            }
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository is null)
                    _transactionRepository = new TransactionRepository(_dbContext);
                return _transactionRepository;
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose() => _dbContext.Dispose();

        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Remove the entity from DbContext
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        // Reload the entity's data from the database
                        entry.Reload();
                        break;
                }
            }
            await CommitAsync(cancellationToken);
        }
    }
}
