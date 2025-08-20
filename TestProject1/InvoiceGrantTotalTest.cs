using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    
    public class InvoiceGrantTotalTest // This class is intended to test the Invoice Grant Total functionality
    {
        //Arrange setup the test data
        [Fact]
        public void CalculateGrandTotal_ForJobWithOnlyRepair_CalculatesCorrectly()
        {
            // ARRANGE - Set up the test data for the job
            var job = new Job
            {
                CheckedInDate= DateTime.Now,
                ManualJobCost = 100.00m, // Labor cost
                RepairRecords = new List<RepairRecord>
        {
            new RepairRecord
            {
                RepairRecordComponents = new List<RepairRecordComponent>
                {
                    // Component 1: 2 RAM sticks at $50 each
                    new RepairRecordComponent { QuantityUsed = 2, PricePerUnit = 50.00m }, // Total: 100.00
                    // Component 2: 1 SSD at $75
                    new RepairRecordComponent { QuantityUsed = 1, PricePerUnit = 75.00m }  // Total: 75.00
                }
            }
        }
            };
            // Expected component total = 100.00 + 75.00 = 175.00
            // Expected grand total = 175.00 (components) + 100.00 (labor) = 275.00
            decimal expectedGrandTotal = 275.00m;

            // ACT - Execute the logic being tested
            // We simulate the calculation that would happen in your UI or service.
            decimal actualGrandTotal = CalculateInvoiceGrandTotal(job);

            // ASSERT - Verify the result
            Assert.Equal(expectedGrandTotal, actualGrandTotal);
        }




        //Scenario 2: Job with only an Upgrade Record (with components)

        [Fact]
        public void CalculateGrandTotal_ForJobWithOnlyUpgrade_CalculatesCorrectly()
        {
            // ARRANGE
            var job = new Job
            {
                CheckedInDate = DateTime.Now,
                ManualJobCost = 150.00m, // Labor cost for the upgrade
                UpgradeRecords = new List<UpgradeRecord>
        {
            new UpgradeRecord
            {
                UpgradeRecordComponents = new List<UpgradeRecordComponent>
                {
                    // Component 1: 1 GPU at $400
                    new UpgradeRecordComponent { QuantityUsed = 1, PricePerUnit = 400.00m }, // Total: 400.00
                    // Component 2: 1 PSU at $80
                    new UpgradeRecordComponent { QuantityUsed = 1, PricePerUnit = 80.00m }  // Total: 80.00
                }
            }
        }
            };
            // Expected component total = 400.00 + 80.00 = 480.00
            // Expected grand total = 480.00 (components) + 150.00 (labor) = 630.00
            decimal expectedGrandTotal = 630.00m;

            // ACT
            decimal actualGrandTotal = CalculateInvoiceGrandTotal(job);

            // ASSERT
            Assert.Equal(expectedGrandTotal, actualGrandTotal);
        }


        // Job with a Repair and a Service Record (no upgrade)

        [Fact]
        public void CalculateGrandTotal_ForJobWithRepairAndService_CalculatesCorrectly()
        {
            // ARRANGE
            var job = new Job
            {
                CheckedInDate = DateTime.Now,
                ManualJobCost = 75.00m, // Labor cost
                RepairRecords = new List<RepairRecord>
        {
            new RepairRecord
            {
                RepairRecordComponents = new List<RepairRecordComponent>
                {
                    new RepairRecordComponent { QuantityUsed = 2, PricePerUnit = 10.00m } // Repair Part: 20.00
                }
            }
        },
                ServiceRecords = new List<ServiceRecord>
        {
            new ServiceRecord {  } // there is no cost associated with services, it is added to the manual job cost
        }
            };
            // Expected component/service total = 20.00 (repair)
            // Expected grand total = 20.00 (tasks) + 75.00 (labor) = 95.00
            decimal expectedGrandTotal = 95.00m;

            // ACT
            decimal actualGrandTotal = CalculateInvoiceGrandTotal(job);

            // ASSERT
            Assert.Equal(expectedGrandTotal, actualGrandTotal);
        }



        // Job with Repair, Upgrade, and Service Records

        [Fact]
        public void CalculateGrandTotal_ForJobWithMixedTasks_CalculatesCorrectly()
        {
            // ARRANGE
            var job = new Job
            {
                CheckedInDate = DateTime.Now,
                ManualJobCost = 250.00m, // Overall labor cost for the entire job
                RepairRecords = new List<RepairRecord>
        {
            new RepairRecord
            {
                RepairRecordComponents = new List<RepairRecordComponent>
                {
                    new RepairRecordComponent { QuantityUsed = 1, PricePerUnit = 25.00m } // Repair Part: 25.00
                }
            }
        },
                UpgradeRecords = new List<UpgradeRecord>
        {
            new UpgradeRecord
            {
                UpgradeRecordComponents = new List<UpgradeRecordComponent>
                {
                    new UpgradeRecordComponent { QuantityUsed = 1, PricePerUnit = 300.00m } // Upgrade Part: 300.00
                }
            }
        },
                ServiceRecords = new List<ServiceRecord>
        {
            new ServiceRecord {  } // Direct Service Charge: 50.00
        }
            };
            // Expected component/service total = 25.00 (repair) + 300.00 (upgrade) = 325.00
            // Expected grand total = 325.00 (tasks) + 250.00 (labor) = 575.00
            decimal expectedGrandTotal = 575.00m;

            // ACT
            decimal actualGrandTotal = CalculateInvoiceGrandTotal(job);

            // ASSERT
            Assert.Equal(expectedGrandTotal, actualGrandTotal);
        }






































        //helper method to calculate the grand total
        private decimal CalculateInvoiceGrandTotal(Job job)
        {
            if (job == null)
            {
                return 0;
            }

            decimal totalComponentAndServiceCharge = 0;

            // Sum component costs from RepairRecords
            if (job.RepairRecords != null)
            {
                foreach (var rr in job.RepairRecords)
                {
                    if (rr.RepairRecordComponents != null)
                    {
                        totalComponentAndServiceCharge += rr.RepairRecordComponents.Sum(rrc => rrc.QuantityUsed * rrc.PricePerUnit);
                    }
                }
            }

            // Sum component costs from UpgradeRecords
            if (job.UpgradeRecords != null)
            {
                foreach (var ur in job.UpgradeRecords)
                {
                    if (ur.UpgradeRecordComponents != null)
                    {
                        totalComponentAndServiceCharge += ur.UpgradeRecordComponents.Sum(urc => urc.QuantityUsed * urc.PricePerUnit);
                    }
                }
            }

            // service record only takes the manualjobcost into account

            //if (job.ServiceRecords != null)
            //{
            //    totalComponentAndServiceCharge += job.ServiceRecords.Sum(sr => sr.Cost);
            //}

            // Add the manual labor/misc cost for the entire job
            decimal grandTotal = totalComponentAndServiceCharge + (job.ManualJobCost ?? 0);

            return grandTotal;
        }

    }
}
