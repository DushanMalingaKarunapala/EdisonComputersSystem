using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class WarrantyCalculationTests
    {

        [Fact] // xUnit attribute for a test method
        public void WarrantyStartDate_WhenCollectedWithin7Days_ShouldBeCollectionDate()
        {
            // ARRANGE - Set up the test data
            var repair = new RepairRecord
            {
                WarrantyPeriodinMonths = 1,
                Job = new Job
                {   
                    CheckedInDate = new DateTime(2025, 6, 20),
                    CompletionDate = new DateTime(2025, 6, 26),
                    CollectionDate = new DateTime(2025, 6, 30),
                    JobStatus = JobStatus.Collected
                }
            };
            var expectedStartDate = new DateTime(2025, 6, 30);

            // ACT - Execute the logic being tested
            var actualStartDate = repair.WarrantyStartDate;

            // ASSERT - Verify the result is correct
            Assert.NotNull(actualStartDate);
            Assert.Equal(expectedStartDate, actualStartDate.Value);
        }

        [Fact]
        public void WarrantyStartDate_WhenCollectedAfter7Days_ShouldBeCompletionDatePlus7()
        {
            // ARRANGE
            var repair = new RepairRecord
            {
                WarrantyPeriodinMonths = 1,
                Job = new Job
                {
                    CheckedInDate = new DateTime(2025, 6, 20),

                    CompletionDate = new DateTime(2025, 6, 16),
                    CollectionDate = new DateTime(2025, 6, 30),
                    JobStatus = JobStatus.Collected
                }
            };
            var expectedStartDate = new DateTime(2025, 6, 23); // 16 + 7

            // ACT
            var actualStartDate = repair.WarrantyStartDate;

            // ASSERT
            Assert.NotNull(actualStartDate);
            Assert.Equal(expectedStartDate, actualStartDate.Value);
        }
    }
}
