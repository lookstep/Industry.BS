using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industry.BS.Data.Interfaces
{
    public interface IDeviceCodeRepository
    {
        Task<List<AllowedDeviceCode>> GetAllAllowedDeviceCodes();
        Task AddAllowedDeviceCode(string deviceCode);
        Task RemoveAllowedDeviceCode(string deviceCode);
    }
}
