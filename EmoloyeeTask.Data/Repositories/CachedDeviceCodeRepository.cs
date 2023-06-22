using Industry.BS.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industry.BS.Data.Repositories
{
    public class CachedDeviceCodeRepository : IAllowedCashedDeviceCodeRepository
    {
        private readonly IDeviceCodeRepository _deviceCodeRepository;
        private readonly IMemoryCache _cache;

        public CachedDeviceCodeRepository(IDeviceCodeRepository deviceCodeRepository, IMemoryCache cache)
        {
            _deviceCodeRepository = deviceCodeRepository;
            _cache = cache;
        }
        public async Task<List<AllowedDeviceCode>> CachedDeviceCodeService()
        {
            if (!_cache.TryGetValue("DeviceCode", out List<AllowedDeviceCode> allowedDeviceCode))
            {
                allowedDeviceCode = await _deviceCodeRepository.GetAllAllowedDeviceCodes();
                _cache.Set("DeviceCode", allowedDeviceCode, TimeSpan.FromHours(2));
            }

            return allowedDeviceCode;
        }

        public async Task AddAllowedDeviceCodesFromCash(string deviceCode)
        {
            await _deviceCodeRepository.AddAllowedDeviceCode(deviceCode);
            _cache.Remove("DeviceCode");
        }

        public async Task RemoveAllowedDeviceCodeFromCash(string deviceCode)
        {
            await _deviceCodeRepository.RemoveAllowedDeviceCode(deviceCode);
            _cache.Remove("DeviceCode");
        }
    }
}
