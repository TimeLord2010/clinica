   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  +    %WPFClinica.Pages.RegistroFuncionarios   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  RegistroFuncionarios��.��� 
������� � ColumnDefinitions �� �� $	��250 �.+�&   System.Windows.Controls.DataGrid  -   $��
RegistroDG��$��5q�   CanUserAddRows .     CanUserReorderColumns .     CanUserDeleteRows .     SelectionMode$ Single=� � ContextMenu �� .@�m� -   $��AtualizarMI��$��	Atualizar���� $��1��$
��Auto=�.����  �� Orientation$ Vertical=�$��5q�.+��  �� �� .+��� .+��� -   $��
CadastrarB��$��	Cadastrar��$��15(�$��5q�$��3q��� $��
Selecionar��$��15(�$��5q�$��3q��� $��1��.+��� -   $��EditarB��$��Editar��$��15(�$��5q�$��3q��� -   $��RemoverB��$��Remover��$��15(�$��5q�$��3q��� $��5q��� $��Filtros��$��15(�$��DemiBold"�$��0,0,0,10q��� $
��Nome��
�������$
��Left=�$	��Top=�$��0,0,0,0q��� -   $��NomeTB��$	��Top=�$��15(��� $
��Left=�$	��Top=�$	��CPF��
�������$��0,10,0,0q��� -   $��CPFTB��$	��Top=�$��15(��� -   $��InativosChB��$��15(�$��0,10,0,0q�.��
Inativos�� $��Função��
�������$
��Left=�$	��Top=�$��0,10,0,0q��� $��0,5,0,0q�$��15(�.��Administrador�� $��0,5,0,0q�$��15(�.��	Médico�� $��0,5,0,0q�$��15(�.��Recepcionista�� $��0,5,0,0q�$��15(�.��Técnico em enfermagem�� $
��Sexo��$��0,10,0,0q�
��������� .+�	� $��Ambos��$��15(�$��0,5,0,5q� d� 	IsChecked$
 TrueY�	� $��	Masculino��$��15(�$��0,0,0,5q�	� $��Feminino��$��15(��� $��Idade��$��0,10,0,0q�
��������  �� �� $��20 ��� .+��� $	��Min��
��������� $��0,15,0,0q�$��15(��� $��~��$��15(�$��5,0,5,0q�$��1��$��Bottom=�$��Center=��� $	��Max��
�������$��2���� $��2��$��0,15,0,0q�$��15(�