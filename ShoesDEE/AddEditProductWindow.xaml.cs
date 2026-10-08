using ShoesDEE.DataBase;
using ShoesDEE.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для AddEditProductWindow.xaml
    /// </summary>
    public partial class AddEditProductWindow : Window
    {
        private pupenkoshoesbdi43Entities1 _db = new pupenkoshoesbdi43Entities1();
        private MessageHelper _mh = new MessageHelper();
        private bool _isEditing;
        private Product _product;
        

        public AddEditProductWindow(int? id)
        {
            InitializeComponent();

            if (id == null)
            {
                _isEditing = false;
            }
            else
            {
                _isEditing = true;
                _product = _db.Product.Find(id);
            }
            LoadData();

        }

        private void LoadData()
        {
            var units = _db.Unit.ToList();
            var categories = _db.Category.ToList();
            var producer = _db.Producer.ToList();
            var providers = _db.Provider.ToList();

            ProductUnit.ItemsSource = units;          
            ProductUnit.DisplayMemberPath = "Name";
            ProductUnit.SelectedValuePath = "Id";
            ProductUnit.SelectedIndex = 0;

            ProductProducer.ItemsSource = producer;           
            ProductProducer.DisplayMemberPath = "Name";
            ProductProducer.SelectedValuePath = "Id";
            ProductProducer.SelectedIndex = 0;

            ProductProvider.ItemsSource = providers;            
            ProductProvider.DisplayMemberPath = "Name";
            ProductProvider.SelectedValuePath = "Id";
            ProductProvider.SelectedIndex = 0;

            ProductCategory.ItemsSource = categories;
            ProductCategory.DisplayMemberPath = "Name";
            ProductCategory.SelectedValuePath = "Id";
            ProductCategory.SelectedIndex = 0;

            if (_isEditing == true)
                FillData();
        }

        private void FillData()
        {
            ProductArticule.Text = _product.Article;
            ProductName.Text = _product.Name;
            ProductPrice.Text = _product.Price.ToString();
            ProductDicsount.Text = _product.Discount.ToString();
            ProductAmountInStock.Text = _product.AmountInStock.ToString();
            ProductDesctiption.Text = _product.Description;
            ProductPhoto.Text = _product.Photo;

            ProductUnit.SelectedValue = _product.UnitId;
            ProductProducer.SelectedValue = _product.ProducerId;
            ProductProvider.SelectedValue = _product.ProviderId;
            ProductCategory.SelectedValue = _product.CategoryId;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductWindow().Show();
            Close();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            if (_isEditing == true)
                UpdateProduct();
            else
                CreateProduct();
        }

        private void CreateProduct()
        {
            Product product = new Product();

            string article = ProductArticule.Text;
            string name = ProductName.Text;
            decimal price = Convert.ToDecimal(ProductPrice.Text);
            decimal discount = Convert.ToDecimal(ProductDicsount.Text);
            decimal amount = Convert.ToDecimal(ProductAmountInStock.Text);
            string description = ProductDesctiption.Text;

            product.Article = article;
            product.Name = name;
            product.Price = price;
            product.Discount = discount;
            product.AmountInStock = amount;
            product.Description = description;
            product.Photo = ProductPhoto.Text;

            product.UnitId = (int)ProductUnit.SelectedValue;
            product.ProducerId = (int)ProductProducer.SelectedValue;
            product.ProviderId = (int)ProductProvider.SelectedValue;
            product.CategoryId = (int)ProductCategory.SelectedValue;

            try
            {
                _db.Product.AddOrUpdate(product);
                _db.SaveChanges();
                _mh.ShowInfo("Продукт успешно создан!");
                CancelButton_Click(null, null);

            }
            catch (Exception ex)
            {
                _mh.ShowError(ex.Message);
            }
        }

        private void UpdateProduct()
        {
            string article = ProductArticule.Text;
            string name = ProductName.Text;
            decimal price = Convert.ToDecimal(ProductPrice.Text);
            decimal discount = Convert.ToDecimal(ProductDicsount.Text);
            decimal amount = Convert.ToDecimal(ProductAmountInStock.Text);
            string description = ProductDesctiption.Text;


            _product.Article = article;
            _product.Name = name;
            _product.Price = price;
            _product.Discount = discount;
            _product.AmountInStock = amount;
            _product.Description = description;
            _product.Photo = ProductPhoto.Text;

            _product.UnitId = (int)ProductUnit.SelectedValue;
            _product.ProducerId = (int)ProductProducer.SelectedValue;
            _product.ProviderId = (int)ProductProvider.SelectedValue;
            _product.CategoryId = (int)ProductCategory.SelectedValue;

            try
            {
                _db.Product.AddOrUpdate(_product);
                _db.SaveChanges();
                _mh.ShowInfo("Продукт успешно изменен!");
                CancelButton_Click(null, null);

            }
            catch (Exception ex)
            {
                _mh.ShowError(ex.Message);
            }
        }

        private bool ValidateInput()
        {
            StringBuilder errors = new StringBuilder();



            string article = ProductArticule.Text;
            string name = ProductName.Text;
            string price = ProductPrice.Text;
            string discount = ProductDicsount.Text;
            string amount = ProductAmountInStock.Text;
            string description = ProductDesctiption.Text;

            if (string.IsNullOrEmpty(article))
                errors.AppendLine("Поле артикул не заполнено!");
            if (string.IsNullOrEmpty(name))
                errors.AppendLine("Поле наименование не заполнено!");
            if (string.IsNullOrEmpty(price)
                || !decimal.TryParse(price, out decimal priceDecimal))
                errors.AppendLine("Поле цены не заполнено!");
            if (string.IsNullOrEmpty(discount)
                || !decimal.TryParse(discount, out decimal discountDecimal)
                || discountDecimal > 100 || discountDecimal < 0)
                errors.AppendLine("Поле скидки не заполнено!");
            if (string.IsNullOrEmpty(amount)
                || !decimal.TryParse(amount, out decimal amountDecimal)
                || amountDecimal < 0)
                errors.AppendLine("Поле кол-во не заполнено!");
            if (string.IsNullOrEmpty(description))
                errors.AppendLine("Поле описания не заполнено!");

            if (errors.Length > 0)
            {
                {
                    _mh.ShowError(errors.ToString());
                    return false;
                }
                
            }
            return true;
        }
    }
}
