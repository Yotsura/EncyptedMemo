using MahApps.Metro.Controls;
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
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            OpenSignWindow();

            InitializeComponent();

            _mwvm = new MainWindowViewModel(_pass);
            this.DataContext = _mwvm;

            if (Settings.Default.MainWindowStat == null) return;
            this.Top = Settings.Default.MainWindowStat.Top;
            this.Left = Settings.Default.MainWindowStat.Left;
            this.Width = Settings.Default.MainWindowStat.Width;
            this.Height = Settings.Default.MainWindowStat.Height;
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
            Settings.Default.MainWindowStat = new WindowStat { Height = this.Height, Width = this.Width, Left = this.Left, Top = this.Top };
            Settings.Default.Save();
        }

        SignWindow _signwindow;
        private string _pass;
        private void OpenSignWindow()
        {
            _signwindow = new SignWindow();
            _signwindow.Btn.Click += SignInvoke;
            _signwindow.Pass1.KeyDown += Pass1_KeyDown;
            _signwindow.Closed += SettingWindow_Closed;
            _signwindow.ShowDialog();
        }
        private void SettingWindow_Closed(object sender, EventArgs e)
        {
            _signwindow = null;
        }
        private void SignInvoke(object sender, RoutedEventArgs e)
        {
            _pass = _signwindow.Pass1.Password;
            _signwindow.Close();
        }

        private void Pass1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            SignInvoke(sender,e);
        }
    }
}
