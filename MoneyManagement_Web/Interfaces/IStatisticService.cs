using MoneyManagement_Web.Data.Statistics;

namespace MoneyManagement_Web.Interfaces
{
    public interface IStatisticService
    {
        Task<Dashboard> GetDashboard();
        Task<IList<BalanceLineChart>> GetBalanceLineChart();

        Task<List<SalaryStats>> GetSalaryStatistic(int userId);
    }
}
