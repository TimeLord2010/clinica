using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFClinica.Scripts {
    class AcceptCPF {

        TextBox CPFTB;

        bool IsEditing = false;

        string Old_cpf = "";

        string CPF {
            get => CPFTB.Text;
            set => CPFTB.Text = value;
        }

        public AcceptCPF (TextBox tb) {
            CPFTB = tb;
            tb.TextChanged += Tb_TextChanged;
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e) {
            var CPFTB = sender as TextBox;
            if (IsEditing) return;
            IsEditing = true;
            int ci = CPFTB.CaretIndex;
            int len = CPF.Length;
            CPF = CPF.Replace("-", "");
            len -= CPF.Length;
            ci -= len;
            len = CPF.Length;
            CPF = CPF.Replace(".", "");
            len -= CPF.Length;
            ci -= len;
            string n = "";
            for (int i = 0; i < CPF.Length; i++) {
                string c = CPF[i] + "";
                if (Data.Any((ii) => ii == i, 3, 6, 9)) {
                    if (i == 9) {
                        if (c != "-") {
                            ci++;
                            n += "-";
                        }
                    } else {
                        if (c != ".") {
                            ci++;
                            n += ".";
                        }
                    }
                }
                n += c;
            }
            CPF = n;
            CPFTB.CaretIndex = ci < 0 ? 0 : ci;
            if (Old_cpf == CPF) {
                string _n = "";
                for (int i = 0; i < CPF.Length; i++) {
                    if (i == ci - 1) continue;
                    _n += CPF[i];
                }
                CPF = _n;
            }
            Old_cpf = CPF;
            IsEditing = false;
        }
    }
}
