using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace DSOplanner
{
    public partial class MainPage : ContentPage
    {
        // Kolekcja obiektów DSO
        public ObservableCollection<DsoViewModel> DsoObjects { get; set; }

        public MainPage()
        {
            InitializeComponent();

            // Inicjalizacja przykładowych danych
            DsoObjects = new ObservableCollection<DsoViewModel>
            {
                new DsoViewModel { Name = "M31 – Galaktyka Andromedy", Description = "Największa galaktyka lokalna", Type = "Galaktyka", Excluded = false },
                new DsoViewModel { Name = "M42 – Mgławica Oriona", Description = "Jasna mgławica, widoczna gołym okiem", Type = "Mgławica", Excluded = false },
                new DsoViewModel { Name = "M13 – Wielki Globular", Description = "Jeden z najjaśniejszych gromad kulistych", Type = "Gromada", Excluded = true }
            };

            ItemsListView.ItemsSource = DsoObjects;

            SessionDatePicker.DateSelected += SessionDatePicker_DateSelected;
        }

      

        private void SessionDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            // Tutaj możesz dodać logikę filtrowania obiektów na podstawie wybranej daty sesji.
            // Przykład: FilterDsoObjects(e.NewDate);
        }
    }

    // ViewModel reprezentujący obiekt DSO do wyświetlania w liście
    public class DsoViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Excluded { get; set; }
    }
}
