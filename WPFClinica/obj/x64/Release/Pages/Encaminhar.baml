   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  !    WPFClinica.Pages.Encaminhar   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  
Encaminhar��.��� 
�������.+��� $��Paciente��$��	5,5,5,120q�.��� $��5q�.+��� 
�������Nome�� -    ��Name$ NomeTB��$	��Top=�$��0,20,0,0q��� 
�������$��0,50,0,0q�CPF�� -   $ CPFTB��$	��Top=�$��0,70,0,0q�&   System.Windows.Controls.DataGrid  -     Name$ 
PesquisaDG��$��
0,110,0,40q��� $
��Left=�$��Bottom=� �� Orientation$ 
Horizontal=�.+��� 
�������Selecionado:�� -   $��SelecionadoTBL��$��Nenhum��$��10,0,0,0q��� $��Right=�$��Bottom=�$ 
Horizontal=�.+��� -   $��DeAgendamentosB��$��5,0,5,0q�.��De agendamentos�� -   $��5,0,0,0q�.��	Cadastrar� $��Bottom=�$��15q� � RowDefinitions �� �� ��  � ColumnDefinitions �� �� $��2* ��� �� .+��� $��Center=�Funcionário:�� -   $��FuncionarioTBL��$��1��$��Center=�$��0,5,0,5q�???�� -   $��	EscolherB��$��3��$��Center=�.��
Escolher�� $��1��$��Center=�Especialização:�� -	   $��EspecializacaoCB��$��1��$��1��$��0,5,0,5q��� -
   $��AsoChB��$��1��$��2��$��Center=�$��Center=�$	��100��.��ASO�� $��2��$��Center=�Encaminhar para�� -   $��EncaminharParaCB��$��2��$��1�� �� SelectedIndex$ 0��$��Center=�$��0,5,0,0q�.@��� $��Triagem���� $��Laboratório���� .��Exames Diversos�� .��ASO�� -    ��Name$ PrioridadeChB��$��2��$��2��$��Center=�$��Center=�$	��100��.��
Prioridade�� -   $��EncaminharB��$��2��$��4��$��Center=�.��
Encaminhar