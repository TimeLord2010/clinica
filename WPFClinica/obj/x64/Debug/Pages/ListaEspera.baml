   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WPFClinica.PagesWPFClinica.Pages  5      "    WPFClinica.Pages.ListaEspera   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:WPFClinica.Pages     5        N� Title$  ListaEspera��5	      .��5
      � 
������� � ColumnDefinitions 5   
   �� 5      6!   �� 5      $	��200 �6   6-   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��
PesquisaDG��5      $��5q�6      
IsReadOnly . 6s   � 5   
   $��1��6   .+�5      �� $��15(�6   $��DemiBold"�6&   $
��Left=�6<   $	��Top=�6W   	Filtros6x   �� 5      $��	0,20,0,40q�6   .��5      �� .+�5      �� 
�������$��5,5,0,0q�62   CPF6H   �� 5      -   5        $��CPFTB��5   7   $��5q�6   $��15(�6)   6f   �� 5      
�������$��5,5,0,0q�62   Nome6I   �� 5      -   5        $��NomeTB��5   7   $��5q�6   $��15(�6)   6h   �� 5      $��5q�6    
�������
Prioridade6I   �� 5      .+�5      	� -   5        $��TodosPrioridadeRB��5   ?   $��5q�6&   $��15(�61    d� 	IsChecked$
 TrueY�6X   .��6i   Todos6p   	� 5      -   5        $��SimPriridadeRB��5   ?   $��5q�6&   $��15(�61   .��6U   Sim6Z   	� 5      -   5        $��NaoPrioridadeRB��5   ?   $��5q�6&   $��15(�61   .��6V   Não6[   5      �� 5      
�������$��5,5,0,0q�62   Encaminhado6P   �� 5      -   5        $��EncaminhadoCB��5      $��5,0,5,0q�64   $��15(�6E    �� SelectedIndex$ 0��6S   .@�5       �� .��6'   Todos6.   �� 5!      .��6'   	Triagem60   �� 5"      .��6'   Consultório64   �� 5#      .��6'   Laboratório64   �� 5$      .��6'   Exames Médicos67   5%      �� 5&      $��5,5,0,0q�6    
�������Status6K   �� 5'      .+�5(      	� -   5        $��TodosStatusRB��5(   P   $��5q�6&   $��15(�61   $
 TrueY�6?   .��6e   Todos6l   	� 5)      -	   5        $��EmAtendimentoRB��5)   ?   $��5q�6&   $��15(�61   .��6V   Em atendimento6f   	� 5*      -
   5        $��EsperandoRB��5*   ?   $��5q�6&   $��15(�61   .��6R   	Esperando6]   	� 5+      -   5        $��
AtendidoRB��5+   ?   $��5q�6&   $��15(�61   .��6Q   
Atendido6[   	� 5,      -   5        $��
AcaminhoRB��5,   ?   $��5q�6&   $��15(�61   .��6Q   	A caminho6\   5-      5.      5/      �� 50      $��Bottom=�6    �� Orientation$ 
Horizontal=�64   $��5q�6M   .+�51      �� -   5        $��EditarB��51   +   {�.  $��15(�6:   $��3q�6H   .��6j   Editar6r   �� 52      -   5        $��DeletarB��52   D   $��5,0,0,0q�6   $��15(�6*   $��3q�68   {�.  .��6}   	Deletar6�   �� 53      -   5        $��
PesquisarB��53   D   $��5,0,0,0q�6   $��3q�6*   $��15(�66   .��6o   	Pesquisar6z   54      55      56      57      