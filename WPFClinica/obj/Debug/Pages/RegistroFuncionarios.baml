   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      +    %WPFClinica.Pages.RegistroFuncionarios   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  RegistroFuncionarios��5	      .��5
      � 
������� � ColumnDefinitions 5   
   �� 5      6!   �� 5      $	��250 �6   6-   .+�5   
   &   System.Windows.Controls.DataGrid  $��5q�6   �� 5   
   $��1��6   $
��Auto=�6'   .��5      ��  �� Orientation$ Vertical=�6   $��5q�60   .+�5      �� $
��Nome��6   
�������$
��Left=�6:   $	��Top=�6U   $��0,0,0,0q�6m   6�   �� 5      $	��Top=�6   $��15(�62   6B   �� 5      $
��Left=�6   $	��Top=�67   $	��CPF��6O   
�������$��0,10,0,0q�6l   6�   �� 5      $	��Top=�6   $��15(�62   6B   �� 5      $��Função��6   
�������$
��Left=�6<   $	��Top=�6W   $��0,10,0,0q�6o   6�   �� 5      $	��Top=�6    �� SelectedIndex$ 0��63   .@�5      �� $��Todos��6#   65   �� 5      $��Médicos��6#   67   �� 5      $��Recepcionistas��6#   6>   �� 5      $��Técnicos em enfermagem��6#   6F   5      �� 5      $
��Sexo��6   $��0,10,0,0q�6(   6<   �� 5      .+�5      	� $��Ambos��6"   $��0,5,0,5q�62    d� 	IsChecked$
 TrueY�6C   6V   	� 5       $��	Masculino��6"   $��0,0,0,5q�66   6I   	� 5!      $��Feminino��6"   67   5"      �� 5#      $��Idade��6   $��0,10,0,0q�6)   6=   � 5$       5%      �� 5&      6-   �� 5'      $��20 �6+   68   �� 5(      6-   .+�5*      �� $	��Min��6    
�������6?   �� 5+      $��0,15,0,0q�6   62   �� 5,      $��~��6    $��5,0,5,0q�6)   $��1��6:   $��Bottom=�6J   $��Center=�6e   6�   �� 5-      $	��Max��6    
�������$��2��6=   6O   �� 5.      $��2��6   $��0,15,0,0q�6.   6B   5/      50      51      52      53      