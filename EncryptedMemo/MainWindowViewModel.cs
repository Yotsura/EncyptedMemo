using System;
using System.ComponentModel;

namespace EncryptedMemo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MemoRecord _memo;
        public MemoRecord Memo
        {
            get => _memo;
            set
            {
                _memo = value;
                OnPropertyChanged(nameof(Memo));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            Memo = new MemoRecord();
            try
            {
                Memo.OpenData();
            }
            catch
            {
                //var errfile = Memo.Filepath.Replace("taskTxt.log", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}taskTxt.log");
                //System.IO.File.Copy(Memo.Filepath, errfile);
                Memo.Txt = $"データファイルの展開に失敗。";
            }
        }
    }
}
