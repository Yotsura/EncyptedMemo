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
using System.Windows.Shapes;

namespace EncryptedMemo
{
    /// <summary>
    /// Sign.xaml の相互作用ロジック
    /// </summary>
    public partial class SignWindow : MetroWindow
    {
        public SignWindow()
        {
            InitializeComponent();
        }

        private void ForcusTxt(object sender, EventArgs e)
        {
            Pass1.Focus();
        }
    }
}
