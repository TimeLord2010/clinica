   M S B A M L   `   `   `  ����E  AWPFClinica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFClinica.WindowsWPFClinica.Windows  5   	   "    WPFClinica.Windows.LembrarEm   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WPFClinica.Windows     5   	     � Title$  
Lembrar em��5      $	��200��6-   $	��300��6:    � WindowStartupLocation$ CenterScreen=�6F    � WindowStyle$ 
ToolWindow=�6k    � 	Resources 5       �� 
TargetType �� Property �� Value �� BasedOn��%  '	�       '�� L     '�� �     7���� 5	   
   " 	�6   .�5
      ��  � ��$ 5q�6'   63   5      �� 5   
   " ��6   .�5      ��  � ��$ 15(�6)   66   5      �� 5   
   " ��6   8   .�5      ��  � ��$ 5q�6'   63   5      5      .��5      � .+�5   
   �� $��Center=�6   $��Center=�62   .+�5      	� -   5         	�Name$ SixMonthsRB��5      .��5      �� 
6 meses.6&   5      	� 5      -   5        $ TwelveMonthsRB��5      	 d� 	IsChecked$
	 TrueY�62   .��5      �� 	12 meses.6'   5      	� 5      -   5        $ CustomRB��5      .��5      � 
 � ColumnDefinitions
 5      �� 5      �� 5      $��50 �6+   .+�5!      �� -   5        $��CustomTB��5!   )   $��50��6   6;   �� 5"      $��1��6    $�� meses��60   6@   5#      5$      �� 5%      -   5        $	��OkB��5%   1   $��Right=�6   .��6N   Ok6R   5&      5'      5(      