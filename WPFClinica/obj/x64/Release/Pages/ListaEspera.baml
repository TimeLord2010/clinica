   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  "    WPFClinica.Pages.ListaEspera   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  ListaEspera��.��� 
������� � ColumnDefinitions �� �� $	��200 �.+�&   System.Windows.Controls.DataGrid  -   $��
PesquisaDG��$��5q�   
IsReadOnly . � $��1��.+��� $��15(�$��DemiBold"�$
��Left=�$	��Top=�	Filtros�� $��	0,20,0,40q�.���� .+��� 
�������$��5,5,0,0q�CPF�� -   $��CPFTB��$��5q�$��15(��� 
�������$��5,5,0,0q�Nome�� -   $��NomeTB��$��5q�$��15(��� $��5q�
�������
Prioridade�� .+�	� -   $��TodosPrioridadeRB��$��5q�$��15(� d� 	IsChecked$
 TrueY�.��Todos	� -   $��SimPriridadeRB��$��5q�$��15(�.��Sim	� -   $��NaoPrioridadeRB��$��5q�$��15(�.��Não�� 
�������$��5,5,0,0q�Encaminhado�� -   $��EncaminhadoCB��$��5,0,5,0q�$��15(� �� SelectedIndex$ 0��.@��� .��Todos�� .��	Triagem�� .��Consultório�� .��Laboratório�� .��Exames Médicos�� $��5,5,0,0q�
�������Status�� .+�	� -   $��TodosStatusRB��$��5q�$��15(�$
 TrueY�.��Todos	� -	   $��EmAtendimentoRB��$��5q�$��15(�.��Em atendimento	� -
   $��EsperandoRB��$��5q�$��15(�.��	Esperando	� -   $��
AtendidoRB��$��5q�$��15(�.��
Atendido	� -   $��
AcaminhoRB��$��5q�$��15(�.��	A caminho�� $��Bottom=� �� Orientation$ 
Horizontal=�$��5q�.+��� -   $��EditarB��{�.  $��15(�$��3q�.��Editar�� -   $��DeletarB��$��5,0,0,0q�$��15(�$��3q�{�.  .��	Deletar�� -   $��
PesquisarB��$��5,0,0,0q�$��3q�$��15(�.��	Pesquisar