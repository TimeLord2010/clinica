   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      #    WPFClinica.Pages.PacientesLab   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  PacientesLab��5   0   .��5	      � 
������� � ColumnDefinitions 5
   
   �� 5      6!   �� 5      6!   .+�5   
   � $��5,5,7,5q�6    � RowDefinitions 5      �� 5      $	��135 �6    6/   �� 5      6"   .+�5      �� .+�5      �� 
�������Nome64   �� 5      -   5        $��NomeTB��5   (   $��15(�6   6Y   �� 5      $��0,5,0,0q�6   
�������Procedimento6M   �� 5      -   5         ��Name$ PesquisaProcedimentoCB��5   -    �� SelectedIndex$ 0��6   .@�5      �� .��6#   Todos6*   5      �� 5      
�������$��0,5,0,0q�6.   Status6G   �� 5      -   5        $ PesquisaStatusCB��5   -   $ 0��6   .@�5      �� .��6#   Todos6*   �� 5      .��6#   
Pendente6-   �� 5      .��6#   Em andamento61   �� 5      .��6#   Pronto6+   5       5!      � 5"      $��1��6   $��0q�6    .+�5#      &   System.Windows.Controls.DataGrid  -   5          Name$ 
PesquisaDG��5#         
IsReadOnly . 6r   5$      5%      �� 5&   
   $��2��6   $��2��6%   $��Center=�69   
�������$��2��6i    J� SnapsToDevicePixels . $��0,5,0,5q�6�   6�   � 5'   
   $��1��6   $��7,5,5,5q�6    5(      �� 5)      6"   �� 5*      6"   .+�5,        -   5        $ PesquisaProcedimentosDG��5,       . 6�   � 5-      $��1��6   $��0,5,0,0q�6    .+�5.      �� $	��Top=�6   .+�5/      �� 
�������$��0,5,0,0q�62   Status6K   �� 50      -   5        $��SelectedStatusCB��50      .@�51      �� .��6'   
Pendente61   �� 52      .��6'   Em andamento65   �� 53      .��6'   Pronto6/   54      �� 55      
�������$��0,5,0,0q�62   Observação6O   56      �� 57      -   5        $��ObservacaoTB��57   -   $��	0,65,0,35q�6   6C   �� 58      $��Bottom=�6   $��Right=�68   .+�59      �� -   5        $��EditarB��59   =   $��15(�6   $��5,3,5,3q�6+   {�.  .��6t   Editar6|   5:      5;      5<      5=      5>      