   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      (    "WPFClinica.Pages.RegistroPacientes   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  RegistroPacientes��5	      .��5
      � -   5         �Name$ MyGrid��5
      
������� � ColumnDefinitions 5   
   �� 5      6!   �� 5      $	��200 �6   6-   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��PacientesDG��5      $��5q�6   6c   � 5   
   $��1��6   $��5q�6    � RowDefinitions 5      �� 5      $��65 �6    6.   �� 5      6"   �� 5      $��60 �6    6.   .+�5      �  5      �� 5      6)   �� 5      6)   .+�5      �� -   5        $��
CadastrarB��5   Z   $��0��6   $��15(�6)   $��5q�67   $	��Top=�6B   .��6�   	Cadastrar6�   �� 5      -   5        $��EditarB��5   l   $��2��6   $��15(�6)   $��5q�67   $	��Top=�6B   {�.  .��6�   Editar6�   �� 5      $��15(�6   $��DemiBold"�6*   $��	0,35,0,10q�6@   	Filtros6\   5      �� 5      $��1��6   $
��Auto=�6(   .��5       �� .+�5!      �� 
�������CPF67   �� 5"      -   5        $��CPFTB��5"   ,   $��15(�6   6[   �� 5#      $��0,10,0,0q�6    
�������Nome6J   �� 5$      -   5        $��NomeTB��5$   ,   $��15(�6   6\   �� 5%      $��0,10,0,0q�6    
�������Sexo6J   �� 5&       �� Orientation$ 
Horizontal=�6!   .+�5'      	� -   5        $��TodosRB��5'   7    d� 	IsChecked$
 TrueY�6&   .��6|   Todos6�   	� 5(      -   5         	�Name$	 MRB��5(   8   $��20,0,0,0q�6&   .��6{   M6~   	� 5)      -	   5        $��20,0,0,0q�5)   &   .��6n   F6q   5*      �� 5+      $��0,10,0,0q�6    
�������Idade6K   �� 5,      $ 
Horizontal=�6!   .+�5-      �� -
   5        $��IIdadeTB��5-   ;   $��15(�6"   $��50��60   6m   �� 5.      $��15(�6$   $��	10,0,10,0q�62   -6H   �� 5/      -   5        $��FIdadeTB��5/   ;   $��15(�6"   $��50��60   6m   50      52      53      54      55      56      