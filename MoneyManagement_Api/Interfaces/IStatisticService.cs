using MoneyManagement_Api.Models.Statistics;

namespace MoneyManagement_Api.Interfaces;

public interface IStatisticService
{
    Task<Dashboard> GetDashboard();
    Task<List<SalaryStatistics>> GetSalaryStatistic(int userId);
}