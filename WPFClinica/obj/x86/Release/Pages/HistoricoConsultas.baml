   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  )    #WPFClinica.Pages.HistoricoConsultas   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  HistoricoConsultas��.��� 
������� � RowDefinitions �� $��50 ��� $��50 ��� �� $��40 �.+�� $��5q� � ColumnDefinitions �� �� �� .+�� $��0,0,5,0q�.+��� 
�������Funcionário�� -   $��FuncionarioTB��$��0,18,0,0q�$	��Top=�$��15(�� $��1��$��5,0,5,0q�.+��� 
�������
Paciente�� -   $��
PacienteTB��$��15(�$	��Top=�$��0,18,0,0q�� $��2��$��5,0,0,0q� �� �� $��20 ��� .+�� .+��� 
�������	Início�� -   $��InicioTB��$��0,18,0,0q��� $��1��$��Center=�$��Bottom=�$��0,0,0,5q�$��15(�-� $��2��.+��� 
�������Fim�� -   $��FimTB��$��0,18,0,0q�� $��1��$��0q� �� �� �� .+��� -    ��Name$ 
RetornoChB��$��Center=�$��15(�$��5q�.��
É Retorno�� -   $ PagoChB��$��1��$��Center=� d� 	IsChecked$
 TrueY�$��15(�$��5q�.��
Foi Paga�� -   $ RecebidaChB��$��2��$��Center=�$��5q�$��15(�.��Foi recebido por funcionário� $��2��$��5q�.+�&   System.Windows.Controls.DataGrid  -     Name$ HistoricoDG��� $��3��$��5q�.+��� $
��Left=�$��Bottom=� �� Orientation$ 
Horizontal=�.+��� -	   $��EditarB��$��15(�$��5,3,5,3q�$��5,0,5,0q�{�.  .��Editar�� $��Right=�$��Bottom=�.+��� -
   $��	ImprimirB��$��15(�$��5,3,5,3q�.��
Imprimir