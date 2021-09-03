using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Model.Client;

namespace BankSystem.Model
{
    class BankSystem : INotifyPropertyChanged
    {
        #region static
        static public DateTime CurrentDate { get; private set; }

        static BankSystem()
        {
            CurrentDate = DateTime.Now;
        }

        static public void StaticGoNextMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
        }

        static public BankSystem GenerateSystem(int defaultDepositeRate, int defaultLoanRate, int countProfiles)
        {
            BankSystem bs = new BankSystem(defaultDepositeRate, defaultLoanRate);
            Random rnd = new Random();
            for(int i = 0; i < countProfiles; i++)
            {
                RegularUser rUser = new RegularUser("RegularUser", bs.RegularUserDepartament.DepositeRate, bs.regularUserDepartament.LoanRate);
                rUser.AddBankAccount(50000);
                rUser.AddDeposite(DepositeType.Default, BankSystem.CurrentDate, BankSystem.CurrentDate.AddMonths(12), 10000);
                rUser.AddLoan(30000, BankSystem.CurrentDate);
                bs.AddUser<RegularUser>(rUser);

                VIPClient vUser = new VIPClient("VIPClient", bs.VIPClientDepartament.DepositeRate, bs.VIPClientDepartament.LoanRate);
                vUser.AddBankAccount(500000);
                vUser.AddDeposite(DepositeType.Default, BankSystem.CurrentDate, BankSystem.CurrentDate.AddMonths(12), 100000);
                vUser.AddLoan(300000, BankSystem.CurrentDate);
                bs.AddUser<VIPClient>(vUser);

                EntityClient eUser = new EntityClient("EntityClient", bs.EntityClientDepartament.DepositeRate, bs.EntityClientDepartament.LoanRate);
                eUser.AddBankAccount(5000000);
                eUser.AddDeposite(DepositeType.Default, BankSystem.CurrentDate, BankSystem.CurrentDate.AddMonths(12), 1000000);
                eUser.AddLoan(3000000, BankSystem.CurrentDate);
                bs.AddUser<EntityClient>(eUser);
            }
            return bs;
        }
        #endregion

        #region fields
        private CustomerServiceDepartament<RegularUser> regularUserDepartament;

        private CustomerServiceDepartament<VIPClient> vIPClientDepartament;

        private CustomerServiceDepartament<EntityClient> entityClientDepartament;


        #endregion

        #region properties

        public CustomerServiceDepartament<RegularUser> RegularUserDepartament { get { return regularUserDepartament; } }

        public CustomerServiceDepartament<VIPClient> VIPClientDepartament { get { return vIPClientDepartament; } }

        public CustomerServiceDepartament<EntityClient> EntityClientDepartament { get { return entityClientDepartament; } }

        #endregion

        public BankSystem(int defaultDepositeRate, int defaultLoanRate)
        {
            this.regularUserDepartament = new CustomerServiceDepartament<RegularUser>(defaultDepositeRate, defaultLoanRate);
            this.vIPClientDepartament = new CustomerServiceDepartament<VIPClient>(defaultDepositeRate * 2, defaultLoanRate * 2);
            this.entityClientDepartament = new CustomerServiceDepartament<EntityClient>(defaultDepositeRate * 2, defaultLoanRate * 2);
        }

        #region methods

        /// <summary>
        /// Добавление нового профиля
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user">новый профиль</param>
        public void AddUser<T>(T user)
            where T : BankClientProfile
        {
            if (user is RegularUser)
            {
                regularUserDepartament.AddProfile(user as RegularUser);
                OnPropertyChanged("RegularUserDepartament");
            }
            else if(user is VIPClient)
            {
                vIPClientDepartament.AddProfile(user as VIPClient);
                OnPropertyChanged("VIPClientDepartament");
            }
            else if (user is EntityClient)
            {
                entityClientDepartament.AddProfile(user as EntityClient);
                OnPropertyChanged("EntityClientDepartament");
            }
        }

        /// <summary>
        /// Удаление профиля
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user">удаляемый профиль</param>
        public void RemoveUser<T>(T user)
            where T:BankClientProfile
        {
            if (user is RegularUser)
            {
                regularUserDepartament.RemoveProfile(user as RegularUser);
                OnPropertyChanged("RegularUserDepartament");
            }
            else if (user is VIPClient)
            {
                vIPClientDepartament.RemoveProfile(user as VIPClient);
                OnPropertyChanged("VIPClientDepartament");
            }
            else if (user is EntityClient)
            {
                entityClientDepartament.RemoveProfile(user as EntityClient);
                OnPropertyChanged("EntityClientDepartament");
            }
        }

        /// <summary>
        /// Переход на следующий месяц
        /// </summary>
        public void GoNextMonth()
        {
            BankSystem.StaticGoNextMonth();
            RegularUserDepartament.GoNextMonth();
            VIPClientDepartament.GoNextMonth();
            EntityClientDepartament.GoNextMonth();
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
