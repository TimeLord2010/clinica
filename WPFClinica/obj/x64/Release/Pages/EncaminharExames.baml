   M S B A M L   `   `   `  ���� E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  '    !WPFClinica.Pages.EncaminharExames   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:WPFClinica.Pages       N� Title$  EncaminharExames��.��� 
������� � RowDefinitions �� �� $��50 ��� $��40 � � ColumnDefinitions �� �� .+�� $��5q�.+��� .+��� 
�������Tipo�� -   $��TipoCB��$��0,1,0,0q��� 
�������$��0,10,0,0q�Nome�� -   $��PesquisaProcedimentoTB��$��15(�$��0,1,0,0q�&   System.Windows.Controls.DataGrid  -   $��ProcedimentosDG��$��
0,100,0,30q���  �� Orientation$ 
Horizontal=�$��Bottom=�.+��� -   $��
AdicionarB��{�.  $��	15,2,15,2q�.��	Adicionar� $��1��$��5q�.+�  -     Name$ AdicionadosDG��$��0,0,0,30q��� $ 
Horizontal=�$��Right=�$��Bottom=�.+��� -   $��RemoverB��$��	15,2,15,2q�.��	Remover� $��1��$��5q�.+��� $ 
Horizontal=� � 	Resources  �� 
TargetType �� Property �� Value��%3   '��       �� " ��.���  � ��$ 0,0,10,0q�.+��� .+��� 
�������	Débito�� -   $��DebitoTB��$��14(�$��60��.��00,0�� .+��� 
�������
Crédito�� -   $��	CreditoTB��$��14(�$��60��.��00,0�� .+��� 
�������
Dinheiro�� -	   $��
DinheiroTB��$��14(�$��60��.��00,0� $��2��$��5q�.+��� -
   $��PacienteTBL��$��15(�$��SemiBold"�$��Center=�???� $��2��$��1��$��5q�.+��� $��Right=�$��Bottom=�$ 
Horizontal=�.+��� $��0,0,5,0q�$��Center=�$��DemiBold"�R$�� -   $��TotalTBL��$��Center=�$��0,0,10,0q�000,00�� -   $��EncaminharSalvarB��$��	15,2,15,2q�.��Encaminhar / Salvar