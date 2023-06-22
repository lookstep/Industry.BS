namespace Industry.BS.Data.Interfaces
{
    public interface IAllowedCashedDeviceCodeRepository
    {
        Task<List<AllowedDeviceCode>> CachedDeviceCodeService();
        Task AddAllowedDeviceCodesFromCash(string deviceCode);
        Task RemoveAllowedDeviceCodeFromCash(string deviceCode);
    }
}
