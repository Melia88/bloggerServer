using System.Data;
using bloggerServer.Models;
using Dapper;

namespace bloggerServer.Repositories
{
  public class AccountsRepository
  {
    private readonly IDbConnection _db;

    public AccountsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Account GetById(string id)
    {
      string sql = "SELECT * FROM accounts WHERE id = @id";
      return _db.QueryFirstOrDefault<Account>(sql, new { id });
    }

    internal Account Create(Account userInfo)
    {
      string sql = @"
      INSERT INTO accounts
      (id, name, picture, email)
      VALUES
      (@Id, @Name, @Picture, @Email)";
      _db.Execute(sql, userInfo);
      return userInfo;
    }
  }
}