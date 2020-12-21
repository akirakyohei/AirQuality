using System;
using AirQualityService.Data.Interface;
using AirQualityService.Model;
using MongoDB.Driver;

namespace AirQualityService.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _account;
        public AccountRepository(AirQualityContext context)
        {
            _account = context.Account;
        }

        public Account GetAccount()
        {
            return _account.Find(x => true).FirstOrDefault();
        }

        public void InsertAccount(Account account)
        {
            _account.InsertOne(account);
        }

        public bool UpdateAccount(Account account)
        {
            var obj = _account.Find(x => x.UserId.Equals(account.UserId)).FirstOrDefault();
            if (obj == null) return false;

            var result = _account.ReplaceOne(x => x.UserId.Equals(account.UserId), account);
            return result.IsAcknowledged;
        }
    }
}
