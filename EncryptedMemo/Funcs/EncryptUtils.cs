using System;
using System.Text;
using System.Security.Cryptography;

namespace EncryptedMemo.Funcs
{
    internal static class EncryptUtils
    {
        private static readonly int KeySize = 256;
        private static readonly int BlockSize = 128;
        private static string _nextKey;
        private static byte[] _entropy = Convert.FromBase64String(Settings.Default.Entropy)
            ?? new byte[] { 0x72, 0xa2, 0x12, 0x04 };
        private const string EnvKeyName = "MemoKey";

        internal static bool CheckKey()
        {
            if (_entropy == null) return false;
            var encryptedKey = Environment.GetEnvironmentVariable(EnvKeyName, EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(encryptedKey)) return false;
            try
            {
                //環境変数の鍵と設定のエントロピーの組み合わせが正しいか確認
                var test = DpapiDecrypt(encryptedKey);
            }
            catch
            {
                return false;
            }
            return true;
        }
        internal static void UpdateKey()
        {
            //dpapi用エントロピーの更新
            byte[] random = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(random);
            _entropy = random;
            Settings.Default.Entropy = Convert.ToBase64String(random);
            Settings.Default.Save();
            //aesの鍵の更新
            _nextKey = DpapiEncrypt(System.Web.Security.Membership.GeneratePassword(32, 0));
            Environment.SetEnvironmentVariable(EnvKeyName, _nextKey, EnvironmentVariableTarget.User);
        }

        internal static string AesEncrypt(string value)
        {
            using (var aes = GetAesManaged())
            {
                //先頭に16byte分ダミー挿入
                var byteValue = Encoding.UTF8.GetBytes("0123456789ABCDEF" + value);
                var encryptor = aes.CreateEncryptor();
                var encryptedValue = encryptor.TransformFinalBlock(byteValue, 0, byteValue.Length);
                return Convert.ToBase64String(encryptedValue);
            }
        }

        internal static string AesDecrypt(string encryptedValue)
        {
            using (var aes = GetAesManaged())
            {
                var byteValue = Convert.FromBase64String(encryptedValue);
                var decryptor = aes.CreateDecryptor();
                var decryptValue = decryptor.TransformFinalBlock(byteValue, 0, byteValue.Length);
                UpdateKey();    //復号直後に鍵を更新する。
                //IVが変わっているので復号できない先頭16byteを切り捨てる
                return Encoding.UTF8.GetString(decryptValue, 16, decryptValue.Length - 16);
            }
        }

        private static AesManaged GetAesManaged()
        {
            var aes = new AesManaged
            {
                KeySize = KeySize,
                BlockSize = BlockSize,
                Mode = CipherMode.CBC
            };
            //IV初期ベクトルはあくまで同じ平文・鍵で別の暗号文を生成するためのもの。
            aes.IV = Encoding.UTF8.GetBytes(System.Web.Security.Membership.GeneratePassword(16, 0));
            var encryptedKey = Environment.GetEnvironmentVariable(EnvKeyName, EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(encryptedKey))
            {
                //環境変数がなければ作成・設定する
                UpdateKey();
                encryptedKey = _nextKey;
            }
            aes.Key = Encoding.UTF8.GetBytes(DpapiDecrypt(encryptedKey));
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        private static string DpapiEncrypt(string value)
        {
            //文字列をバイト型配列に変換
            byte[] userData = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] encryptedData = ProtectedData.Protect(userData, _entropy, DataProtectionScope.CurrentUser);
            //暗号化されたデータを文字列に変換
            return System.Convert.ToBase64String(encryptedData);
        }

        private static string DpapiDecrypt(string encryptedValue)
        {
            //文字列を暗号化されたデータに戻す
            byte[] encryptedData = System.Convert.FromBase64String(encryptedValue);
            byte[] userData = ProtectedData.Unprotect(encryptedData, _entropy, DataProtectionScope.CurrentUser);
            //復号化されたデータを文字列に変換
            return System.Text.Encoding.UTF8.GetString(userData);
        }
    }
}