   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      &     WPFClinica.Pages.EscolherEmpresa   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  EscolherEmpresa��5	       � 	Resources 5
       �� 
TargetType �� BasedOn �� Property �� Value��%�   '��       7��'�� z     7���� 5   
   " ��6   8   .�5      ��  � ��$ 12(�6)   66   �� 5       � ��$ 5q�6'   63   5      �� 5   
   " ��6   8   .�5      ��  � ��$ 5,22,5,5q�6'   6:   5      5      .��5      � 
������� � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      �� 5      $��50 �6    � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      .+�5   
   � .+�5       �� Nome6   �� 5!      -   5         ��Name$ NomeEmpresaTB��5!      6/   5"      � 5#   
   $��1��6   .+�5$      �� CNPJ6   �� 5%      -   5        $ CNPJ_TB��5%      6)   5&      � 5'   
   $��3��6   $��Center=�6   $��Center=�6<   .+�5(      �� -   5        	 ��Name$	 PesquisarTB��5(      .��6D   	Pesquisar6O   5)      � 5*   
   $��1��6   $��4��6   $��5q�60   .+�5+      &   System.Windows.Controls.DataGrid  -   5        
  Name$
 
EmpresasDG��5+      6\   5,      � 5-   
   $��2��6   $��Center=�6   $��Center=�69   .+�5.      �� -   5        $	 	EscolherB��5.      .��6@   
Escolher6J   5/      � 50   
   $��2��6   $��1��6   $
��Left=�6,   $��Center=�6G   .+�51      �� -   5        $	 EditarB��51      .��6<   Editar6D   52      � 53   
   $��2��6   $��3��6   $��Center=�6,   $��Center=�6G   .+�54      �� -   5        $	 	CadastroB��54      .��6@   	Casdastro6K   55      56      57      