﻿#pragma checksum "..\..\..\..\Pages\ProcedimentosAdministrador.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "89CFF3EE543F90914A5A487AE4609F6116084FC18D0CBE4A11FCA3F27DC4B0B5"
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
    /// ProcedimentosAdministrador
    /// </summary>
    public partial class ProcedimentosAdministrador : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ProcedimentoTB;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ConvenioTB;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ProcedimentosDG;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AdicionarB;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeletarB;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EditarB;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFClinica;component/pages/procedimentosadministrador.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
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
            this.ProcedimentoTB = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
            this.ProcedimentoTB.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ProcedimentoTB_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ConvenioTB = ((System.Windows.Controls.TextBox)(target));
            
            #line 27 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
            this.ConvenioTB.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ProcedimentoTB_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ProcedimentosDG = ((System.Windows.Controls.DataGrid)(target));
            
            #line 31 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
            this.ProcedimentosDG.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ProcedimentosDG_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AdicionarB = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
            this.AdicionarB.Click += new System.Windows.RoutedEventHandler(this.AdicionarB_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DeletarB = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
            this.DeletarB.Click += new System.Windows.RoutedEventHandler(this.DeletarB_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.EditarB = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\..\Pages\ProcedimentosAdministrador.xaml"
            this.EditarB.Click += new System.Windows.RoutedEventHandler(this.EditarB_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
