using System.IO;

namespace EncryptedMemo
{
    public class MemoRecord
    {
        public string Txt { get; set; } = string.Empty;

        public void SaveData()
        {
            Settings.Default.Data = Funcs.EncryptUtils.AesEncrypt(Txt);
            Settings.Default.Save();
        }

        public void OpenData()
        {
            var encrypted = Settings.Default.Data;
            Txt = string.IsNullOrEmpty(encrypted) ? string.Empty : Funcs.EncryptUtils.AesDecrypt(encrypted);
        }
    }
}
