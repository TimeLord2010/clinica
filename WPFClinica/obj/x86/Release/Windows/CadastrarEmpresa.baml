   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  )    #WPFClinica.Windows.CadastrarEmpresa   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:WPFClinica.Windows       � Title$  CadastrarEmpresa��$	��260��$	��400�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$ 
ToolWindow=� � 	Resources  �� 
TargetType �� BasedOn �� Property �� Value��%�   '��       7��'�� ?     7���� " ��8   .���  � ��$	 Top=���  � ��$ 10q��� " ��8   .���  � ��$ 10q�.���  � RowDefinitions �� �� �� $��2* ��� 	 � ColumnDefinitions	 �� �� $��2* �.+��� $��0��Nome*�� -   $��NomeEmpresaTB��$��1���� $��1��CNPJ�� -   
 ��Name$
 CNPJ_TB��$��1��$��1���� $��2��Observação�� -   $
 ObservacaoTB��$��2��$��1�� �� TextWrapping$
 Wrap=�" �� VerticalScrollBarVisibility$ Visible=� �� AcceptsReturn .  �� 
AcceptsTab . �� $��3���� -   $��
MessageTBL��$��3��$��2��$��Center=�$��10,10,110,10q�$
��Wrap=��� -   $��
CadastrarB��$��3��$��1��$��Right=�$��Center=�$��10q�.��	Cadastrar