using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ContractMonthlyClaim.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService()
        {
            // MODIFIED: We now create the byte arrays directly to bypass any encoding issues.
            // This guarantees a 32-byte key and a 16-byte IV.
            _key = new byte[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16,
                0x17, 0x18, 0x19, 0x20, 0x21, 0x22, 0x23, 0x24,
                0x25, 0x26, 0x27, 0x28, 0x29, 0x30, 0x31, 0x32
            };

            _iv = new byte[]
            {
                0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8,
                0xA9, 0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6
            };

            Debug.WriteLine($"[DIAGNOSTIC] EncryptionService constructor: Key length is {_key.Length} bytes.");
        }

        public async Task<byte[]> EncryptFileAsync(IFormFile file)
        {
            using var aes = Aes.Create();
            aes.Key = _key; // This will now work.
            aes.IV = _iv;

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            await file.CopyToAsync(cryptoStream);
            await cryptoStream.FlushFinalBlockAsync();

            return memoryStream.ToArray();
        }

        public async Task<byte[]> DecryptFileAsync(byte[] encryptedData)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var inputStream = new MemoryStream(encryptedData);
            using var outputStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);

            await cryptoStream.CopyToAsync(outputStream);

            return outputStream.ToArray();
        }
    }
}