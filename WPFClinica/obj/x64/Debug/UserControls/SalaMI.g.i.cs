﻿#pragma checksum "..\..\..\..\UserControls\SalaMI.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "18E595331AEE4FC598748C53010E7804B7DC1BB032C61561268B3D6DD077648C"
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
using WPFClinica.UserControls;


namespace WPFClinica.UserControls {
    
    
    /// <summary>
    /// SalaMI
    /// </summary>
    public partial class SalaMI : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\UserControls\SalaMI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem SalaMI;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\UserControls\SalaMI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock NomeTBL;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\UserControls\SalaMI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem MudarNomeMI;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\UserControls\SalaMI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton TriagemRB;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\UserControls\SalaMI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton ConsultorioRB;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\UserControls\SalaMI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton LaboratioRB;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFClinica;component/usercontrols/salami.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\SalaMI.xaml"
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
            this.SalaMI = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 2:
            this.NomeTBL = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.MudarNomeMI = ((System.Windows.Controls.MenuItem)(target));
            
            #line 14 "..\..\..\..\UserControls\SalaMI.xaml"
            this.MudarNomeMI.Click += new System.Windows.RoutedEventHandler(this.MudarNomeMI_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TriagemRB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 16 "..\..\..\..\UserControls\SalaMI.xaml"
            this.TriagemRB.Checked += new System.Windows.RoutedEventHandler(this.TriagemRB_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ConsultorioRB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 17 "..\..\..\..\UserControls\SalaMI.xaml"
            this.ConsultorioRB.Checked += new System.Windows.RoutedEventHandler(this.TriagemRB_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.LaboratioRB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 18 "..\..\..\..\UserControls\SalaMI.xaml"
            this.LaboratioRB.Checked += new System.Windows.RoutedEventHandler(this.TriagemRB_Checked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

