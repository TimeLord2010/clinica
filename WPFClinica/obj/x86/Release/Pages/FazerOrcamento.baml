   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  %    WPFClinica.Pages.FazerOrcamento   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  FazerOrcamento��.��� 
������� � ColumnDefinitions �� �� .+��� $
��Auto=�.��� .+��� $��5q�.+��� $��Estabelecimento��.���� $��5q�.+��� 
�������Nome�� -   $��NomeEstabelecimentoTB��$��15(�.��Clínica Saúde Fácil�� 
�������$��0,10,0,0q�	Endereço�� -   $��
EnderecoTB��$��15(�.��R. Inspetor Marcelino. N° 60�� $��0,10,0,0q�
�������	Contato�� -   $��	ContatoTB��$��15(�.��$"(96) 44141-4242 / (96) 9 8141-6020�� $��Paciente��.���  �� �� .+��� $��5q�.+��� 
�������CPF�� -   $��CPFTB��$��15(��� 
�������$��0,10,0,0q�
Nascimento�� -   $��NascimentoTB��$��15(��� $��1��$��5q�.+��� 
�������Nome�� -   $��NomePacienteTB��$��15(��� 
�������$��0,10,0,0q�Sexo�� -   $��SexoTB��$��15(��� $��Procedimentos��.���� $��5q�.+��� 
�������	Convênio� .+��� -   $��
ConvenioTB��$��15(�$��0,0,90,0q��� -	   $��SelecionarB��$��Right=�$��	10,2,10,2q�.��
Selecionar�� 
�������$��0,10,0,0q�Procedimento�� -
   $��ProcedimentoTB��$��15(�� $	��200��$��0,10,0,0q� � RowDefinitions �� $��20 ��� �� $��30 � �� �� .+��� 
�������$��Center=�
Pesquisa&   System.Windows.Controls.DataGrid  -   $��
PesquisaDG��$��1��$��0,0,5,0q��� $��2��$��Center=� �� Orientation$ 
Horizontal=�.+��� -   $��
AdicionarB��$��	10,2,10,2q�{�.  .��	Adicionar�� $��1��
�������$��Center=�$��5,0,0,0q�Adicionados  -   $��AdicionadosDG��$��1��$��1��$��5,0,0,0q��� $��2��$��1��$��Right=�$��Center=�.+��� -   $��RemoverB��$��	10,2,10,2q�{�.  .��	Remover�� $��Valores��.��� $��5q� �� �� .+��� $��0,0,5,0q�.+��� 
�������
Desconto�� -   $��DescontosTB��$��15(��� $��1��$��5,0,0,0q�.+��� 
�������	Entrada�� -   $��	EntradaTB��$��15(��� $��5,10,0,0q�
�������Observação�� -   $��ObservaçãoTB��$��15(�$��5,0,5,0q�$��50��� $��1��$��5q� �� �� $��35 �.+��� $��14(�
�������Visualização�� -   $��
PreviewRTB��$��0,20,0,0q�" �� VerticalScrollBarVisibility$
 Auto=�$ �� HorizontalScrollBarVisibility$
 Auto=�. �/� �� $��1��$ 
Horizontal=�$��Right=�$��Center=�.+��� -   $��AbrirDocumentoB��$��15(�$��	10,2,10,2q�.��Abrir documento