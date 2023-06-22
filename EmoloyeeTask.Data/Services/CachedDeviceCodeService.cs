using Microsoft.Extensions.Caching.Memory;

namespace EmoloyeeTask.Data.Services
{
    public class CachedDeviceCodeService
    {
        private readonly DeviceCodeService _allowedDeviceCode;
        private readonly IMemoryCache _cache;

        public CachedDeviceCodeService(DeviceCodeService allowedDeviceCode, IMemoryCache cache)
        {
            _allowedDeviceCode = allowedDeviceCode;
            _cache = cache;
        }

        public async Task<List<AllowedDeviceCode>> GetAllowedDeviceCode()
        {
            if (!_cache.TryGetValue("AllowedDeviceCode", out List<AllowedDeviceCode> allowedDeviceCode))
            {
                allowedDeviceCode = await _allowedDeviceCode.GetAllowedDevicesCode();
                _cache.Set("AllowedDeviceCode", allowedDeviceCode, TimeSpan.FromHours(2));
            }

            return allowedDeviceCode;
        }

        public async Task AddAllowedDeviceCode(string deviceCode)
        {
            await _allowedDeviceCode.AddDeviceCode(deviceCode);
            _cache.Remove("DeviceCode");
        }

        public async Task RemoveAllowedDeviceCode(string deviceCode)
        {
            await _allowedDeviceCode.RemoveDeviceCode(deviceCode);
            _cache.Remove("DeviceCode");
        }
    }
}
