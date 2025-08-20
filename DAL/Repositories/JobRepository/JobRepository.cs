using Core.Enums;
using Core.IRepository.IJobRepository;
using Core.Models;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using Core.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.JobRepository
{
    public class JobRepository : Repository<Job>, IjobRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public JobRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Job> AddJob(int deviceId, Job job)
        {

            var device = await _dbContext.Devices.FindAsync(deviceId);

            job.DeviceId = deviceId; // Set the device ID for the job

            //device.CustomerId = customerId; // this could be problematic if the device already has a customer assigned so removing this line

            var addedJob = await _dbContext.Jobs.AddAsync(job);
            await _dbContext.SaveChangesAsync(); // Save changes to the database
            return addedJob.Entity; // Return the added job entity



        }



        public async Task<IEnumerable<Job>> GetJobsByDeviceIdAsync(int deviceId)
        {
            return await _dbContext.Jobs
                .Where(j => j.DeviceId == deviceId)
                .ToListAsync();
        }

        //get jobs by job status
        public async Task<List<Job>> GetJobsByStatusAsync(JobStatus status)
        {
            return await _dbContext.Jobs
                .Include(j => j.Device).ThenInclude(d => d.Customer) // Include Device and Customer navigation properties
                .Where(j=> j.JobStatus == status)
                .ToListAsync();
        }

        public async Task<Job> UpdateJob(int jobId, Job jobToUpdate)
        {
            //update the job with the given id
            var existingJob = _dbContext.Jobs.Find(jobId);
            if (existingJob == null)
            {
                throw new ArgumentException($"Job with ID {jobId} does not exist.");
            }
            // Update the properties of the existing job with the new values
            _dbContext.Entry(existingJob).CurrentValues.SetValues(jobToUpdate);

            await _dbContext.SaveChangesAsync();
            return existingJob; // Return the updated job entity

        }

        //public Task<Job> GetJobByIdWithDetailsAsync(int jobId)
        //{

        //    // This method should retrieve a job by its ID along with related device, customer, and billing information.
        //    // have navigation properties set up in your Job model for Device, Customer, and Billing.
        //    var job = _dbContext.Jobs
        //        .Include(j => j.Device)
        //        .ThenInclude(d => d.Customer) // Assuming Device has a Customer navigation property
        //        .Include(j => j.Billing) // Assuming Job has a Billing navigation property
        //        .FirstOrDefaultAsync(j => j.JobId == jobId);
        //    return job;
        //}

        //a method to get a job by id with device, customer, billing, repair records, upgrade records, and service records
        public async Task<Job> GetJobByIdWithAllDetailsForBillingAsync(int jobId)
        {
            return await _dbContext.Jobs
                .Include(j => j.Device).ThenInclude(d => d.Customer)
                .Include(j => j.Billing)
                //only include repair records, upgrade records and service records where status is not cancelled
                .Include(j => j.RepairRecords.Where(rr => rr.RepairStatus != RepairStatus.Cancelled)).ThenInclude(rr => rr.RepairRecordComponents).ThenInclude(rrc => rrc.Component)
                .Include(j => j.UpgradeRecords.Where(ur => ur.UpgradeStatus != UpgradeStatus.Cancelled)).ThenInclude(ur => ur.UpgradeRecordComponents).ThenInclude(urc => urc.Component)
                .Include(j => j.ServiceRecords.Where(sr => sr.ServiceStatus != ServiceStatus.ServiceCancelled)) // Assuming ServiceRecords has a ServiceStatus property
                //.Include(j => j.RepairRecords).ThenInclude(rr => rr.RepairRecordComponents).ThenInclude(rrc => rrc.Component)
                //.Include(j => j.UpgradeRecords).ThenInclude(ur => ur.UpgradeRecordComponents).ThenInclude(urc => urc.Component)
                //.Include(j => j.ServiceRecords) // Include ServiceRecordComponents if they exist and use components
               
                .FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        //public async Task SaveOrUpdateJobBillingAsync(int jobId, decimal billingAmount, decimal manualJobCost, bool isPaid, DateTime? paymentDate, PaymentMethods paymentMethod)
        //{
        //    var job = await _dbContext.Jobs.Include(j => j.Billing).FirstOrDefaultAsync(j => j.JobId == jobId);
        //    if (job == null)
        //    {
        //        throw new ArgumentException($"Job with ID {jobId} does not exist.");
        //    }
        //    // If the job has no billing information, create a new Billing object
        //    if (job.Billing == null)
        //    {
        //        job.Billing = new Billing
        //        {
        //            JobId = jobId,
        //            //BillingAmount = billingAmount,
        //            Amount = manualJobCost,
        //            IsPaid = isPaid,
        //            PaymentDate = paymentDate,
        //            PaymentMethod = paymentMethod
        //        };
        //        _dbContext.Billings.Add(job.Billing);
        //    }
        //    else
        //    {
        //        // Update existing billing information
        //        job.Billing.Amount = billingAmount;
        //        //job.Billing.ManualJobCost = manualJobCost;
        //        job.Billing.IsPaid = isPaid;
        //        job.Billing.PaymentDate = paymentDate;
        //        job.Billing.PaymentMethod = paymentMethod;
        //    }
        //    await _dbContext.SaveChangesAsync();

        //}

        //a method to get a job by id with billing information only
        public async Task<Job> GetJobByIdWithBillingAsync(int jobId)
        {
            return await _dbContext.Jobs.Include(j => j.Billing).FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        //a method to update only the job entity without affecting the billing information
        public async Task UpdateJobOnlyAsync(Job job)
        {
            _dbContext.Jobs.Update(job); // This will also update Job.Billing if it's tracked and modified
            await _dbContext.SaveChangesAsync();



        }

        public async Task<int> CountJobsByStatusesAsync(IEnumerable<JobStatus> statuses)
        {
            //count the number of jobs with the given statuses
            return await _dbContext.Jobs.CountAsync(j => statuses.Contains(j.JobStatus));
        }


        public async Task<int> CountJobsByStatusAndBillingAsync()
        {
            //count the number of jobs with the given status and billing information
            return await _dbContext.Jobs.Where(j=>j.JobStatus == JobStatus.ReadyForPickup && j.Billing != null && j.Billing.IsPaid == true)
                .CountAsync();

        }

        public async Task<int> CountCompletedJobsNotPaidAsync()
        {
            //count the number of completed jobs that are not paid
            return await _dbContext.Jobs
                .Where(j => j.JobStatus == JobStatus.TasksCompleted || j.JobStatus == JobStatus.ReadyForBilling && (j.Billing != null || !j.Billing.IsPaid == false))
                .CountAsync();
        }


       

        public async Task<List<Job>> GetRecentJobsByDaysAsync(int days)
        {
            // Get jobs that were checked in within the last 'days' days
            var cutoffDate = DateTime.Now.AddDays(-days);
            return await _dbContext.Jobs
                .Include(j => j.Device).ThenInclude(d => d.Customer) // Include Device and Customer navigation properties
                .Where(j => j.CheckedInDate >= cutoffDate)
                .OrderByDescending(j => j.CheckedInDate) // order by the date the job was checked in
                .ToListAsync();

        }

        public async Task<List<Job>> GetAllJobsWithBillingAsync(bool? isPaid = null)
        {
            // Get all jobs with billing information, optionally filtered by payment status
            var query = _dbContext.Jobs.Include(j => j.Billing).AsQueryable();
            if (isPaid.HasValue)
            {
                query = query.Where(j => j.Billing.IsPaid == isPaid.Value);
            }
            return await query.ToListAsync();

        }

        public async Task<List<Job>> GetRecentJobsAsync(int count)
        {
            // Get the most recent jobs added to the system, ordered by creation date
            return await _dbContext.Jobs
                .OrderByDescending(j => j.CheckedInDate) // order by the date the job was checked in
                .Take(count)// take the latest 'count' jobs
                .ToListAsync();
        }

        public async  Task<JobDetailsViewModel> GetJobDetailsForViewByIdAsync(int jobId)
        {
            // This query is highly efficient. It translates to a single SQL query
            // that joins the tables and returns only the selected columns.
            // EF Core does not create full Job, Device, Customer, etc. entities.
            var result = await _dbContext.Jobs
        .Where(j => j.JobId == jobId)
        .Select(j => new JobDetailsViewModel
        {
            // Job Info
            JobId = j.JobId,
            JobDescription = j.JobDescription,
            JobStatus = j.JobStatus,
            CheckedInDate = j.CheckedInDate,
            CompletionDate = j.CompletionDate,
            ManualJobCost = j.ManualJobCost ?? 0,

            // Device & Customer Info
            DeviceIdentifier = $"{j.Device.Brand} {j.Device.Model} (SN: {j.Device.SerialNumber})",
            CustomerFullName = j.Device.Customer.FirstName + " " + j.Device.Customer.LastName,

            // Billing Info
            BillingExists = j.Billing != null,
            BillingAmount = j.Billing != null ? j.Billing.Amount : 0,
            IsPaid = j.Billing != null ? (j.Billing.IsPaid ?? false) : false,
            PaymentDate = j.Billing != null ? j.Billing.PaymentDate : null,
            PaymentMethod = j.Billing != null ? j.Billing.PaymentMethod : PaymentMethods.Cash,

            // --- Database-side Calculations ---
            TotalComponentChargeToCustomer =
                (j.RepairRecords.SelectMany(rr => rr.RepairRecordComponents).Sum(rrc => (decimal?)rrc.QuantityUsed * rrc.PricePerUnit) ?? 0) +
                (j.UpgradeRecords.SelectMany(ur => ur.UpgradeRecordComponents).Sum(urc => (decimal?)urc.QuantityUsed * urc.PricePerUnit) ?? 0),

            ActualStoreCostForComponents =
                (j.RepairRecords.SelectMany(rr => rr.RepairRecordComponents).Sum(rrc => (decimal?)rrc.QuantityUsed * rrc.Component.Price) ?? 0) +
                (j.UpgradeRecords.SelectMany(ur => ur.UpgradeRecordComponents).Sum(urc => (decimal?)urc.QuantityUsed * urc.Component.Price) ?? 0)

        }).FirstOrDefaultAsync();


            if (result != null)
            {
                // Calculate profit in C# after getting the data
                result.CalculatedProfit = result.BillingAmount - result.ActualStoreCostForComponents;
            }

            return result;
        }



        //method to get monthly summary of sales and costs for a given year
        public async Task<List<MonthlySummaryViewModel>> GetMonthlySummaryAsync(int year)
        {
            // --- Step 1: Fetch all PAID bills for the year. We only need the amount, month, and the job's manual cost. ---
            // This query is simple and efficient.
            var paidBillsForYear = await _dbContext.Billings
                .Where(b => b.IsPaid == true && b.PaymentDate.HasValue && b.PaymentDate.Value.Year == year)
                .Select(b => new // Project into a temporary, flat anonymous type
                {
                    PaymentMonth = b.PaymentDate.Value.Month,
                    SaleAmount = b.Amount,
                    // This is the profit for this specific job, according to your rule
                    ProfitForJob = b.Job.ManualJobCost ?? 0
                })
                .ToListAsync(); // Execute the query and bring the flat list into C# memory

            // --- Step 2: Perform the grouping and final summing in C# ---
            // This is now very fast because it's operating on an in-memory list.
            var monthlySummary = paidBillsForYear
                .GroupBy(b => b.PaymentMonth) // Group the results by month
                .Select(g => new MonthlySummaryViewModel
                {
                    Year = year,
                    Month = g.Key,
                    MonthName = new DateTime(year, g.Key, 1).ToString("MMM"),
                    // Sum up the total sales for all bills in that month
                    TotalSales = g.Sum(item => item.SaleAmount),
                    // Sum up the profit (ManualJobCost) for all bills in that month
                    TotalProfit = g.Sum(item => item.ProfitForJob)
                })
                .OrderBy(s => s.Month)
                .ToList();

            // --- Step 3: Ensure all 12 months are present, even if they have no sales ---
            var fullYearSummary = new List<MonthlySummaryViewModel>();
            for (int i = 1; i <= 12; i++)
            {
                var monthData = monthlySummary.FirstOrDefault(m => m.Month == i);
                if (monthData != null)
                {
                    fullYearSummary.Add(monthData);
                }
                else
                {
                    // Add a placeholder for months with no data
                    fullYearSummary.Add(new MonthlySummaryViewModel
                    {
                        Year = year,
                        Month = i,
                        MonthName = new DateTime(year, i, 1).ToString("MMM"),
                        TotalSales = 0,
                        TotalProfit = 0
                    });
                }
            }

            return fullYearSummary;
        }


        //a method to get jobs that need attention based on their status and a limit on the number of jobs returned(this method is called by the dashboard service)
        public async Task<List<JobAttentionViewModel>> GetJobsNeedingAttentionAsync(List<JobStatus> statuses ,int limit)
        {
            return await _dbContext.Jobs
            .Where(j => statuses.Contains(j.JobStatus))
            .OrderBy(j => j.CheckedInDate) // Show oldest first
            .Take(limit)
            .Select(j => new JobAttentionViewModel
                {
                    JobId = j.JobId,
                    CustomerName = j.Device.Customer.FirstName + " " + j.Device.Customer.LastName,
                    DeviceIdentifier = j.Device.Brand + " " + j.Device.Model,
                    Status = j.JobStatus.ToString(),
                    DaysOpen = (int)(DateTime.UtcNow - j.CheckedInDate).TotalDays
                })
            .ToListAsync();
        }





        //a method to calculate the total component charge for a given job by its id and return the total cost of components used in the job
        public async Task<decimal> GetTotalComponentChargeForJobAsync(int jobId)
        {
            //calculate the total component charge for a given job by its id

            // This query is extremely efficient. The database does all the work.
            var repairCosts = await _dbContext.RepairRecords
                .Where(rr => rr.JobId == jobId)
                .SelectMany(rr => rr.RepairRecordComponents)
                .SumAsync(rrc => (decimal?)rrc.QuantityUsed * rrc.PricePerUnit) ?? 0;

            var upgradeCosts = await _dbContext.UpgradeRecords
                .Where(ur => ur.JobId == jobId)
                .SelectMany(ur => ur.UpgradeRecordComponents)
                .SumAsync(urc => (decimal?)urc.QuantityUsed * urc.PricePerUnit) ?? 0;

            return repairCosts + upgradeCosts;

        }

        public Task<List<Job>> SearchJobsAsync(string searchTerm)
        {
            //search jobs by device model, customer name

            // The query uses EF.Functions.Like for efficient, case-insensitive partial matching
            // and .Contains for the phone number which is generally faster for non-text columns.
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Return an empty list immediately if search term is empty
                return Task.FromResult(new List<Job>());
            }

            searchTerm = searchTerm.Trim(); // Trim the search term once at the beginning

            return _dbContext.Jobs
                .Include(j => j.Device).ThenInclude(d => d.Customer) // Include Device and Customer navigation properties
                .Where(j => EF.Functions.Like(j.Device.Brand, $"%{searchTerm}%") || // Search by device brand
                            EF.Functions.Like(j.Device.Model, $"%{searchTerm}%") || // Search by device model
                            EF.Functions.Like(j.Device.Customer.FirstName, $"%{searchTerm}%") || // Search by customer first name
                            EF.Functions.Like(j.Device.Customer.LastName, $"%{searchTerm}%") || // Search by customer last name
                            j.Device.Customer.Phone.Contains(searchTerm) || // Search by customer phone number
                            j.Device.Customer.Email.Contains(searchTerm)) // Search by customer email
                .ToListAsync();

        }
        



        public Task<List<Job>> GetAllJobsWithDeviceAndCustomerInfoAsync()
        {
            //fetch all jobs with device and customer information
            return _dbContext.Jobs
                .Include(j => j.Device).ThenInclude(d => d.Customer) // Include Device and Customer navigation properties
                .ToListAsync();
        }



        // This method is not implemented yet, but it should return invoice data for a given job ID.
        //public async Task<InvoiceViewModel> GetInvoiceDataAsync(int jobId)
        //{

        //    // This is a read-only query, so AsNoTracking() is a great performance boost.
        //    var jobData = await _dbContext.Jobs
        //        .AsNoTracking()
        //        .Where(j => j.JobId == jobId)
        //        .Select(j => new // Project to an anonymous type first to keep the query clean
        //        {
        //            Job = j,
        //            Customer = j.Device.Customer,
        //            Device = j.Device,
        //            Billing = j.Billing,
        //            RepairLineItems = j.RepairRecords.Select(rr => new
        //            {
        //                RepairRecord = rr,
        //                Components = rr.RepairRecordComponents.Select(rrc => new { rrc.Component.Name, rrc.QuantityUsed, rrc.PricePerUnit })
        //            }),
        //            UpgradeLineItems = j.UpgradeRecords.Select(ur => new
        //            {
        //                UpgradeRecord = ur,
        //                Components = ur.UpgradeRecordComponents.Select(urc => new { urc.Component.Name, urc.QuantityUsed, urc.PricePerUnit, urc.WarrantyPeriodInMonths })
        //            }),
        //            ServiceLineItems = j.ServiceRecords.Select(sr => new { sr.ServiceDescription, sr.ServiceType })
        //        })
        //        .FirstOrDefaultAsync();

        //    if (jobData == null)
        //    {
        //        throw new KeyNotFoundException($"Job with ID {jobId} not found.");
        //    }

        //    // Now, build the final ViewModel in C# memory.
        //    var viewModel = new InvoiceViewModel
        //    {
        //        JobId = jobData.Job.JobId,
        //        billingId = jobData.Billing?.BillingId ?? 0, // Use null-coalescing to handle cases where Billing might be null
        //        InvoiceNumber = $"INV-{jobData.Billing?.BillingId:D5}",
        //        CustomerFullName = $"{jobData.Customer.FirstName} {jobData.Customer.LastName}",
        //        CustomerAddress = jobData.Customer.Address,
        //        CustomerPhone = jobData.Customer.Phone,
        //        CustomerEmail = jobData.Customer.Email,
        //        DeviceIdentifier = $"{jobData.Device.Brand} {jobData.Device.Model} (SN: {jobData.Device.SerialNumber})",
        //        ManualJobCost = jobData.Job.ManualJobCost ?? 0,
        //        GrandTotal = jobData.Billing?.Amount ?? 0,
        //        IsPaid = jobData.Billing?.IsPaid ?? false
        //    };

        //    // Populate Line Items
        //    foreach (var rr in jobData.RepairLineItems)
        //    {
        //        string warrantyText = GetWarrantyDisplayString(rr.RepairRecord.WarrantyExpiryDate, rr.RepairRecord.WarrantyPeriodinMonths);
        //        foreach (var comp in rr.Components)
        //        {
        //            viewModel.LineItems.Add(new InvoiceLineItem
        //            {
        //                Description = $"{rr.RepairRecord.ProblemDescription?.Truncate(30)}: {comp.Name}",
        //                Quantity = comp.QuantityUsed,
        //                UnitPrice = comp.PricePerUnit,
        //                LineTotal = comp.QuantityUsed * comp.PricePerUnit,
        //                WarrantyText = warrantyText
        //            });
        //        }
        //    }
        //    foreach (var ur in jobData.UpgradeLineItems)
        //    {
        //        foreach (var comp in ur.Components)
        //        {
        //            string warrantyText =  GetWarrantyDisplayString(null, comp.WarrantyPeriodInMonths); // Simplified for upgrade components
        //                                                                                               // A more advanced GetWarrantyDisplayString would be needed if it depends on Job dates
        //            viewModel.LineItems.Add(new InvoiceLineItem
        //            {
        //                Description = $"{ur.UpgradeRecord.UpgradeType} Upgrade: {comp.Name}",
        //                Quantity = comp.QuantityUsed,
        //                UnitPrice = comp.PricePerUnit,
        //                LineTotal = comp.QuantityUsed * comp.PricePerUnit,
        //                WarrantyText = warrantyText
        //            });
        //        }
        //    }
        //    foreach (var sr in jobData.ServiceLineItems)
        //    {
        //        viewModel.LineItems.Add(new InvoiceLineItem
        //        {
        //            Description = $"Service: {sr.ServiceDescription ?? sr.ServiceType.ToString()}",
        //            Quantity = 1,
        //            //UnitPrice = sr.Cost,
        //            //LineTotal = sr.Cost
        //        });
        //    }

        //    // Calculate subtotal and payment status text
        //    viewModel.Subtotal = viewModel.LineItems.Sum(li => li.LineTotal);
        //    viewModel.PaymentStatusText = viewModel.IsPaid ? "PAID" : "UNPAID";
        //    if (viewModel.IsPaid && jobData.Billing?.PaymentDate != null)
        //    {
        //        viewModel.PaymentDetailsText = $"Paid on {jobData.Billing.PaymentDate:yyyy-MM-dd} via {jobData.Billing.PaymentMethod}";
        //    }

        //    return viewModel;
        //}


        //private string GetWarrantyDisplayString(DateTime? warrantyExpiryDate, int? warrantyPeriodinMonths)
        //{
        //    if (warrantyExpiryDate.HasValue)
        //    {
        //        return $"Warranty until: {warrantyExpiryDate.Value:MM/dd/yyyy}";
        //    }
        //    // If warranty has a duration but hasn't started yet
        //    else if (warrantyPeriodinMonths.HasValue && warrantyPeriodinMonths > 0)
        //    {
        //        return $"Warranty: {warrantyPeriodinMonths.Value} months from pickup";
        //    }
        //    return "No Warranty";

        //}
    }
}
