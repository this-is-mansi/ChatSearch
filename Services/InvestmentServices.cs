public class InvestmentService
{
    private static List<InvestmentPlan> plans = new List<InvestmentPlan>
    {
        new InvestmentPlan { Id = 1, Name = "SIP Growth Plan", MinAmount = 1000, Risk = "Medium" },
        new InvestmentPlan { Id = 2, Name = "Secure Fixed Deposit", MinAmount = 5000, Risk = "Low" },
        new InvestmentPlan { Id = 3, Name = "Equity Mutual Fund", MinAmount = 3000, Risk = "High" },
        new InvestmentPlan { Id = 4, Name = "Gold Investment Plan", MinAmount = 2000, Risk = "Low" }
    };

    public List<InvestmentPlan> GetPlans(int budget)
    {
        return plans.Where(p => p.MinAmount <= budget).ToList();
    }
}

public class InvestmentPlan
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MinAmount { get; set; }
    public string Risk { get; set; }
}