   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      $    WPFClinica.Pages.HistoricoAsos   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  HistoricoAsos��5   0    � 	Resources 5	       �� 
TargetType �� BasedOn �� Property �� Value��%�   '��       7��'�� z     7���� 5
   
   " ��6   8   .�5      ��  � ��$ 12(�6)   66   �� 5       � ��$ 5q�6'   63   5      �� 5   
   " ��6   8   .�5      ��  � ��$ 5,22,5,5q�6'   6:   5      5      .��5      � 
������� � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��50 �6   �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   � .+�5      �� 
Paciente6"   �� 5      -   5         ��Name$ NomePacienteTB��5      60   5       � 5!   
   $��1��6   .+�5"      �� 	Empresa6!   �� 5#      -   5        $ NomeEmpresaTB��5#      6/   5$      � 5%   
   $��2��6   .+�5&      �� CNPJ6   �� 5'      -   5        $ CNPJ_TB��5'      6)   5(      � 5)   
   $��1��6   .+�5*      �� Inicio6    �� 5+      -   5        $ InicioTB��5+      6*   5,      � 5-   
   $��1��6   $��1��6   .+�5.      �� Fim6   �� 5/      -   5        $ FimTB��5/      6'   50      � 51   
   $��1��6   $��2��6   $��Center=�6,   $��Center=�6I   .+�52      �� -   5        	 ��Name$	 
PesquisarB��52      .��6B   	Pesquisar6M   53      � 54   
   $��2��6   $��3��6   $��5q�60   .+�55      &   System.Windows.Controls.DataGrid  -   5        
  Name$
 ASO_DG��55      6)   56      57      58      