using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Model.Deposite;

namespace BankSystem.Model.Client
{
    abstract class BankClientProfile : INotifyPropertyChanged
    {
        #region static
        static private List<BankClientProfile> BClientProfiles;

        static private int nowId;
        static private int NextId { get { BankClientProfile.nowId++; return BankClientProfile.nowId; } }

        static public BankClientProfile Find(int id)
        {
            foreach (var i in BClientProfiles)
            {
                if (i.Id == id)
                    return i;
            }
            return null;
        }

        static BankClientProfile()
        {
            BClientProfiles = new List<BankClientProfile>();
            nowId = -1;
        }

        static void AddBClientProfile(BankClientProfile bClientProfile)
        {
            BClientProfiles.Add(bClientProfile);
        }
        #endregion

        #region fields

        private int id;

        private string name;

        private float confidenceCoefficient;

        private ObservableCollection<BankAccount> bankAccounts;

        private ObservableCollection<Deposite.Deposite> deposites;

        private int depositeRate;

        private ObservableCollection<Loan> loans;

        private int loanRate;

        #endregion

        #region properties

        /// <summary>
        /// Индетефикатор банковского профиля
        /// </summary>
        public int Id { get { return id; } }

        /// <summary>
        /// Имя банковского профиля
        /// </summary>
        public string Name { get { return name; } }

        public float ConfidenceCoefficient { get { return confidenceCoefficient; } set { confidenceCoefficient = value; OnPropertyChanged("ConfidenceCoefficient"); } }

        /// <summary>
        /// Банковские счета профиля
        /// </summary>
        public ObservableCollection<BankAccount> BankAccounts { get { return bankAccounts; } private set { bankAccounts = value; OnPropertyChanged("BankAccounts"); } }

        /// <summary>
        /// Депозиты профиля
        /// </summary>
        public ObservableCollection<Deposite.Deposite> Deposites { get { return deposites; } private set { deposites = value; OnPropertyChanged("Deposites"); } }

        /// <summary>
        /// Ставка по депозиту
        /// </summary>
        public int DepositeRate { get { return depositeRate; } private set { depositeRate = value; OnPropertyChanged("DepositeRate"); } }

        /// <summary>
        /// Кредиты профиля
        /// </summary>
        public ObservableCollection<Loan> Loans { get { return loans; } private set { loans = value; OnPropertyChanged("Loans"); } }

        /// <summary>
        /// Ставка по кредиту
        /// </summary>
        public int LoanRate { get { return loanRate; } private set { loanRate = value; OnPropertyChanged("LoanRate"); } }


        #endregion

        public BankClientProfile(string name, int depositeRate, int loanRate)
        {
            Random rnd = new Random();
            confidenceCoefficient = 2 * (float)rnd.NextDouble();
            this.id = NextId;
            this.name = name;
            this.bankAccounts = new ObservableCollection<BankAccount>();
            this.deposites = new ObservableCollection<Deposite.Deposite>();
            this.depositeRate = confidenceCoefficient < 1 ? depositeRate / 2 : depositeRate;
            this.loanRate = confidenceCoefficient < 1 ? loanRate * 2 : loanRate;
            this.loans = new ObservableCollection<Loan>();
            this.deposites = new ObservableCollection<Deposite.Deposite>();

            BankClientProfile.AddBClientProfile(this);

        }

        #region methods

        /// <summary>
        /// Метод добавления нового банковского счёта
        /// </summary>
        /// <param name="currentBalance">баланс банковского счёта</param>
        public void AddBankAccount(int currentBalance = 0)
        {
            BankAccounts.Add(new BankAccount(id, currentBalance));
            OnPropertyChanged("BankAccounts");
        }

        /// <summary>
        /// Метод добавления нового кредита
        /// </summary>
        /// <param name="startLoanAmount">сумма кредита</param>
        /// <param name="startPeriod">Дата взятия кредита</param>
        public void AddLoan(decimal startLoanAmount, DateTime startPeriod)
        {
            Loans.Add(new Loan(startLoanAmount, LoanRate, startPeriod, Id));
            OnPropertyChanged("Loans");
        }

        /// <summary>
        /// Метод добавления вклада
        /// </summary>
        /// <param name="type">Вид вклада</param>
        /// <param name="period">Время, на которое открывается вклад</param>
        /// <param name="startPeriod">Дата открытия вклада</param>
        /// <param name="startBalance">Начальная сумма</param>
        public void AddDeposite(DepositeType type, DateTime startPeriod, DateTime finishPeriod, decimal startBalance)
        {
            switch(type)
            {
                case DepositeType.Default:
                    Deposites.Add(new DefaultDeposite(DepositeRate, Id,startBalance, startPeriod, finishPeriod));
                    break;
                case DepositeType.WithCapitalization:
                    Deposites.Add(new DepositeWithCapitalization(DepositeRate, Id, startBalance, startPeriod, finishPeriod));
                    break;
            }
            OnPropertyChanged("Deposites");
        }

        /// <summary>
        /// Метод удаления Банковского аккаунта
        /// </summary>
        /// <param name="BAccount"></param>
        public void RemoveBankAccount(BankAccount BAccount)
        {
            BankAccounts.Remove(BAccount);
            OnPropertyChanged("BankAccounts");
        }

        /// <summary>
        /// Переход времени в следующий месяц
        /// </summary>
        public void GoNextMonth()
        {
            foreach(var i in Deposites)
            {
                i.GoNextMounth();
            }
            foreach(var i in Loans)
            {
                i.GoNextMonth();
            }
            RemoveUnactiveControls();
        }

        /// <summary>
        /// Метод сбора неактивных частей
        /// </summary>
        public void RemoveUnactiveControls()
        {
            bool isFind;
            do
            {
                isFind = false;
                Loan removeL = null;
                foreach(var i in Loans)
                {
                    if(!i.IsActive)
                    {
                        isFind = true;
                        removeL = i;
                        break;
                    }
                }

                Deposite.Deposite removeD = null;
                foreach (var i in Deposites)
                {
                    if (!i.IsActive)
                    {
                        isFind = true;
                        removeD = i;
                        break;
                    }
                }
                if (isFind)
                {
                    loans.Remove(removeL);
                    deposites.Remove(removeD);
                }

            } while (isFind);

            
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

    enum DepositeType
    {
        Default,
        WithCapitalization
    }
}
