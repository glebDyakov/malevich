   M S B A M L   `   `   `  ����@  <paint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:paint.Dialogspaint.Dialogs  5   	   %    paint.Dialogs.GaussFilterDialog   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:paint.Dialogs     5   	     � Title$(  "Размытие по Гауссу��5   	   
�������$	��350��66   $	��350��6C   .��5	      �  � RowDefinitions 5
   
   �� 5       � ColumnDefinitions 5   
   �� 5      .+�5   
   �� $��0��6   $��0��6"   .+�5      �� $��15q�6    �� Orientation$ 
Horizontal=�6&   .+�5      �� $	��200��6   $	��200��6%   
�������62   �� 5      $��15, 0, 0, 0q�6   .+�5      �� -   5        $��0, 0, 0, 10q�5      $
��ОК��6J   �� 5      -   5        $��Отмена��5      6.   �� 5      $��0, 15, 0, 0q�6!   $ 
Horizontal=�67   .+�5      �� $��0, 0, 15, 0q�6#   �� 5      
�������$��Просмотр��67   5      5      5      �� 5      $ 
Horizontal=�6   .+�5      �� 
�������$��15, 0, 0, 0q�6/   $��Радиус:��6D   �� 5      -   5         ��Name$ radius��5      
���iii�
�������$��50��6R   $��15, 0, 15, 0q�6]   $��1��6s   �� 5      
�������$��0, 0, 15, 0q�6/   $��Пиксели��6D   5       5!      5"      5#      