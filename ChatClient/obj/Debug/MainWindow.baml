   M S B A M L   `   `   `  ����E  AChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ChatClient
ChatClient  5   	       ChatClient.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:ChatClient     5   	   -   5          � Title$  Чат��5   	   $	��370��6   $	��525��6"   .��5	      � ���#3d4540 � ColumnDefinitions 5
   
   �� 5      $
��103* �6   �� 5      $
��257* �6   �� 5      $
��165* �6   .+�5   
   �� -   5         ��Name$ 
tbUserName��5      
�������$
��Left=�6<   $��14��6W   $��	239,9,0,0q�6c   $
��Wrap=�6v   $�� ��6�   $	��Top=�6�   $	��158��6�   $��2��6�   $��1��6�   �� 5   
   -   5         ��Name$ 
bConnDicon��5      
����������#7e4345$��
Disconnect��6g   $
��Left=�6|   $��
138,10,0,0q�6�   $	��Top=�6�   $��82��6�   $��29��6�   $��1��6�   �� 5   
   -   5         ��Name$ lbChat��5      ���#bfd2c6$��Right=�68   $	��170��6T   $��	0,55,25,0q�6a   $	��Top=�6t   $	��271��6�   $��2��6�   $��1��6�   6�   �� 5   
   -   5         ��Name$ 	tbMessage��5      ���#bfd2c6$��Right=�6;   $��79��6W   $��
0,230,72,0q�6c    �� TextWrapping$
 Wrap=�6w   $�� ��6�   $	��Top=�6�   $	��224��6�   $��2��6�   $��1��6�   �� 5   
   -   5        $ lbOnlineUsers��5      ���#bfd2c6$��2��6p   $��	10,55,0,0q�6�   $
��Left=�6�   $	��147��6�   $	��254��6�   $	��Top=�6�   �� 5   
   $+��%Пользователи в сети:��6   
�������$
��Left=�6B   $��6,31,0,0q�6]   $	��Top=�6o   $��2��6�   $��24��6�   $	��155��6�   �� 5   
   -   5        $��Общий Чат��5      ���#3c7b84$
��Left=�6X   $��	10,14,0,0q�6s   $	��Top=�6�   $��22��6�   $	��147��6�   $��2��6�   �� 5   
   -   5        $ WhatIsTheChat��5      
�������$
��Left=�6?   $��14��6Z   $��
239,24,0,0q�6f   $
��Wrap=�6z   $��Общий чат��6�   $	��Top=�6�   $	��158��6�   $��2��6�   $��1��6�   �� 5   
   -	   5        $��2��5      ���#3c7b84$
��File��66   $��Right=�6E   $��
0,230,25,0q�6a   $��42��6�   $��32��6�   $	��Top=�6�   �� 5   
   -
   5        $��2��5      ���#3c7b84$
��Send��66   $��Right=�6E   $��
0,277,25,0q�6a   $��42��6�   $��32��6�   $	��Top=�6�   5      5      