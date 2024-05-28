using System;
using System.Collections.Generic;
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
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            var enumValues = Enum.GetValues(typeof(FridgeInventory.FridgeItemType));
            foreach (FridgeItemType value in enumValues)
            {
                var checkBox = new CheckBox()
                {
                    Content = value.ToString(),
                    IsChecked = false,
                    Name = value.ToString()
                };
                EnumListBox.Items.Add(checkBox);
            }
            EnumListBox.Visibility = Visibility.Collapsed;
        }

        private void AutomaticShopingList_OnClick(object sender, RoutedEventArgs e)
        {
            if (AutomaticShopingList.IsChecked is null or false)
            {
                foreach (var item in EnumListBox.Items)
                {
                    if (item is CheckBox checkBox)
                    {
                        checkBox.IsChecked = false;
                    }
                }
                EnumListBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                EnumListBox.Visibility = Visibility.Visible;
            }
        }
    }
}
