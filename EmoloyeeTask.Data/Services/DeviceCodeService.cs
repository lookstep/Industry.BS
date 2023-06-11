using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoloyeeTask.Data.Services
{
    public class DeviceCodeService
    {
        private readonly AppDbContext _db;

        public DeviceCodeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<AllowedDeviceCode>> GetAllowedDevicesCode()
        {
            return await _db.AllowedDeviceCode.ToListAsync();
        }

        public async Task AddDeviceCode(string deviceCode)
        {
            var allowedIP = new AllowedDeviceCode { DeviceCode = deviceCode };
            _db.AllowedDeviceCode.Add(allowedIP);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveDeviceCode(string deviceCode)
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
