   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  5   	   &     WPFClinica.Windows.PagarConsulta   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WPFClinica.Windows     5   	   -   5          � Title$  PagarConsulta��5   	   $	��360��6   $	��300��6,    � WindowStartupLocation$ CenterScreen=�6Q   .��5	      � .+�5
   
   �� $��0,0,0,40q�6   .��5      �� $��5q�6   .+�5      �� 
�������Nome do paciente6@   �� 5      -   5        $��NomePacienteTB��5   (   $��15(�6    �� 
IsReadOnly . .��6P   [Nulo]6X   �� 5      
�������$��0,10,0,0q�6.   Funcionário6M   �� 5      -   5        $��NomeFuncionarioTB��5   :   $��15(�6    . .��6S   [Nulo]6[   �� 5      
�������$��0,10,0,0q�6.   Especialização6P   �� 5      -   5        $��EspecializacaoTB��5   :    . $��15(�6,   .��6R   [Nulo]6Z   �� 5      
�������$��0,10,0,0q�6.   Total6G   �� 5      -   5        $��TotalTB��5   (   $��15(�6   {�.  .��6I   [Nulo]6Q   �� 5      
�������$��0,10,0,0q�6.   
Dinheiro6J   �� 5      -   5        $��
DinheiroTB��5   (   $��15(�6   6<   �� 5      
�������$��0,10,0,0q�6.   	Débito6H   �� 5      -   5        $��	DébitoTB��5   (   $��15(�6   6:   �� 5      
�������$��0,10,0,0q�6.   
Crédito6I   �� 5      -   5        $��
CréditoTB��5   (   $��15(�6   6;   5      5      � 5   
   $��Bottom=�6    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   .+�5!      �� -	   5        $��SalvarB��5!   B   $��15(�6   $��5q�6#   $��	10,2,10,2q�6.   .��6g   Salvar6o   �� 5"      -
   5        $��	CancelarB��5"   R   $��1��6   $��15(�6%   $��5q�63   $��	10,2,10,2q�6>   .��6{   
Cancelar6�   5#      5$      5%      