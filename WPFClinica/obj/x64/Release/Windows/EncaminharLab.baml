   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  &     WPFClinica.Windows.EncaminharLab   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:WPFClinica.Windows       � Title$"  Encaminhar para laboratório��$	��400��$	��800�� � WindowStartupLocation$ CenterScreen=�.���  � RowDefinitions �� $��55 ��� $��1* ��� $��55 � � ColumnDefinitions �� �� .+�� $��2��$��5q�.+��� 
�������	Paciente:�� -   $��PacienteTBL��$��0,20,0,0q�$��15(�???� $��1��$��5q�.+��� Procedimentos�� -   $��ProcedimentosTB��$��0,20,0,0q�$	��Top=�$��15(�&   System.Windows.Controls.DataGrid  -   $��ProcedimentosDG��$��	0,50,0,35q��� $��Bottom=� �� Orientation$ 
Horizontal=�.+��� -   $��
AdicionarB��$��15(�$��5,3,5,3q�{�.  .��	Adicionar� $��1��$��1��$��5q�.+��� Adicionados  -   $��AdicionadosDG��$��	0,20,0,35q��� $��Bottom=�$��Right=�.+��� -    ��Name$ RemoverB��$��15(�$��5,3,5,3q�{�.  .��	Remover� $��2��$��5q�.+��� $��Center=�$ 
Horizontal=�.+��� .+��� 
������� �� Padding$ 0,0,10,0q�
Dinheiro�� -    ��Name$ 
DinheiroTB��$��14(�.��00,00�� $��	10,0,10,0q�.+��� 
�������$ 0,0,10,0q�
Crédito�� -   $ 	CreditoTB��$��14(�.��00,00�� .+��� 
�������$ 0,0,10,0q�	Débito�� -	   $ DebitoTB��$��14(�.��00,00� $��2��$��1��$��5q�.+��� $��Center=�$��Right=�$ 
Horizontal=�.+��� $��Center=�$��14(�$��DemiBold"�Total:�� -
   $��TotalTBL��$��Center=�$��5,0,20,0q�$��14(�000,00�� -   $��EncaminharB��$��15(�$��5,3,5,3q�.��
Encaminhar