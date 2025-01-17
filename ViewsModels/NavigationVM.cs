using Chef_tracker.Properties;
using Chef_tracker.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chef_tracker.ViewsModels
{
    public class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand AdminHomeCommand { get; set; }
        public ICommand TasksCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand AdminTaskCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand AddUserCommand { get; set; }
        public ICommand AdminConCommand { get; set; }
        public ICommand AdminProductsCommand { get; set; }
        public ICommand AdminUsersCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }
        public ICommand TaskByTaskCommand { get; set; }
        public ICommand AddTaskByTaskCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void AdminHome(object obj) => CurrentView = new AdminHomeVM();
        private void Tasks(object obj) => CurrentView = new TasksVM();
        private void AddTask(object obj) => CurrentView = new AddTaskVM();
        private void Products(object obj) => CurrentView = new ProductsVM();
        private void AdminTask(object obj) => CurrentView = new AdminTaskVM();
        private void AddProduct(object obj) => CurrentView = new AddProductVM();
        private void AddUser(object obj) => CurrentView = new AddUserVM();
        private void TaskByTask(object obj) => CurrentView = new TaskByTaskVM();
        private void AddTaskByTask(object obj) => CurrentView = new addTaskByTaskVM();
        private void AdminCon(object obj)
        {
            CurrentView = new AdminConVM();
            Console.WriteLine("Current modifier !!!!!");
        }

        private void AdminProducts(object obj) => CurrentView = new AdminProductsVM();
        private void AdminUsers(object obj) => CurrentView = new AdminUsersVM();
        public NavigationVM()
        {
            try {
                HomeCommand = new RelayCommand(Home);
                AdminHomeCommand = new RelayCommand(AdminHome);
                TasksCommand = new RelayCommand(Tasks);
                ProductsCommand = new RelayCommand(Products);
                AdminTaskCommand = new RelayCommand(AdminTask);
                AddProductCommand = new RelayCommand(AddProduct);
                AddUserCommand = new RelayCommand(AddUser);
                AdminConCommand = new RelayCommand(AdminCon);
                AdminProductsCommand = new RelayCommand(AdminProducts);
                AdminUsersCommand = new RelayCommand(AdminUsers);
                AddTaskCommand = new RelayCommand(AddTask);
                TaskByTaskCommand = new RelayCommand(TaskByTask);
                AddTaskByTaskCommand = new RelayCommand(AddTaskByTask);
                 // Startup Page
                 CurrentView = new AdminHomeVM();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
           
        }
    }
}
