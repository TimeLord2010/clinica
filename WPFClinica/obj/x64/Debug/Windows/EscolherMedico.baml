   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  5   	   '    !WPFClinica.Windows.EscolherMedico   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WPFClinica.Windows     5   	     � Title$  Escolher Médico��5   	   $	��400��6!   $	��400��6.   .��5	      � -   5         �Name$ MyGrid��5	       � RowDefinitions 5
   
   �� 5      $��55 �6   6*   �� 5      6   �� 5      $��45 �6   6*    � ColumnDefinitions 5   
   �� 5      $��1* �6   6,   �� 5      $	��200 �6   6-   .+�5   
   �� $��5q�6   
�������Nome67   �� 5   
   -   5        $��NomeTB��5   2   $��5,25,5,5q�6   $��15(�6$   6c   �� 5   
   $��1��6   $��5q�6$   
�������Especialização6Q   �� 5   
   -   5        $��EspecializacaoCB��5   5   $��1��6   $��5,25,5,5q�6#   6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��	MedicosDG��5   ?   $��1��6   $��2��6    $��5q�64   6�   �� 5   
   -   5        $��SelecionarB��5   �   $��2��6   $��1��6   $��Right=�6.   $��Bottom=�6J   $��5q�6e   $��5,3,5,3q�6p   $��15(�6�   .��6�   
Selecionar6�   5      5      