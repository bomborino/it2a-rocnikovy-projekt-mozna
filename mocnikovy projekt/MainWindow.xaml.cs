using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Xml.Serialization;
using System.ComponentModel;

namespace mocnikovy_projekt
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        bool hasScrewdriver = false, hasCable = false, hasKeys = false, hasTweezers = false, hasMoney = false;

        private int Kood;

        private string _code = string.Empty;
        public string Code
        {
            get => _code;
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        public MainWindow()
        {
            var rnd = new Random();
            Kood = rnd.Next(1000, 9999);
            InitializeComponent();

            // Set the DataContext so the TextBlock in XAML can bind to the Code property.
            DataContext = this;
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e) => SettingsPanel.Visibility = Visibility.Visible;
        private void CloseSettings_Click(object sender, RoutedEventArgs e) => SettingsPanel.Visibility = Visibility.Collapsed;
        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private async void StartGame_Click(object sender, RoutedEventArgs e)
        {
            MenuScene.Visibility = Visibility.Collapsed;
            Scene1.Visibility = Visibility.Visible;
            await ShowMsg("Zaspal jsi a nestíháš hodinu POGu a nemůžeš najít klíče, uteč z domu.", 6);
        }

        private async Task ShowMsg(string text, int sec = 4)
        {
            NotificationText.Text = text;
            NotificationPanel.Visibility = Visibility.Visible;
            await Task.Delay(sec * 1000);
            NotificationPanel.Visibility = Visibility.Collapsed;
        }

        private void Bed_Click(object sender, MouseButtonEventArgs e) => ShowMsg("Pod postelí nic není.");
        private void Pillow_Click(object sender, MouseButtonEventArgs e) { if (!hasScrewdriver) { hasScrewdriver = true; ShowMsg("Pod polštářem byl šroubovák. Jak jsi na tom spal?"); } else ShowMsg("Už tam nic není, díky bohu."); }

        private void PC_Click(object sender, MouseButtonEventArgs e)
        {
            if (hasCable)
            { 
                RoomBackground.Source = new BitmapImage(new Uri("pack://application:,,,/pocitacovypokoj.png"));
                aojadhagfuawhguhwRGHwroghpiWRGHIPwrphpgw.Visibility = Visibility.Visible;
                abcd.Visibility = Visibility.Visible;
                Code = $"Kód: {Kood}";
                ShowMsg("Zapojil jsi počíítač a na monitoru se něco zobrazilo.");
            }
            else ShowMsg("Počítač je odpojený.");
        }

        private void Cage_Click(object sender, MouseButtonEventArgs e) => ShowMsg("Králík má seno i vodu, yay.");
        private void GoToBathroom_Click(object sender, RoutedEventArgs e) { Scene1.Visibility = Visibility.Collapsed; Scene2.Visibility = Visibility.Visible; }
        private void Bath_Click(object sender, MouseButtonEventArgs e) { if (hasTweezers && !hasMoney) { hasMoney = true; ShowMsg("Pinzetou jsi z odtoku vany vytáhl padesátikorunu."); } else ShowMsg("Vana je prázdná, ale v odtoku se něco leskne"); }
        private void Cabinet_Click(object sender, MouseButtonEventArgs e) { if (!hasCable) { hasCable = true; ShowMsg("Našel jsi kabel!"); } else ShowMsg("Skříňka už je prázdná"); }

        private void Tile_Click(object sender, MouseButtonEventArgs e)
        {
            if (hasScrewdriver)
            {
                Tile_Zone.Visibility = Visibility.Collapsed;
                sejf.Visibility = Visibility.Visible;
                Safe_Hitbox.Visibility = Visibility.Visible;
                ShowMsg("Rozmlátil jsi kachličku a objevil sejf, jak dlouho to tady je?");
            }
            else ShowMsg("Kachlička je nějaká uvolněná, hmm.");
        }

        public void Safe_Click(object sender, MouseButtonEventArgs e)
        {
            string coder = Microsoft.VisualBasic.Interaction.InputBox("Zadej kód:", "Sejf", "");

            // parse the entered text and compare to the class-level Kood
            if (int.TryParse(coder, out int enteredCode) && enteredCode == Kood)
            {
                hasKeys = true;
                hasTweezers = true;
                sejf.Source = new BitmapImage(new Uri("pack://application:,,,/sejfotevreny.png"));
                Safe_Hitbox.Visibility = Visibility.Collapsed;
                ShowMsg("Sejf cvakl a odemčel se.");
            }
            else
            {
                ShowMsg("Špatný kód.");
            }
        }

        private void GoToRoom_Click(object sender, MouseButtonEventArgs e)
        {
            if (hasKeys)
            {
                string bonus = hasMoney ? "\n\nBONUS: Vyhrál jsi a máš i na 1,85185185185185 párku v rohlíku!!!" : "";
                MessageBox.Show("Dostal jsi se ven! " + bonus, "VÍTĚZSTVÍ!");
                Application.Current.Shutdown();
            }
            else { Scene2.Visibility = Visibility.Collapsed; Scene1.Visibility = Visibility.Visible; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}