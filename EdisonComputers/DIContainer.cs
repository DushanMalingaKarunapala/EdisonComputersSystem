using BLL.Services.ComponentService;
using BLL.Services.CustomerService;
using BLL.Services.DashboardService;
using BLL.Services.DeviceService;
using BLL.Services.JobService;
using BLL.Services.RepairRecordService;
using BLL.Services.ServiceRecordService;
using BLL.Services.UpgradeRecordService;
using BLL.Services.UserService;
using Core.IRepository;
using Core.IRepository.IComponentRepository;
using Core.IRepository.ICustomerRepository;
using Core.IRepository.IDeviceImageRepository;
using Core.IRepository.IDeviceRepository;
using Core.IRepository.IJobRepository;
using Core.IRepository.IRepairRecordRepository;
using Core.IRepository.IServiceRecordRepository;
using Core.IRepository.IUpgradeRecordRepository;
using Core.IRepository.IUserRepository;
using Core.Models;
using DAL;
using DAL.Repositories;
using DAL.Repositories.ComponentRepository;
using DAL.Repositories.CustomerRepository;
using DAL.Repositories.DeviceImageRepository;
using DAL.Repositories.DeviceRepository;

using DAL.Repositories.JobRepository;
using DAL.Repositories.RepairRecordRepository;
using DAL.Repositories.ServiceRecordRepository;
using DAL.Repositories.UpgradeRecordRepository;
using DAL.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation
{
    public class DIContainer
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            // Add services here

            services.AddDbContext<ApplicationDbContext>(options =>
            
            options.UseSqlServer("Server=BELUGA;Database=ComputerRepairDB;Trusted_Connection=True;Trust Server Certificate=True; MultipleActiveResultSets=True;"),
                ServiceLifetime.Scoped

            //Server=BELUGA;Database=ComputerRepairDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"
            );

            services.AddScoped<CustomerService>();
            services.AddScoped<DeviceService>();
            services.AddScoped<JobService>();
            services.AddScoped<ComponentService>();
            services.AddScoped<RepairRecordService>();
            services.AddScoped<UpgradeRecordService>();
            services.AddScoped<ServiceRecordService>();
            services.AddScoped<DashboardService>();
            services.AddScoped<UserService>();
            services.AddScoped<BLL.Services.InvoiceService.InvoiceService>();
         


            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRepository<Customer>, Repository<Customer>>();

            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IRepository<Device>, Repository<Device>>();

            services.AddScoped<IjobRepository, JobRepository>();
            services.AddScoped<IRepository<Job>, Repository<Job>>();

            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IRepository<Component>, Repository<Component>>();

            services.AddScoped<IDeviceImageRepository, DeviceImageRepository>();

            services.AddScoped<IRepairRecordRepository, RepairRecordRepository>();
            services.AddScoped<IRepository<RepairRecord>, Repository<RepairRecord>>();

            services.AddScoped<IUpgradeRecordRepository, UpgradeRecordRepository>();
            services.AddScoped<IRepository<UpgradeRecord>, Repository<UpgradeRecord>>();

            services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
            services.AddScoped<IRepository<ServiceRecord>, Repository<ServiceRecord>>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRepository<User>, Repository<User>>();

            return services.BuildServiceProvider();



        }
    }
}
