using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace mocnikovy_projekt
{
    public partial class MainWindow : Window
    {
        // Inventář a stavy hry
        bool hasScrewdriver = false;
        bool hasCable = false;
        bool hasKeys = false;
        bool hasTweezers = false;
        bool hasMoney = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartGame_Click(object sender, RoutedEventArgs e)
        {
            MenuScene.Visibility = Visibility.Collapsed;
            Scene1.Visibility = Visibility.Visible;
            await ShowMsg("Zaspal jsi a nestíháš hodinu POGu a nemůžeš najít klíče, uteč z vlastního domu.", 6);
        }

        private async Task ShowMsg(string text, int sec = 4)
        {
            NotificationText.Text = text;
            NotificationPanel.Visibility = Visibility.Visible;
            await Task.Delay(sec * 1000);
            NotificationPanel.Visibility = Visibility.Collapsed;
        }

        // --- SCÉNA 1: POKOJ ---
        private void Bed_Click(object sender, MouseButtonEventArgs e) => ShowMsg("Podíval jsi se pod postel, ale bohužel nic nenasel.");

        private void Pillow_Click(object sender, MouseButtonEventArgs e)
        {
            if (!hasScrewdriver)
            {
                hasScrewdriver = true;
                ShowMsg("Shodil jsi polštář z postele a našel jsi šroubovák pod tím, jak jsi na tom spal?");
            }
            else ShowMsg("Pod polštářem už nic dalšího není.");
        }

        private void PC_Click(object sender, MouseButtonEventArgs e)
        {
            if (hasCable) ShowMsg("Počítač jsi zapojil a zapnul, na monitoru se objevil kód: 6769");
            else ShowMsg("Počítač je odpojený, nic nedělá.");
        }

        private void Cage_Click(object sender, MouseButtonEventArgs e) => ShowMsg("Králík má i vodu i seno, yay.");

        private void GoToBathroom_Click(object sender, MouseButtonEventArgs e)
        {
            Scene1.Visibility = Visibility.Collapsed;
            Scene2.Visibility = Visibility.Visible;
        }

        // --- SCÉNA 2: KOUPELNA ---
        private void Bath_Click(object sender, MouseButtonEventArgs e)
        {
            if (hasTweezers && !hasMoney)
            {
                hasMoney = true;
                ShowMsg("Pomocí pinzety jsi vytáhl padesátikorunu! Ta se bude v kantýně hodit.");
            }
            else if (!hasMoney) ShowMsg("Ve vaně nic není, ale v odtoku se leskne padesátikoruna - ta by se hodila do kantýny.");
            else ShowMsg("Vana už je prázdná.");
        }

        private void Cabinet_Click(object sender, MouseButtonEventArgs e)
        {
            if (!hasCable)
            {
                hasCable = true;
                ShowMsg("Našel jsi napájecí kabel k počítači!");
            }
            else ShowMsg("Skříňka je teď prázdná.");
        }

        private void Tile_Click(object sender, MouseButtonEventArgs e)
        {
            if (hasScrewdriver)
            {
                Tile_Zone.Visibility = Visibility.Collapsed;
                Safe_Zone.Visibility = Visibility.Visible;
                ShowMsg("Vzal jsi šroubovák a kachličku vypáčil... Objevil jsi skrytý sejf!");
            }
            else ShowMsg("Tato kachlička není nějak pevná, hmm.");
        }

        private void Safe_Click(object sender, MouseButtonEventArgs e)
        {
            // Pro jednoduchost bez nutnosti reference na VisualBasic použijeme standardní WPF dialog
            // Pokud chceš hezčí, musel bys udělat vlastní okno
            string code = Microsoft.VisualBasic.Interaction.InputBox("Zadej kód (4 čísla):", "Sejf", "");

            if (code == "6769")
            {
                hasKeys = true;
                hasTweezers = true;
                Safe_Zone.Visibility = Visibility.Collapsed;
                ShowMsg("Sejf cvakl a otevřel se! Sebral jsi klíče a pinzetu.");
            }
            else if (code != "") ShowMsg("Špatný kód! Zkus se podívat po pokoji.");
        }

        private void GoToRoom_Click(object sender, MouseButtonEventArgs e)
        {
            if (hasKeys)
            {
                string bonus = hasMoney ? "\n\nBONUS: Vyhrál jsi a dokonce máš i na 1,85185185 párku v rohlíku!!!" : "";
                MessageBox.Show("Dostal jsi se ven! " + bonus, "VÍTĚZSTVÍ");
                Application.Current.Shutdown();
            }
            else
            {
                Scene2.Visibility = Visibility.Collapsed;
                Scene1.Visibility = Visibility.Visible;
            }
        }

        // --- MENU ---
        private void Settings_Click(object sender, RoutedEventArgs e) => SettingsPopup.Visibility = Visibility.Visible;
        private void CloseSettings_Click(object sender, RoutedEventArgs e) => SettingsPopup.Visibility = Visibility.Collapsed;
        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}