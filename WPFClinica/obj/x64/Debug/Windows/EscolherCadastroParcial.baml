   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  5   	   0    *WPFClinica.Windows.EscolherCadastroParcial   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WPFClinica.Windows     5   	     � Title$  EscolherCadastroParcial��5   	   $	��400��6)   $	��400��66   .��5	      �  � RowDefinitions 5
   
   �� 5      $��55 �6   6*   �� 5      $��55 �6   6*   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      $	��100 �6   6-   .+�5   
   � .+�5      �� $��5,5,0,0q�6   
�������Nome6A   �� 5      -   5        $��NomeTB��5   6   $��15(�6   $��5,25,5,5q�6$   6g   5      � 5   
   $��1��6   .+�5      �� 
�������$��5,5,0,0q�6*   	Contato6D   �� 5      -   5        $��	ContatoTB��5   6   $��5,25,5,5q�6   $��15(�6(   6m   5      � 5   
   $��1��6   $��0,20,0,0q�6   .+�5      �� -   5        $��CriarB��5       $��5q�6   .��6C   Criar6J   5      � 5   
   $��1��6   $��1��6   $��5,25,5,5q�6,   .+�5      �� -   5         ��Name$ SelecionarB��5      .��6D   
Selecionar6P   5       � 5!   
   $��2��6   $��2��6   $��5q�60   .+�5"      &   System.Windows.Controls.DataGrid  -   5          Name$ PacientesTempDG��5"      62   5#      5$      5%      