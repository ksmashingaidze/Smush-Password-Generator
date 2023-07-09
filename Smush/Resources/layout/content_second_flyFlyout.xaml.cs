using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smush.Resources.layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class content_second_flyFlyout : ContentPage
    {
        public ListView ListView;

        public content_second_flyFlyout()
        {
            InitializeComponent();

            BindingContext = new content_second_flyFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class content_second_flyFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<content_second_flyFlyoutMenuItem> MenuItems { get; set; }

            public content_second_flyFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<content_second_flyFlyoutMenuItem>(new[]
                {
                    new content_second_flyFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new content_second_flyFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new content_second_flyFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new content_second_flyFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new content_second_flyFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}