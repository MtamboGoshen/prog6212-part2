namespace ContractMonthlyClaim.Services
{
    public interface IEncryptionService
    {
        Task<byte[]> EncryptFileAsync(IFormFile file);
        Task<byte[]> DecryptFileAsync(byte[] encryptedData);
    }
}