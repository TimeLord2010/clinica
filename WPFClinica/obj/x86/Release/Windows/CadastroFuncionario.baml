   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  A%clr-namespace:WPFClinica.UserControlsWPFClinica.UserControls  ,    &WPFClinica.Windows.CadastroFuncionario   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:WPFClinica.Windows     4uc%clr-namespace:WPFClinica.UserControls       � Title$  CadastroFuncionario��$	��600��$	��800��.��� -    �Name$ MyGrid�� � RowDefinitions �� �� $��50 � � ColumnDefinitions �� �� $	��100 �.+�� -   $��MyGrid2��$��2�� �� $��50 ��� $��50 ��� $��75 ��� $��50 ��� $��50 ��� �� �� $��50 � �� �� �� �� �� .+��� -   $��	AddImageR��$��4��$��4��$��10q�
������� � Cursor$
 Hand���� -   $��	AddImageI��$��4��$��4��$��Center=�$��Center=�$��10q�$
 Hand���� -   $��AddImageTBL��$��4��$��1��$��2��$��10q�$��Adicionar Image��$��15(�$��Center=�$��Center=�$
 Hand���� $��2��$��10q�$
��Left=�$	��Top=�$��	Funções��
��������� $��2��$��4��$��Disabled=�$��Visible=�.����  �� Orientation$ 
Horizontal=�$
��Left=�.+��� -    ��Name$ AdministradorChB��$��2��$��Center=�$��	10,10,0,0q�$��15(�$��Administrador���� -   $ RecepcionistaChB��$��2��$��1��$��Center=�$��	10,10,0,0q�$��15(�$��Recepcionista���� -   $ 	MedicoChB��$��2��$��2��$��Center=�$��	10,10,0,0q�$��15(�.��	Médico�� -	   $ TecnicoEnfermeiroChB��$��2��$��3��$��Center=�$��	10,10,0,0q�$��15(����� $
��Wrap=�$��Center=�$��2��$��3��$��15(�$	��100��Técnico em enfermagem�� -
   $ FonoaudiologoChB��$��15(�$��Center=�$��	10,10,0,0q�.��Fonoaudiólogo�� -   $ PsicologoChB��$��15(�$��Center=�$��	10,10,0,0q�.��	Psicologo�� -   $ NutricionistaChB��$��15(�$��Center=�$��	10,10,0,0q�.��Nutricionista�� -   $ DentistaChB��$��15(�$��Center=�$��	10,10,0,0q�.��
Dentista�� $��4��$��4��$��10q�$��Salário��
��������� -    ��Name$ 	SalarioTB��$	��Top=�$��
10,25,10,0q�$��4��$��4��$��15(��� $��10q�$
��Left=�$	��Top=�$��5��$��4��$��Observações��
��������� -   $ ObservacoesTB��$��5��$��2��$��4��$��10,25,10,10q� �� AcceptsReturn . "	 �� VerticalScrollBarVisibility$
	 Auto=�$��15(��� $��5��$��2��$��2��$��Anexos��$��
10,5,10,10q�.���  �� $��50 ��� .+��� -   $��AdicionarAnexo��$��Adicionar anexo��$��15(�$��3q�$��10q��� $��1��$
��Auto=�.���� -   
 ��Name$
 AnexosSP��$ Vertical=��� -   $��EspecificacoesMedicoGB��$��5��$��2��$��2��$��2��$��
10,5,10,10q�$��Especifições do médico��{�.  .���� .���  �� ��  �� $��50 ��� $��50 ��� .+��� $	��CRM��
�������$��10q�$
��Left=�$	��Top=��� -   $ CRMTB��$	��Top=�$��
10,25,10,0q�$��15(��� $��% por consulta��
�������$��10q�$
��Left=�$	��Top=�$��1���� -   $ PorcentagemConsultaTB��$	��Top=�$��1��$��
10,25,10,0q�$��15(��� -   $��AdicionarEspecializacao��$��1��$��2��$ ��Adicionar especialização��$��10q�$��15(��� $��2��$��Especializações:��$��10,0,0,0q�$
��Left=�$	��Top=��� -   $��EspecializacoesSP��$��2��$��2��$��10,20,10,10q�$ Vertical=��� 
�������$��7��$��10,0,0,0q�SenhaG� -    G�Name$ SenhaPB��$��15(�$��7��$��2��$	��Top=�$��
10,20,10,0q��� 
�������$��7��$��2��$��10,0,0,0q�Confirmar SenhaG� -   $ ConfirmarSenhaPB��$��15(�$��7��$��2��$��2��$	��Top=�$��
10,20,10,0q��� -   $��AtivoCB�� d� 	IsChecked$
 TrueY�$��4��$��7��$��Center=�$��Center=�$��15(�.��Ativo�� -   $��
MessageTBL��$��1��$��8q�$��15(�$
��Wrap=��� -   $��
CadastrarB��$��1��$��1��$��Center=�$��Right=�$��15(�$��3q�$��10q�.��	Cadastrar