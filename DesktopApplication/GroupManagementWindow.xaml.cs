﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class GroupManagementWindow : Window
{
    private DataRepository _dataRepository;
    
    public GroupManagementWindow(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        CourseComboBox.ItemsSource = _dataRepository.Courses;
        CourseComboBox.SelectionChanged += CourseComboBox_SelectionChanged;
    }

    private void EditGroupName_Click(object sender, RoutedEventArgs e)
    {
        string selectedGroupName = EditGroupNameComboBox.SelectedItem as string;
        string newGroupName = EditGroupNameTextBox.Text;

        if (!string.IsNullOrWhiteSpace(selectedGroupName) && !string.IsNullOrWhiteSpace(newGroupName))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;
           
            if (selectedCourse != null)
            {
                Group editGroupName = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
               
                if (editGroupName != null)
                {
                    editGroupName.GroupName = newGroupName;

                    EditGroupNameTextBox.Clear();
                    
                    DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                }
            }
            else
            {
                MessageBox.Show("Please, select a course to edit the group name within", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please select a group and provide a new group name", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
    
    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        string groupName = GroupNameTextBox.Text;

        if (!string.IsNullOrWhiteSpace(groupName))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;
            
            if (selectedCourse != null)
            {
                bool groupNameExists = selectedCourse.Groups.Any(group => group.GroupName == groupName);
                
                if(!groupNameExists)
                {
                    selectedCourse.LastUsedGroupId++;

                    Group createNewGroup = new Group
                    {
                        GroupId = selectedCourse.CourseId * 10 + selectedCourse.LastUsedGroupId,
                        GroupName = groupName
                    };
                        
                    selectedCourse.Groups.Add(createNewGroup);
                
                    GroupNameTextBox.Clear();
                
                    DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                }
                else
                {
                    MessageBox.Show("A group with same name already exist. Please, select other name for new group", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please, select a course to add group to it", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, enter a valid group name", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        string preferableGroupToDelete = DeleteGroupComboBox.SelectedItem as string;

        if (!string.IsNullOrWhiteSpace(preferableGroupToDelete))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;

            if (selectedCourse != null)
            {
                Group deleteGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == preferableGroupToDelete);
                
                if (deleteGroup != null)
                {
                    selectedCourse.Groups.Remove(deleteGroup);

                    DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                }
            }
            else
            {
                MessageBox.Show("Please, select a course to delete the group from", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select a group from course to delete", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Course selectedCourse = CourseComboBox.SelectedItem as Course;

        if (selectedCourse != null)
        {
            DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
        }
    }

    private void RefreshAllGroupComboBoxes()
    {
        // Need to add later a future realization of sorta refresher to all group combo boxes smth like "CourseComboBox_SelectionChanged"
    }
}