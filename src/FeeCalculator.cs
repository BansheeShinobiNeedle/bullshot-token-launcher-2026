namespace BullshotLauncher
{
    /// <summary>
    /// Represents the result of a creator fee estimation.
    /// </summary>
    public class CreatorFeeEstimate
    {
        public decimal EstimatedFee { get; set; }
        public decimal FeePercentage { get; set; }
        public decimal Volume { get; set; }
    }

    /// <summary>
    /// Calculates creator fees for Bullshot memecoin launches.
    /// </summary>
    public class FeeCalculator
    {
        /// <summary>
        /// Base trading volume fee percentage (1%).
        /// </summary>
        public const decimal BaseFeePercentage = 1.0m;

        /// <summary>
        /// Estimates creator fee based on volume and price data.
        /// </summary>
        /// <param name="materialCost">Initial token price.</param>
        /// <param name="currentPrice">Current token price.</param>
        /// <param name="volume">Trading volume.</param>
        public CreatorFeeEstimate EstimateCreatorFee(decimal initialPrice, decimal currentPrice, decimal volume)
        {
            var feePercentage = BaseFeePercentage;
            var estimatedFee = volume * (feePercentage / 100m);

            return new CreatorFeeEstimate
            {
                EstimatedFee = estimatedFee,
                FeePercentage = feePercentage,
                Volume = volume
            };
        }

        /// <summary>
        /// Compares fee scenarios across low, medium, and high volume.
        /// </summary>
        public List<CreatorFeeEstimate> CompareScenarios(decimal initialPrice, decimal currentPrice)
        {
            var volumes = new[] { 10_000m, 50_000m, 100_000m };
            var results = new List<CreatorFeeEstimate>();

            foreach (var volume in volumes)
            {
                results.Add(EstimateCreatorFee(initialPrice, currentPrice, volume));
            }

            return results;
        }
    }
}