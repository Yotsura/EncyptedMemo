using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EncryptedMemo
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            _mwvm = new MainWindowViewModel();
            this.DataContext = _mwvm;

            if (Settings.Default.MainWindowStat == null) return;
            Top = Settings.Default.MainWindowStat.Top;
            Left = Settings.Default.MainWindowStat.Left;
            Width = Settings.Default.MainWindowStat.Width;
            Height = Settings.Default.MainWindowStat.Height;
        }

        MainWindowViewModel _mwvm;

        private void ForcusTxt(object sender, EventArgs e)
        {
            MemoTxt.Focus();
        }

        public void TaskTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mwvm.Memo.SaveData();
        }
        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            Settings.Default.MainWindowStat = new WindowStat { Height = Height, Width = Width, Left = Left, Top = Top };
            Settings.Default.Save();
        }
    }
}
