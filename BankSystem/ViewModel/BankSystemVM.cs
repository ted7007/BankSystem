using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BankSystem.Model;
using BankSystem.Model.Client;
using BankSystem.View;

namespace BankSystem.ViewModel
{
    class BankSystemVM : INotifyPropertyChanged
    {
        #region fields
        private Model.BankSystem bankSystem;

        private ButtonCommand addCommand;

        private ButtonCommand removeCommand;

        private ButtonCommand editCommand;

        private ButtonCommand nextMonthCommand;

        private ButtonCommand showFAQCommand;

        private RegularUser selectedRegularProfile;

        private VIPClient selectedVIPClient;

        private EntityClient selectedEntityClient;

        private TabItem selectedDepartament;

        #endregion

        #region properties

        /// <summary>
        /// Отдел с обычными клиентами
        /// </summary>
        public CustomerServiceDepartament<RegularUser> RegularDepartament { get { return bankSystem.RegularUserDepartament; } }

        /// <summary>
        /// Отдел с ВИП клиентами
        /// </summary>
        public CustomerServiceDepartament<VIPClient> VIPDepartament { get { return bankSystem.VIPClientDepartament; } }

        /// <summary>
        /// Отдел с юр. лицами
        /// </summary>
        public CustomerServiceDepartament<EntityClient> EntityDepartament { get { return bankSystem.EntityClientDepartament; } }

        /// <summary>
        /// Выбранный отдел в окне вкладок
        /// </summary>
        public TabItem SelectedDepartament { get { return selectedDepartament; } set { selectedDepartament = value; OnPropertyChanged("SelectedDepartament"); } }

        /// <summary>
        /// Выбранный профиль обычного клиента
        /// </summary>
        public RegularUser SelectedRegularProfile { get { return selectedRegularProfile; } set { selectedRegularProfile = value; OnPropertyChanged("SelectedRegularProfile"); } }

        /// <summary>
        /// Выбранный профиль ВИП клиента
        /// </summary>
        public VIPClient SelectedVIPClient { get { return selectedVIPClient; } set { selectedVIPClient = value; OnPropertyChanged("SelectedVIPClient"); } }

        /// <summary>
        /// Выбранный профиль юр. лица
        /// </summary>
        public EntityClient SelectedEntityClient { get { return selectedEntityClient; } set { selectedEntityClient = value; OnPropertyChanged("SelectedEntityClient"); } }

        /// <summary>
        /// Команда на добавление профиля
        /// </summary>
        public ButtonCommand AddCommand { get { return addCommand ?? (addCommand = new ButtonCommand(sender => AddPorfile())); } }

        /// <summary>
        /// Команда на удаление профиля
        /// </summary>
        public ButtonCommand RemoveCommand { get { return removeCommand ?? (removeCommand = new ButtonCommand(sender => RemoveProfile())); } }

        /// <summary>
        /// Команда для перехода в окно редактирования профиля
        /// </summary>
        public ButtonCommand EditCommand { get { return editCommand ?? (editCommand = new ButtonCommand(sender => EditProfile())); } }

        /// <summary>
        /// Команда для перехода на следующий месяц
        /// </summary>
        public ButtonCommand NextMonthCommand { get{ return nextMonthCommand ?? (nextMonthCommand = new ButtonCommand(sender => NextMonth())); } }

        /// <summary>
        /// Команда для показа FAQ
        /// </summary>
        public ButtonCommand ShowFAQCommand { get { return showFAQCommand ?? (showFAQCommand = new ButtonCommand(sender => MessageBox.Show(FAQ))); } }

        public string FAQ { get { return bankSystem.FAQ; } }

        #endregion

        public BankSystemVM()
        {
            this.bankSystem = Model.BankSystem.GenerateSystem(12, 24, 5);
        }

        #region methods

        /// <summary>
        /// Метод добавления профиля в выбранный отдел
        /// </summary>
        public void AddPorfile()
        {
            switch (selectedDepartament.Header as string)
            {
                case "Regular Clients":
                    RegularDepartament.AddProfile(new RegularUser("RegularUser", RegularDepartament.DepositeRate, RegularDepartament.LoanRate));
                    break;
                case "VIP Clients":
                    VIPDepartament.AddProfile(new VIPClient("VIPClient", VIPDepartament.DepositeRate, VIPDepartament.LoanRate));
                    break;
                case "Entity Clients":
                    EntityDepartament.AddProfile(new EntityClient("EntityClient", EntityDepartament.DepositeRate, EntityDepartament.LoanRate));
                    break;
            }
        }

        /// <summary>
        /// Метод удаления выбранного профиля из выбранного отдела
        /// </summary>
        public void RemoveProfile()
        {
            switch (selectedDepartament.Header as string)
            {
                case "Regular Clients":
                    if (!(SelectedRegularProfile is null))
                        RegularDepartament.RemoveProfile(SelectedRegularProfile);
                    break;
                case "VIP Clients":
                    if (!(SelectedVIPClient is null))
                        VIPDepartament.RemoveProfile(SelectedVIPClient);
                    break;
                case "Entity Clients":
                    if (!(SelectedEntityClient is null))
                        EntityDepartament.RemoveProfile(SelectedEntityClient);
                    break;
            }
        }

        /// <summary>
        /// Метод открытия окна редактирования выбранного профиля
        /// </summary>
        public void EditProfile()
        {
            switch (selectedDepartament.Header as string)
            {
                case "Regular Clients":
                    if (SelectedRegularProfile is null)
                        return;
                    ClientProfileWindow rw = new ClientProfileWindow();
                    rw.DataContext = new ClientProfileVM<RegularUser>(SelectedRegularProfile);
                    rw.ShowDialog();
                    break;
                case "VIP Clients":
                    if (SelectedVIPClient is null)
                        return;
                    ClientProfileWindow vw = new ClientProfileWindow();
                    vw.DataContext = new ClientProfileVM<VIPClient>(SelectedVIPClient);
                    vw.ShowDialog();
                    break;
                case "Entity Clients":
                    if (SelectedEntityClient is null)
                        return;
                    ClientProfileWindow ew = new ClientProfileWindow();
                    ew.DataContext = new ClientProfileVM<EntityClient>(SelectedEntityClient);
                    ew.ShowDialog();
                    break;
            }
        }

        /// <summary>
        /// Метод перехода банковской системы на следующий месяц.
        /// </summary>
        public void NextMonth()
        {
            bankSystem.GoNextMonth();
            MessageBox.Show("One month has passed. Do not forget to pay the loan and fill the deposit.");
        }
        #endregion

        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
