namespace Dota.API.Queries.GetAllAccounts;

public class AccountDto
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
}