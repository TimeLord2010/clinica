   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  5   	   &     WPFClinica.Windows.EncaminharLab   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WPFClinica.Windows     5   	     � Title$"  Encaminhar para laboratório��5   	   $	��400��6-   $	��800��6:    � WindowStartupLocation$ CenterScreen=�6F   .��5	      �  � RowDefinitions 5
   
   �� 5      $��55 �6   6*   �� 5      $��1* �6   6*   �� 5      $��55 �6   6*    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   � $��2��6   $��5q�6#   .+�5      �� 
�������	Paciente:65   �� 5      -   5        $��PacienteTBL��5   8   $��0,20,0,0q�6   $��15(�6*   ???6P   5      � 5   
   $��1��6   $��5q�6   .+�5      �� Procedimentos6'   �� 5      -   5        $��ProcedimentosTB��5   N   $��0,20,0,0q�6   $	��Top=�6(   $��15(�6@   6�   &   System.Windows.Controls.DataGrid  5      -   5        $��ProcedimentosDG��5   *   $��	0,50,0,35q�6   6w   �� 5      $��Bottom=�6    �� Orientation$ 
Horizontal=�64   .+�5      �� -   5        $��
AdicionarB��5   K   $��15(�6   $��5,3,5,3q�6'   {�.  .��6v   	Adicionar6�   5      5      � 5   
   $��1��6   $��1��6   $��5q�6,   .+�5       �� Adicionados6%     5!      -   5        $��AdicionadosDG��5!   *   $��	0,20,0,35q�6   6s   �� 5"      $��Bottom=�6   $��Right=�64   .+�5#      �� -   5         ��Name$ RemoverB��5#   K   $��15(�6   $��5,3,5,3q�6'   {�.  .��6t   	Remover6}   5$      5%      � 5&   
   $��2��6   $��5q�6   .+�5'      �� $��Center=�6   $ 
Horizontal=�64   .+�5(      �� .+�5)      �� 
������� �� Padding$ 0,0,10,0q�62   
Dinheiro6O   �� 5*      -   5         ��Name$ 
DinheiroTB��5*      $��14(�62   .��6@   00,006G   5+      �� 5,      $��	10,0,10,0q�6   .+�5-      �� 
�������$ 0,0,10,0q�62   
Crédito6N   �� 5.      -   5        $ 	CreditoTB��5.      $��14(�61   .��6?   00,006F   5/      �� 50      .+�51      �� 
�������$ 0,0,10,0q�62   	Débito6M   �� 52      -	   5        $ DebitoTB��52      $��14(�60   .��6>   00,006E   53      54      55      � 56   
   $��2��6   $��1��6   $��5q�6,   .+�57      �� $��Center=�6   $��Right=�64   $ 
Horizontal=�6P   .+�58      �� $��Center=�6   $��14(�67   $��DemiBold"�6E   Total:6c   �� 59      -
   5        $��TotalTBL��59   W   $��Center=�6   $��5,0,20,0q�67   $��14(�6I   000,006o   �� 5:      -   5        $��EncaminharB��5:   9   $��15(�6   $��5,3,5,3q�6'   .��6f   
Encaminhar6r   5;      5<      5=      5>      