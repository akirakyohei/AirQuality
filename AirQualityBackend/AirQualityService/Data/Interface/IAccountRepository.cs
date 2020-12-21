using System;
using AirQualityService.Model;

namespace AirQualityService.Data.Interface
{
    public interface IAccountRepository
    {
        public Account GetAccount();
        public void InsertAccount(Account account);
        public bool UpdateAccount(Account account);
    }
}
