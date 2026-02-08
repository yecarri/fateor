namespace Fateor.Infrastructure.Services
{
    public interface IEncryptionService
    {
        T Decrypt<T>(byte[] encryptedBytes);
        T DecryptBase64<T>(string s, bool fromUrl = false);
        T DecryptHex<T>(string s); 
        byte[] DecryptBytes(byte[] encryptedBytes);        
        byte[] Encrypt(byte[] bytes);
        byte[] Encrypt<T>(T t);
        byte[] HexadecimalStringToBytes(string hex);
        string EncryptBase64<T>(T t, bool forUrl=false);
        string EncryptHex<T>(T t);
        string BytesToHexadecimalString(byte[] bytes);

    }
}