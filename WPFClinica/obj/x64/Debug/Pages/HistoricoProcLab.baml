   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      '    !WPFClinica.Pages.HistoricoProcLab   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  HistoricoProcLab��5   0   .��5	      � 
������� � RowDefinitions 5
   
   �� 5      $��45 �6   6*   �� 5      6   �� 5      $��31 �6   6*   .+�5   
   � $��0,5,0,0q�6    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   �� 5      6%   �� 5      6%   .+�5      �� $��5,0,5,0q�6   .+�5      �� 
�������Procedimento6<   �� 5      -   5        $��ProcedimentoTB��5   (   $��15(�6   6i   5      �� 5      $��1��6   $��5,0,5,0q�6)   .+�5      �� 
�������
Convenio68   �� 5      -   5        $��
ConvenioTB��5   (   $��15(�6   6a   5      �� 5      $��2��6   $��5,0,5,0q�6)   .+�5      �� 
�������Inicio66   �� 5       -   5        $��InicioTB��5    (   $��15(�6   6]   5!      �� 5"      $��3��6   $��5,0,5,0q�6)   .+�5#      �� 
�������Fim63   �� 5$      -   5        $��FimTB��5$   (   $��15(�6   6W   5%      5&      � 5'   
   $��1��6   $��5q�6   .+�5(      &   System.Windows.Controls.DataGrid  -   5        $��ProcedimentosDG��5(         
IsReadOnly . 6B   5)      � 5*   
   $��2��6   $��5,0,0,5q�6   .+�5+      �� $��Bottom=�6    �� Orientation$ 
Horizontal=�64   .+�5,      �� $��15(�6   Total:62   �� 5-      $��15(�6   $��5,0,5,0q�6*   $��DemiBold"�6;   R$6U   �� 5.      -   5        $��TotalTBL��5.   @   $��15(�6   $��DemiBold"�6*   000,006X   5/      �� 50      $��Right=�6   $��0,0,5,0q�65   $ 
Horizontal=�6F   .+�51      �� -   5        $��IncluirNaoPagosChB��51   h   $��Center=�6   $��0,0,10,0q�66   $��15(�6H    d� 	IsChecked$ FalseY�6V   .��6�   Incluir não pagos6�   �� 52      -   5        $��	ImprimirB��52   ;   $��15(�6   $��	10,2,10,2q�6'   .��6d   
Imprimir6n   53      54      55      56      