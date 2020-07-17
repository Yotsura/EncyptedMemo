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
            pass = "@2+あしいgasJ";
            encrypt = new EncryptUtils(pass);
        }

        public void SaveData()
        {
            Settings.Default.Data = encrypt.AesEncrypt(Txt);
            Settings.Default.Save();
        }

        public void OpenData()
        {
            if (!encrypt.CheckKey())
                encrypt.UpdateKey();

            var encrypted = Settings.Default.Data;
            Txt = string.IsNullOrEmpty(encrypted) ? string.Empty : encrypt.AesDecrypt(encrypted);
        }

        private void UpdatePass(string pass)
        {
            encrypt = new EncryptUtils(pass);
        }
    }
}
