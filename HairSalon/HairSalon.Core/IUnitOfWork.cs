using HairSalon.Core.Contracts.Repositories;

namespace HairSalon.Core
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IAppointmentRepository AppointmentRepository { get; }

        IAppointmentServiceRepository AppointmentServiceRepository { get; }

        ICustomerRepository CustomerRepository { get; }

        IEmployeeRepository EmployeeRepository { get; }

        IEmployeeScheduleRepository EmployeeScheduleRepository { get; }

        IServiceRepository ServiceRepository { get; }

        IStylistFeedbackRepository StylistFeedbackRepository { get; }

        ITransactionRepository TransactionRepository { get; }

        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
