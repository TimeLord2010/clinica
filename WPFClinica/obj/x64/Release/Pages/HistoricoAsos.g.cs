﻿#pragma checksum "..\..\..\..\Pages\HistoricoAsos.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "72941D4E37D4C4FE55988FD1E35158C3BDC9A867EF9EE5E13206CA93D79C8BCB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPFClinica.Pages;


namespace WPFClinica.Pages {
    
    
    /// <summary>
    /// HistoricoAsos
    /// </summary>
    public partial class HistoricoAsos : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\Pages\HistoricoAsos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NomePacienteTB;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Pages\HistoricoAsos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NomeEmpresaTB;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Pages\HistoricoAsos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CNPJ_TB;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Pages\HistoricoAsos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InicioTB;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Pages\HistoricoAsos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FimTB;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Pages\HistoricoAsos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PesquisarB;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\Pages\HistoricoAsos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ASO_DG;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPFClinica;component/pages/historicoasos.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\HistoricoAsos.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.NomePacienteTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.NomeEmpresaTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.CNPJ_TB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.InicioTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.FimTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.PesquisarB = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\Pages\HistoricoAsos.xaml"
            this.PesquisarB.Click += new System.Windows.RoutedEventHandler(this.PesquisarB_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ASO_DG = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

