﻿#pragma checksum "..\..\..\..\Windows\EscolherSala.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "51F51AAAC71EFF1E02F754C1E1AD73196282157EDF815E224566EA165031E15F"
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
using WPFClinica.Windows;


namespace WPFClinica.Windows {
    
    
    /// <summary>
    /// EscolherSala
    /// </summary>
    public partial class EscolherSala : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FuncionarioTB;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SalaTB;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FuncaoCB;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox StatusCB;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox EspecializacaoCB;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid SalasDG;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid EspecializacoesDG;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Windows\EscolherSala.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OkB;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFClinica;component/windows/escolhersala.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\EscolherSala.xaml"
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
            this.FuncionarioTB = ((System.Windows.Controls.TextBox)(target));
            
            #line 27 "..\..\..\..\Windows\EscolherSala.xaml"
            this.FuncionarioTB.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.FuncionarioTB_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SalaTB = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\..\..\Windows\EscolherSala.xaml"
            this.SalaTB.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SalaTB_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.FuncaoCB = ((System.Windows.Controls.ComboBox)(target));
            
            #line 35 "..\..\..\..\Windows\EscolherSala.xaml"
            this.FuncaoCB.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.FuncaoCB_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.StatusCB = ((System.Windows.Controls.ComboBox)(target));
            
            #line 43 "..\..\..\..\Windows\EscolherSala.xaml"
            this.StatusCB.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.StatusCB_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.EspecializacaoCB = ((System.Windows.Controls.ComboBox)(target));
            
            #line 52 "..\..\..\..\Windows\EscolherSala.xaml"
            this.EspecializacaoCB.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.EspecializacaoCB_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SalasDG = ((System.Windows.Controls.DataGrid)(target));
            
            #line 62 "..\..\..\..\Windows\EscolherSala.xaml"
            this.SalasDG.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SalasDG_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.EspecializacoesDG = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.OkB = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\..\..\Windows\EscolherSala.xaml"
            this.OkB.Click += new System.Windows.RoutedEventHandler(this.OkB_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

