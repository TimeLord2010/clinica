   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  '    !WPFClinica.Pages.HistoricoProcLab   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  HistoricoProcLab��.��� 
������� � RowDefinitions �� $��45 ��� �� $��31 �.+�� $��0,5,0,0q� � ColumnDefinitions �� �� �� �� .+��� $��5,0,5,0q�.+��� 
�������Procedimento�� -   $��ProcedimentoTB��$��15(��� $��1��$��5,0,5,0q�.+��� 
�������
Convenio�� -   $��
ConvenioTB��$��15(��� $��2��$��5,0,5,0q�.+��� 
�������Inicio�� -   $��InicioTB��$��15(��� $��3��$��5,0,5,0q�.+��� 
�������Fim�� -   $��FimTB��$��15(�� $��1��$��5q�.+�&   System.Windows.Controls.DataGrid  -   $��ProcedimentosDG��   
IsReadOnly . � $��2��$��5,0,0,5q�.+��� $��Bottom=� �� Orientation$ 
Horizontal=�.+��� $��15(�Total:�� $��15(�$��5,0,5,0q�$��DemiBold"�R$�� -   $��TotalTBL��$��15(�$��DemiBold"�000,00�� $��Right=�$��0,0,5,0q�$ 
Horizontal=�.+��� -   $��IncluirNaoPagosChB��$��Center=�$��0,0,10,0q�$��15(� d� 	IsChecked$ FalseY�.��Incluir não pagos�� -   $��	ImprimirB��$��15(�$��	10,2,10,2q�.��
Imprimir