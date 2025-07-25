namespace MoneyManagement_Web.Data.Statistics;

public class Dashboard
{
    public IList<BalanceLineChart>? BalanceLineChar { get; set; }
    public IList<SpentChart>? SpentChar { get; set; }
    public IList<SalaryChart>? SalaryCharts { get; set; }
    public IList<BalancesSummary>? BalancesSummary { get; set; }
}