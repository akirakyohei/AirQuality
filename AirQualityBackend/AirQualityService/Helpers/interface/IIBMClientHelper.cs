using System;
using System.Threading.Tasks;

namespace AirQualityService.Helpers.@interface
{
    public interface IIBMClientHelper
    {
        public Task<string> registerDevice(string deviceId);

        public Task<string> removeDevice(string deviceId);

        public Task<string> LogConnection(string deviceId);

        public Task<string> LogDiagDevice(string deviceId);
    }
}
