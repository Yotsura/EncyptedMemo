using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EncryptedMemo.Funcs
{
    class TxtFuncs
    {
        public static void InheritLineHead(object sender, KeyEventArgs e, List<char> headSpace, List<char> headMark)
        {
            if (e.Key != Key.Enter) return;
            if (headSpace == null || headMark == null) return;
            if (headSpace.Count < 1 || headMark.Count < 1) return;
            var calet = ((TextBox)sender).SelectionStart;
            var txt = ((TextBox)sender).Text;
            var foreTxt = txt.Substring(0, calet);
            if (!foreTxt.EndsWith("\r\n")) return;

            var lines = foreTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            var lastLine = lines[lines.Count - 2];
            var exTxt = "";

            foreach (var t in lastLine.ToCharArray())
            {
                if (headSpace.Contains(t)) { exTxt += t; continue; }
                if (!headMark.Contains(t)) break;
                exTxt += t;
                break;
            }

            ((TextBox)sender).Text = foreTxt + exTxt + txt.Substring(calet);
            ((TextBox)sender).SelectionStart = calet + exTxt.Length;
        }
        //private static List<char> HeadSpace = new List<char> { ' ', '　', '\t', };
        //private static List<char> HeadMark = new List<char> { '〇', '・', '#', '＞', '>' };
    }
}
