   M S B A M L   `   `   `  ����[  WST10034939_PROG6212_POE_PART_ONE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:ST10034939_PROG6212_POE_PART_ONE ST10034939_PROG6212_POE_PART_ONE  5   	   1    +ST10034939_PROG6212_POE_PART_ONE.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:ST10034939_PROG6212_POE_PART_ONE     5   	     � Title$  
MainWindow��5   	   $	��730��6   $	��802��6)   .��5	      � $��10q�6   .+�5   
   �� $��Enter Program Code: ��6   $��
0,-1,616,0q�6/   $	��Top=�6C   �� 5   
   $��Class hours per week:��6   $��0,123,600,0q�60   $	��Top=�6E   �� 5   
   $"��Enter weeks in the semester:��6   $��
0,89,600,0q�67   $	��Top=�6K   �� 5   
   $��Number of Credits:��6   $��-1,59,601,0q�6-   $	��Top=�6B   �� 5   
   -   5         ��Name$ txtCode��5      $	��150��6#   $
��Left=�6/   $	��Top=�6J   $��Prog6212��6b   $��	185,3,0,0q�6r   �� 5   
   -   5        $ txtName��5      $	��151��6#   $
��Left=�6/   $	��Top=�6J   $��
184,33,0,0q�6b   $��Programming 2B��6v   �� 5   
   -   5        $ 
txtCredits��5      $	��100��6&   $
��Left=�62   $	��Top=�6M   $��
185,67,0,0q�6e   $��15��6y   �� 5   
   -   5        $ txtWeeksSemester��5      $	��100��6,   $
��Left=�68   $	��Top=�6S   $��
185,97,0,0q�6k   $��5��6   �� 5   
   $��Enter Program Name: ��6   $��-1,29,616,0q�6/   $	��Top=�6D   �� 5   
   -   5        $ txtWeeks��5      $��184,131,476,0q�6$   $	��Top=�6;    J� RenderTransformOrigin$ 	0.5,0.615&�6S   6u   �� 5   
   $(��"Enter start date for the semester:��6   $��0,158,591,0q�6=   $	��Top=�6R   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ dpStartDate��5      $��196,160,418,0q�6*   $	��Top=�6A   �� 5   
   -   5        $��
Add Module��5      $	��240��6&   $��30��62   $
��Left=�6>   $	��Top=�6Y   $��
94,206,0,0q�6q   $ 0.74,-1.432&�6�   �� 5    
   -   5         ��Name$ ModulesList��5       $
��Left=�6(   $	��100��6C   $	��Top=�6P   $	��608��6h   $��
-1,264,0,0q�6t    �� View 5!      �� 5"      .P�5#      �� $
��Code��6%    �� Width$	 100��6Y    �� DisplayMemberBinding 63   �� *Code+6Y   �� 5$      $
��Name��6%   $	 150��6Y    63   �� *Name+6Y   �� 5%      $��Credits��6%   $ 70��6_    66   �� *	Credits+6_   �� 5&      $��Class Hours��6%   $ 90��6m    6:   �� *ClassHoursPerWeek+6m   �� 5'      $��Self Study Hours/Week��6%   $	 140��6t    6D   �� *SelfStudyHours+6t   5(      5*      �� 5,   
   $��Record Study Hours��6   $��17,369,467,0q�6-   $	��Top=�6C   �� 5-   
   $��Amount of hours:��6   $��10,429,640,0q�6+   $	��Top=�6A   �� 5/   
   -	   5         ��Name$ 	cmbModule��5/      $	��150��6&   $
��Left=�62   $	��Top=�6M   $��
10,395,0,0q�6e     50   
   -
   5        $ 	studyDate��50      $��186,394,0,0q�6(   $
��Left=�6=   $	��Top=�6X   �� 51   
   -   5        $ txtHoursStudied��51      $��50��6+   $
��Left=�66   $	��Top=�6Q   $��168,433,0,0q�6i   $�� ��6~   $ -0.04,0.559&�6�   �� 52   
   -   5        $��Record Hours��52      $	��278��6(   $��30��64   $
��Left=�6@   $	��Top=�6[   $��
10,460,0,0q�6s   6�   �� 54   
   -   5        $��Update Remaining Hours��54      $	��242��62   $��30��6>   $
��Left=�6J   $	��Top=�6e   $��
46,536,0,0q�6}   6�   &   System.Windows.Controls.DataGrid  57   
   -   5        	  Name$	 RemainingHoursDataGrid��57      $
��Left=�63   $	��Top=�6N   $��
17,589,0,0q�6f   
   Columns
 58      0  *System.Windows.Controls.DataGridTextColumn  59      ,  &System.Windows.Controls.DataGridColumn   Header$ Module��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6N   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *Name+6N     5:      $ Remaining Hours��6%   $	 150 6a    6>   �� *RemainingHours+6a   5<      5B      5C      