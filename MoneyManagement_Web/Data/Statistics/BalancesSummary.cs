namespace MoneyManagement_Web.Data.Statistics
{
    public class BalancesSummary
    {
        public string? AccountName { get; set; }

        public DateTime BalanceDate { get; set; }


        public double BalanceAmount { get; set; }
        public string? Currency { get; set; }
        public string? AccountType { get; set; }

        public double DifferenceAmount { get; set; }

        public double TotAmount { get; set; }
        public double TotDiff { get; set; }

        public double GranTotalAmountEur { get; set; }
        public double GranTotalDiffEur { get; set; }

    }
}
