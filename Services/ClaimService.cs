public class ClaimService
{
    private static List<Claim> claims = new List<Claim>
    {
        new Claim { Id = 1001, Customer = "Rahul", Status = "Approved", Amount = 20000 },
        new Claim { Id = 1002, Customer = "Anita", Status = "Pending", Amount = 15000 },
        new Claim { Id = 1003, Customer = "Ravi", Status = "Rejected", Amount = 10000 }
    };

    public Claim GetClaim(int id)
    {
        return claims.FirstOrDefault(c => c.Id == id);
    }
}

public class Claim
{
    public int Id { get; set; }
    public string Customer { get; set; }
    public string Status { get; set; }
    public int Amount { get; set; }
}