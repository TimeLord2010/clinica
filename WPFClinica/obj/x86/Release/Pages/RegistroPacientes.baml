   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  (    "WPFClinica.Pages.RegistroPacientes   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  RegistroPacientes��.��� -    �Name$ MyGrid��
������� � ColumnDefinitions �� �� $	��200 �.+�&   System.Windows.Controls.DataGrid  -   $��PacientesDG��$��5q�� $��1��$��5q� � RowDefinitions �� $��65 ��� �� $��60 �.+��  �� �� .+��� -   $��
CadastrarB��$��0��$��15(�$��5q�$	��Top=�.��	Cadastrar�� -   $��EditarB��$��2��$��15(�$��5q�$	��Top=�{�.  .��Editar�� $��15(�$��DemiBold"�$��	0,35,0,10q�	Filtros�� $��1��$
��Auto=�.���� .+��� 
�������CPF�� -   $��CPFTB��$��15(��� $��0,10,0,0q�
�������Nome�� -   $��NomeTB��$��15(��� $��0,10,0,0q�
�������Sexo��  �� Orientation$ 
Horizontal=�.+�	� -   $��TodosRB�� d� 	IsChecked$
 TrueY�.��Todos	� -    	�Name$	 MRB��$��20,0,0,0q�.��M	� -	   $��20,0,0,0q�.��F�� $��0,10,0,0q�
�������Idade�� $ 
Horizontal=�.+��� -
   $��IIdadeTB��$��15(�$��50���� $��15(�$��	10,0,10,0q�-�� -   $��FIdadeTB��$��15(�$��50��