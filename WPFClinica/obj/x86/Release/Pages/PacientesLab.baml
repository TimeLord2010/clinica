   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  #    WPFClinica.Pages.PacientesLab   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  PacientesLab��.��� 
������� � ColumnDefinitions �� �� .+�� $��5,5,7,5q� � RowDefinitions �� $	��135 ��� .+��� .+��� 
�������Nome�� -   $��NomeTB��$��15(��� $��0,5,0,0q�
�������Procedimento�� -    ��Name$ PesquisaProcedimentoCB�� �� SelectedIndex$ 0��{�.  .@��� .��Todos�� 
�������$��0,5,0,0q�Status�� -   $ PesquisaStatusCB��$ 0��{�.  .@��� .��Todos�� .��
Pendente�� .��Em andamento�� .��Pronto� $��1��$��0q�.+�&   System.Windows.Controls.DataGrid  -     Name$ 
PesquisaDG��   
IsReadOnly . �� $��2��$��2��$��Center=�
�������$��2�� J� SnapsToDevicePixels . $��0,5,0,5q�� $��1��$��7,5,5,5q� �� �� .+�  -   $ PesquisaProcedimentosDG�� . � $��1��$��0,5,0,0q�.+��� $	��Top=�.+��� 
�������$��0,5,0,0q�Status�� -   $��SelectedStatusCB��.@��� .��
Pendente�� .��Em andamento�� .��Pronto�� 
�������$��0,5,0,0q�Observação�� -   $��ObservacaoTB��$��	0,65,0,35q��� $��Bottom=�$��Right=�.+��� -   $��EditarB��$��15(�$��5,3,5,3q�{�.  .��Editar