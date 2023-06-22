using EmoloyeeTask.Data;
using Industry.BS.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industry.BS.Data.Repositories
{
    public class DeviceCodeRepository : IDeviceCodeRepository
    {
        private readonly AppDbContext _db;

        public DeviceCodeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAllowedDeviceCode(string deviceCode)
        {
            var allowedIP = new AllowedDeviceCode { DeviceCode = deviceCode };
            _db.AllowedDeviceCode.Add(allowedIP);
            await _db.SaveChangesAsync();
        }

        public async Task<List<AllowedDeviceCode>> GetAllAllowedDeviceCodes()
        {
            return await _db.AllowedDeviceCode.ToListAsync();
        }

        public async Task RemoveAllowedDeviceCode(string deviceCode)
        {
            var allowedIP = await _db.AllowedDeviceCode.FirstOrDefaultAsync(i => i.DeviceCode == deviceCode);
            if (allowedIP != null)
            {
                _db.AllowedDeviceCode.Remove(allowedIP);
                await _db.SaveChangesAsync();
            }
        }
    }
}
