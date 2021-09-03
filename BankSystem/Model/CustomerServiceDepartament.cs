﻿using BankSystem.Model.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model
{
    class CustomerServiceDepartament<T> : INotifyPropertyChanged
    where T : BankClientProfile
    {
        #region fields

        private ObservableCollection<T> profiles;

        private int depositeRate;

        private int loanRate;
        #endregion

        #region properties

        /// <summary>
        /// Банковские аккаунты отдела
        /// </summary>
        public ObservableCollection<T> Profiles { get { return profiles; } private set { profiles = value; OnPropertyChanged("Profiles"); } }

        /// <summary>
        /// Ставка по вкладу отдела
        /// </summary>
        public int DepositeRate { get { return depositeRate; } set { depositeRate = value; OnPropertyChanged("DepositeRate"); } }

        /// <summary>
        /// Кредитная ставка отдела
        /// </summary>
        public int LoanRate { get { return loanRate; } set { loanRate = value; OnPropertyChanged("LoanRate"); } }

        #endregion

        public CustomerServiceDepartament(int depositeRate, int loanRate)
        {
            this.profiles = new ObservableCollection<T>();
            this.depositeRate = depositeRate;
            this.loanRate = loanRate;
        }

        #region methods

        /// <summary>
        /// Добавление нового профиля
        /// </summary>
        /// <param name="profile">новый профиль</param>
        public void AddProfile(T profile)
        {
            Profiles.Add(profile);
            OnPropertyChanged("Profiles");
        }

        /// <summary>
        /// Удаление профиля
        /// </summary>
        /// <param name="profile">удаляемый профиль</param>
        public void RemoveProfile(T profile)
        {
            Profiles.Remove(profile);
            OnPropertyChanged("Profiles");
        }

        /// <summary>
        /// Переход на следующий месяц
        /// </summary>
        public void GoNextMonth()
        {
            foreach(var i in Profiles)
            {
                i.GoNextMonth();
            }    
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