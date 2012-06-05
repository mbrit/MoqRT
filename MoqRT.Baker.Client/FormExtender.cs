using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqRT.Baking.Client
{
    internal static class FormExtender
    {
        internal static void ShowMessage(this Form form, string message)
        {
            MessageBox.Show(form, message, "WinRT Baker");
        }
    }
}
