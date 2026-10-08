using ShoesDEE.DataBase;
using ShoesDEE.Helpers;
using ShoesDEE.Statics;
using ShoesDEE.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShoesDEE
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private pupenkoshoesbdi43Entities1 _db = new pupenkoshoesbdi43Entities1();
        private MessageHelper _mbh = new MessageHelper();
        private List<ProductViewModel> _productViewModels = new List<ProductViewModel>();

        private string[] _sortingTypes = new string[]
        {
            "По умолчанию",
            "По убыванию",
            "По возрастанию"
        };
        private List<string> _filteringTypes = new List<string>()
        {
            "Все поставщики"
        };

        public ProductWindow(User user)
        {
            InitializeComponent();
            LoadProducts();
            LoadData();
            LoadUI();
        }

        private void LoadUI()
        {
            User user = CurrentSession.CurrentUser;
            if (user == null || user.RoleId == 3)
            {
                AdminPanel.Visibility = Visibility.Collapsed;
            }
            else if (user.RoleId == 1)
            {
                CreateButton.Visibility = Visibility.Visible;
            }
        }
        private void LoadData()
        {
            SortingComboBox.ItemsSource = _sortingTypes;
            SortingComboBox.SelectedIndex = 0;
            var provider = _db.Provider.ToList();
            foreach (var p in provider)
                _filteringTypes.Add(p.Name);
            FilteringComboBox.ItemsSource = _filteringTypes;
            FilteringComboBox.SelectedIndex = 0;
            User user = CurrentSession.CurrentUser;
            if (user != null)
                FullUserName.Text = $"{user.Surname} {user.Name} {user.Patronmic}";
        }

        
        public ProductWindow(UserControl user)
        {
            InitializeComponent();
            LoadProducts();
            
        }

        public ProductWindow()
        {
            InitializeComponent();
            LoadProducts();
            LoadData();
            LoadUI();
        }

        private void LoadProducts()
        {
            var products = _db.Product.ToList();
            foreach (var p in products)
            
                _productViewModels.Add(new ProductViewModel(p));
            
            ProductList.ItemsSource = _productViewModels;
        }
        private void UpdateProducts()
        {
            ProductList.ItemsSource = _productViewModels;
        }


        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;
            new MainWindow().Show();
            Close();
        }

        private void SearchingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchingText = SearchingTextBox.Text;
            _productViewModels = _db.Product
                .Where(p =>
                p.Category.Name.ToLower().Contains(searchingText) ||
                p.Name.ToLower().Contains(searchingText) ||
                p.Description.ToLower().Contains(searchingText) ||
                p.Provider.Name.ToLower().Contains(searchingText) ||
                p.Producer.Name.ToLower().Contains(searchingText) ||
                p.Unit.Name.ToLower().Contains(searchingText)
                )
                .ToList()
                .Select(p => new ProductViewModel(p))
                .ToList();
            UpdateProducts();
        }

        private void SortingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sortingType = SortingComboBox.SelectedIndex;
            if (sortingType == 0)
            {
                LoadProducts();
            }
            else if (sortingType == 1)
            {
                _productViewModels = _productViewModels
                    .OrderByDescending(p => p.AmountInStock)
                    .ToList();
                UpdateProducts();
            }
            else if (sortingType == 2)
            {
                _productViewModels = _productViewModels
                    .OrderBy(p => p.AmountInStock)
                    .ToList();
                UpdateProducts();
            }
        }

        private void FilteringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string filterText = FilteringComboBox.SelectedValue.ToString();
            if (filterText == "Все поставщики")
            {
                LoadProducts();
                return;
            }
            _productViewModels = _productViewModels
                .Where(p => p.Provider.Name == filterText)
                .ToList();
            UpdateProducts();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            new AddEditProductWindow(null).Show();
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            User user = CurrentSession.CurrentUser;
            if (user == null || user.RoleId != 1)
                return;
            int id = (int)(sender as Border).Tag;

            new AddEditProductWindow(id).Show();   
            Close();
        }
    }
}
