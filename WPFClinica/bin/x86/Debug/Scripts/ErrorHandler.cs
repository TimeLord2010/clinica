using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClinica.Scripts {

    public static class ErrorHandler {

        //public static Action<Exception> OnError;

        public static void OnError (Exception ex, string additional_info = null) {
            MessageBox.Show($"{ex.Message}.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            StaticLogger.WriteLine(ex);
            if (additional_info != null) StaticLogger.WriteLine(additional_info);
        }

    }
}
