using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DSOplanner.ViewModels;
using Microsoft.Maui.Controls;

namespace DSOplanner
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<DsoViewModel> DsoObjects { get; set; } = new ObservableCollection<DsoViewModel>();
        private bool _isLoading = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDsoData();
        }

        private async Task LoadDsoData()
        {
            if (_isLoading) return;
            _isLoading = true;

            await DsoCsvLoader.InitializeReader("merged_telescopius.csv");

            var batch = await DsoCsvLoader.GetNextBatch();
            if (batch.Count > 0)
            {
                // Batch update UI
                await Dispatcher.DispatchAsync(() =>
                {
                    foreach (var dso in batch)
                        DsoObjects.Add(dso);
                });
            }

            _isLoading = false;
        }

        private async void OnItemAppearing(object sender, ItemsViewScrolledEventArgs e)
        {
            if (_isLoading || DsoObjects.Count == 0) return;

            if (DsoObjects.Count - e.LastVisibleItemIndex < 10) // Load more when nearing the end
            {
                await LoadMoreDso();
            }
        }

        private async void OnRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            await LoadMoreDso();
        }

        public async Task LoadMoreDso()
        {
            if (_isLoading) return;
            _isLoading = true;

            var nextBatch = await DsoCsvLoader.GetNextBatch();
            if (nextBatch.Count > 0)
            {
                await Dispatcher.DispatchAsync(() =>
                {
                    foreach (var dso in nextBatch)
                        DsoObjects.Add(dso);
                });
            }

            _isLoading = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
