using Core.Models;

namespace EdisonProjectTest01
{
    public class WarrantyCalculation
    {
        [Fact] // xUnit attribute for a test method
        public void WarrantyStartDate_WhenCollectedWithin7Days_ShouldBeCollectionDate()
        {
            // ARRANGE - Set up the test data
            var repair = new RepairRecord
            {
                WarrantyPeriodInMonths = 1,
                Job = new Job
                {
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
                WarrantyPeriodInMonths = 1,
                Job = new Job
                {
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