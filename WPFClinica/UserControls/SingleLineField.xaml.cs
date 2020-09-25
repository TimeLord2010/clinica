using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WPFClinica.UserControls {

    public partial class SingleLineField : UserControl {

        public SingleLineField() {
            InitializeComponent();
            GotFocus += SingleLineField_GotFocus;
            LostFocus += SingleLineField_LostFocus;
        }

        public double OriginalHeight { get; set; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Header { get => HeaderTBL.Text; set => HeaderTBL.Text = value; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Text { get => FieldTB.Text; set => FieldTB.Text = value; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsReadOnly { get => FieldTB.IsReadOnly; set => FieldTB.IsReadOnly = value; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double ExpandUpTo { get; set; } = Double.NaN;

        public int FocusedHeight { get; set; }

        private void SingleLineField_GotFocus(object sender, RoutedEventArgs e) {
            OriginalHeight = Height;
            ChangeHeight(ExpandUpTo);
        }

        private void SingleLineField_LostFocus(object sender, RoutedEventArgs e) {
            if (OriginalHeight != 0 && !double.IsNaN(OriginalHeight)) {
                ChangeHeight(OriginalHeight);
            }
        }

        void ChangeHeight(double h) {
            if (double.IsNaN(h)) {
                return;
            }
            bool increasing = Height < h ? true : false;
            Action action = null;
            Func<bool> stop = null;
            int inc = 5;
            if (increasing) {
                action = () => Height += inc;
                stop = () => Height >= h;
            } else {
                action = () => Height -= inc;
                stop = () => Height <= h;
            }
            void _action() {
                Dispatcher.Invoke(() => {
                    action.Invoke();
                });
            }
            bool _stop() {
                bool r = false;
                Dispatcher.Invoke(() => {
                    r = stop.Invoke();
                });
                return r;
            }
            ChangeGradually(_action, _stop, TimeSpan.FromMilliseconds(10));
        }

        void ChangeGradually(Action action, Func<bool> stop, TimeSpan tick) {
            var t = new Thread(() => {
                while (!stop.Invoke()) {
                    action.Invoke();
                    Thread.Sleep((int)tick.TotalMilliseconds);
                }
            });
            t.Start();
        }

    }
}
