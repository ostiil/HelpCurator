   M S B A M L   `   `   `  ����=  9DP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:DPDP  5   	       DP.AutorizeForm   5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   "localclr-namespace:DP     5   	     � Title$  Авторизация��5	   	   $	��300��6   $	��500��6*    � WindowStartupLocation$ CenterScreen=�66    � 
ResizeMode$ NoResize=�6[    � 	Resources 5
         BlackAndWhite �� Property �� Value��%�   &	        �� 5   
   .�5      ��  � ��$ Comic Sans MS)�63   �� 5       � ��$ 14(�61   �� 5       � ��$ 30��6/   5      5      .��5      � ���#E4EDDB � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      $
��0.3* �6    � ColumnDefinitions 5   
   �� 5      $��3* �6   �� 5      $	��10* �6   �� 5      $��3* �6   .+�5   
   �� -   5        $��
StartLabel��5      $��0��6"   $��0��6/   $��3��6?   $'��!Помощник куратора��6S   $��Center=�6o   $��Center=�6�   
���   �$��Comic Sans MS)�6�   $��40(�6�   �� 5   
   $��1��6   $��1��6"   $��Center=�62   $
��10 0q�6M   .+�5      G� -   5         G�Name$ PassTb��5      )  #MaterialDesignThemes.Wpf.HintAssist	   Hint$	 Пароль��5       .  (MaterialDesignThemes.Wpf.TextFieldAssist
   HasClearButton$

 True��5!       + 'MaterialDesignOutlinedRevealPasswordBox#��[ 5"      
���   ����#8AAE926m   5#      �� 5$   
   -   5        $��AutorizeBtn��5$      $��2��6$   $��1��61   $"��Авторизоваться��6A   ���#8AAE92$��Center=�6o   $��Comic Sans MS)�6�   $��20(�6�   $
��10 0q�6�    ! MaterialDesignPaperDarkButton#��[ 6�   �� 5%   
   -   5        $��
ChangePass��5%      $��3��6#   $��1��60   $!��Сменить пароль��6@   ���#8AAE92 J� Opacity$	 0.7Q�6n   $��Center=�6|   $��Comic Sans MS)�6�   $��10(�6�   $
��70 0q�6�   $��20��6�   #��[ 6�   6  5&      5'      