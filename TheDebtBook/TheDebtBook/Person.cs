using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace TheDebtBook.ViewModels
{
    public class Person : BindableBase
    {
        private string name;
        private int debt;
        private double sum;
        private ObservableCollection<Debt> _debtentry = new ObservableCollection<Debt>();

        public Person()
        {

        }

        public Person(string aName, Debt intial)
        {
            name = aName;
            //debt = aDebt;
            DebtEntry.Add(intial);
            Debt = intial.Amount;
        }
        public Person Clone()
        {
            return this.MemberwiseClone() as Person;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }

        public int Debt
        {
            get
            {
                return debt;
            }
            set
            {
                SetProperty(ref debt, value);
            }
        }

        public ObservableCollection<Debt> DebtEntry
        {
            get
            {
                return _debtentry;
            }
            set
            {
                SetProperty(ref _debtentry, value);
            }
        }

        public double Sum
        {
            get
            {
                double sum = 0;

                foreach (var debt in DebtEntry)
                {
                    sum += debt.Amount;
                }

                return sum;
            }
        }

        public void Add()
        {
            DebtEntry.Add(new Debt(Debt));
            RaisePropertyChanged("Sum");
        }
    }
}



   

