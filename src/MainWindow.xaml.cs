using System;
using System.Globalization;
using System.Windows;

namespace BullshotLauncher
{
    /// <summary>
    /// Main window for Bullshot Token Launcher.
    /// Handles UI events, tracker integration, and dashboard updates.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BondingCurveTracker _curveTracker = new BondingCurveTracker();
        private readonly FeeCalculator _feeCalculator = new FeeCalculator();
        private readonly QuestManager _questManager = new QuestManager();

        public MainWindow()
        {
            InitializeComponent();
            RefreshQuests();
            UpdateStatus("Ready");
        }

        private async void BtnTrack_Click(object sender, RoutedEventArgs e)
        {
            var address = txtTokenAddress.Text.Trim();
            if (string.IsNullOrEmpty(address))
            {
                txtStatus.Text = "Token address required";
                return;
            }

            btnTrack.IsEnabled = false;
            UpdateStatus("Fetching curve data...");

            try
            {
                var progress = await _curveTracker.GetProgressAsync(address);
                var velocity = await _curveTracker.GetVelocityAsync(address);

                txtCurveResult.Text =
                    $"Percent Filled: {progress.PercentFilled}%\n" +
                    $"Estimated Migration: {progress.EstimatedMigration.TotalHours:F1} hours\n" +
                    $"Liquidity Locked: {progress.LiquidityLocked}\n" +
                    $"Fill Velocity: {velocity:N1}% per hour";

                UpdateStatus("Curve data updated");
            }
            catch (Exception ex)
            {
                txtCurveResult.Text = $"Error: {ex.Message}";
                UpdateStatus("Error fetching curve data");
            }
            finally
            {
                btnTrack.IsEnabled = true;
            }
        }

        private void BtnCalculateFee_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtInitialPrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var initialPrice))
            {
                txtFeeResult.Text = "Invalid initial price";
                return;
            }

            if (!decimal.TryParse(txtCurrentPrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var currentPrice))
            {
                txtFeeResult.Text = "Invalid current price";
                return;
            }

            if (!decimal.TryParse(txtVolume.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var volume))
            {
                txtFeeResult.Text = "Invalid volume";
                return;
            }

            var result = _feeCalculator.EstimateCreatorFee(initialPrice, currentPrice, volume);

            txtFeeResult.Text =
                $"Fee Percentage: {result.FeePercentage}%\n" +
                $"Estimated Fee: {result.EstimatedFee:N2} Kinah\n" +
                $"Volume: {result.Volume:N2}";

            var scenarios = _feeCalculator.CompareScenarios(initialPrice, currentPrice);
            txtFeeResult.Text += "\n\nScenarios:\n";
            foreach (var s in scenarios)
            {
                txtFeeResult.Text += $"Volume {s.Volume:N0}: Fee {s.EstimatedFee:N2}\n";
            }
        }

        private void BtnRefreshQuests_Click(object sender, RoutedEventArgs e)
        {
            RefreshQuests();
            UpdateStatus("Quests refreshed");
        }

        private void RefreshQuests()
        {
            lstQuests.Items.Clear();
            foreach (var q in _questManager.GetQuests())
            {
                var status = q.IsCompleted ? "[Done]" : "[Pending]";
                lstQuests.Items.Add($"[{q.Type}] {q.Name} - {status}\n  {q.Description}");
            }
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }
    }
}