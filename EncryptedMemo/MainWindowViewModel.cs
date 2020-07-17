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
            Memo = new MemoRecord("");
            try
            {
                Memo.OpenData();
            }
            catch
            {
                Memo.Txt = $"データの復号に失敗。データは破棄されました。";
            }
        }
    }
}
