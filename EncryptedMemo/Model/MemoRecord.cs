using System.IO;
using EncryptedMemo.Funcs;

namespace EncryptedMemo
{
    public class MemoRecord
    {
        private EncryptUtils encrypt;
        public string Txt { get; set; } = string.Empty;

        public MemoRecord(string pass)
        {
            encrypt = new EncryptUtils(pass);
            if (!encrypt.CheckKey())
                encrypt.UpdateKey();
        }

        public void SaveData()
        {
            Settings.Default.Data = encrypt.AesEncrypt(Txt);
            Settings.Default.Save();
        }

        public void OpenData()
        {
            var encrypted = Settings.Default.Data;
            Txt = string.IsNullOrEmpty(encrypted) ? string.Empty : encrypt.AesDecrypt(encrypted);
        }

        public void UpdatePass(string pass)
        {
            encrypt.UpdatePass(pass);
            SaveData();
        }
    }
}
