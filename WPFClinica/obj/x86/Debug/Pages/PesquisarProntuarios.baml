   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      +    %WPFClinica.Pages.PesquisarProntuarios   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  PesquisarProntuarios��5	      .��5
      � 
������� � RowDefinitions 5   
   �� 5      $��55 �6   6*   �� 5      6   �� 5      $��40 �6   6*    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   �� $��5q�6   .+�5      �� 
�������Nome paciente69   �� 5      -   5        $��NomePacienteTB��5      6W   5      �� 5   
   $��1��6   $��5q�6%   .+�5      �� 
�������%#Especialização da ultima consulta6M   �� 5      -   5        $��EspecializacaoCB��5      6f   5      � 5   
   $��1��6   $��5q�6   $��2��6'   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ PacientesDG��5         
IsReadOnly . 6p   5      � 5   
   $��2��6   .+�5       ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   .+�5!      �� -   5        $��AbrirProntuarioB��5!   X   $��15(�6   $��	10,2,10,2q�6'   $��5q�6;   {�.  .��6�   Abrir6�   �� 5"      -   5        $��AtualizarProntuarioB��5"   X   $��15(�6   $��	10,2,10,2q�6'   $��5q�6;   {�.  .��6�   	Atualizar6�   5#      5$      5%      5&      