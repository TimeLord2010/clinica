   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  (    "WPFClinica.Pages.FinalizarPaciente   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  FinalizarPaciente��.��� 
������� � ColumnDefinitions �� �� .+�� $��5q� � RowDefinitions �� $	��125 ��� .+��� .+��� 
�������CPF�� -   $��CPFTB��$��15(��� 
�������$��0,10,0,0q�Nome�� -   $��NomeTB��$��15(��� -   $��MostrarTodosChB��$��0,10,0,0q�$��15(�.��" Mostrar todos na fila de espera.&   System.Windows.Controls.DataGrid  -     Name$ PacientesDG��$��1��   
IsReadOnly . � $��1��$��5q� �� $��1* ��� $	��110 ��� $��35 �.+�� $��0��$��0,5,0,5q�.+��� 
�������Procedimentos laboratoriais  -   $��ProcedimentosDG��$��	0,15,0,30q� . �� $
��Left=�$��Bottom=� �� Orientation$ 
Horizontal=�.+��� -   $��AdicionarProcedimentoB��$��0,0,5,0q�$��5,2,5,2q�$��15(�.��	Adicionar�� -   $��EditarProcedimentoB��$��5,0,5,0q�$��5,2,5,2q�$��15(�{�.  .��Editar�� -   $��RemoverProcedimentoB��$��5,0,5,0q�$��5,2,5,2q�$��15(�{�.  .��	Remover� $��1��$��0,5,0,5q�.+��� .+��� 
�������Método de pagamento	� $��0,5,0,5q� d� 	IsChecked$
 TrueY�.���� $��15(�
Dinheiro	� $��0,5,0,5q�.���� $��15(�Cartão de crédito	� $��0,5,0,5q�.���� $��15(�Cartão de débito� $��2��.+��� $ 
Horizontal=�$��Center=�.+��� $��15(�
�������Total:�� -	   $��ValorTotalTBL��$��15(�$��DemiBold"�$��10,0,0,0q�???.??�� -
   $��SalvarB��$��Right=�$��Center=�$��5,2,5,2q�$��15(�.��Salvar