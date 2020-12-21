using System;
namespace AirQualityService.Helpers.@interface
{
    public interface IRsaHelper
    {
        string Encrypt(string text);
        string Decrypt(string encrypted);
    }
}
