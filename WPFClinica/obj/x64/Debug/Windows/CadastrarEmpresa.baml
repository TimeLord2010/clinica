   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  5   	   )    #WPFClinica.Windows.CadastrarEmpresa   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WPFClinica.Windows     5   	     � Title$  CadastrarEmpresa��5   	   $	��260��6"   $	��400��6/    � WindowStartupLocation$ CenterScreen=�6;    � WindowStyle$ 
ToolWindow=�6`    � 	Resources 5	       �� 
TargetType �� BasedOn �� Property �� Value��%�   '��       7��'�� |     7���� 5
   
   " ��6   8   .�5      ��  � ��$	 Top=�62   6@   �� 5       � ��$ 10q�6'   64   5      �� 5   
   " ��6   8   .�5      ��  � ��$ 10q�6'   64   5      5      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      �� 5      $��2* �6   �� 5      	 � ColumnDefinitions	 5   
   �� 5      �� 5      $��2* �6   .+�5   
   �� $��0��6   Nome*6(   �� 5   
   -   5        $��NomeEmpresaTB��5   "   $��1��6   69   �� 5   
   $��1��6   CNPJ6'   �� 5    
   -   5        
 ��Name$
 CNPJ_TB��5    /   $��1��6   $��1��6   6B   �� 5!   
   $��2��6   Observação6-   �� 5"   
   -   5        $
 ObservacaoTB��5"   i   $��2��6   $��1��6    �� TextWrapping$
 Wrap=�6/   " �� VerticalScrollBarVisibility$ Visible=�6C    �� AcceptsReturn .  �� 
AcceptsTab . 6�   �� 5#   
   $��3��6   6#   �� 5$   
   -   5        $��
MessageTBL��5$   z   $��3��6   $��2��6!   $��Center=�65   $��10,10,110,10q�6P   $
��Wrap=�6f   6�   �� 5%   
   -   5        $��
CadastrarB��5%   q   $��3��6   $��1��6   $��Right=�6.   $��Center=�6J   $��10q�6e   .��6�   	Cadastrar6�   5&      5'      