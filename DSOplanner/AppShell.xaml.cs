namespace DSOplanner
{
    public partial class AppShell : Shell
    {
        private bool _isOpen = true;
        public AppShell()
        {
            InitializeComponent();
        }
        // Metoda wywoływana po kliknięciu przycisku toggle
        private void ToggleButton_Clicked(object sender, EventArgs e)
        {
            if (_isOpen)
            {
                // Zwijamy panel – zmieniamy szerokość kolumny na mniejszą, np. 60
                SideNavColumn.Width = new GridLength(60);
                Shell.SetFlyoutWidth(this, 60); // Fix: Added 'this' as the first argument

                ToggleButton.Text = "Rozwiń";
            }
            else
            {
                // Rozwijamy panel – przywracamy pierwotną szerokość (250)
                SideNavColumn.Width = new GridLength(250);
                Shell.SetFlyoutWidth(this, 250); // Fix: Added 'this' as the first argument
                ToggleButton.Text = "Zwiń";
            }
            _isOpen = !_isOpen;
        }
    }
}
