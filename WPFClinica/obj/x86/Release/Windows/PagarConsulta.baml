   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  &     WPFClinica.Windows.PagarConsulta   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:WPFClinica.Windows     -     � Title$  PagarConsulta��$	��360��$	��300�� � WindowStartupLocation$ CenterScreen=�.��� .+��� $��0,0,0,40q�.���� $��5q�.+��� 
�������Nome do paciente�� -   $��NomePacienteTB��$��15(� �� 
IsReadOnly . .��[Nulo]�� 
�������$��0,10,0,0q�Funcionário�� -   $��NomeFuncionarioTB��$��15(� . .��[Nulo]�� 
�������$��0,10,0,0q�Especialização�� -   $��EspecializacaoTB�� . $��15(�.��[Nulo]�� 
�������$��0,10,0,0q�Total�� -   $��TotalTB��$��15(�{�.  .��[Nulo]�� 
�������$��0,10,0,0q�
Dinheiro�� -   $��
DinheiroTB��$��15(��� 
�������$��0,10,0,0q�	Débito�� -   $��	DébitoTB��$��15(��� 
�������$��0,10,0,0q�
Crédito�� -   $��
CréditoTB��$��15(�� $��Bottom=� � ColumnDefinitions �� �� .+��� -	   $��SalvarB��$��15(�$��5q�$��	10,2,10,2q�.��Salvar�� -
   $��	CancelarB��$��1��$��15(�$��5q�$��	10,2,10,2q�.��
Cancelar