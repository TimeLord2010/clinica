﻿#pragma checksum "..\..\..\..\Windows\EscolherConvenio2.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3BDE1AE2B18C067F296E77781FD1003221A244D81B72EB6589F1CE8737152C6B"
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
    /// EscolherConvenio2
    /// </summary>
    public partial class EscolherConvenio2 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\Windows\EscolherConvenio2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ConveniosTB;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Windows\EscolherConvenio2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ConveniosDG;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Windows\EscolherConvenio2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelecionarB;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFClinica;component/windows/escolherconvenio2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\EscolherConvenio2.xaml"
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
            this.ConveniosTB = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\..\Windows\EscolherConvenio2.xaml"
            this.ConveniosTB.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ConveniosTB_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ConveniosDG = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.SelecionarB = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\..\Windows\EscolherConvenio2.xaml"
            this.SelecionarB.Click += new System.Windows.RoutedEventHandler(this.SelecionarB_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
