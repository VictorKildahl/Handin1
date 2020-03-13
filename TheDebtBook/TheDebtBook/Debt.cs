using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace TheDebtBook.ViewModels
{
    public class Debt : BindableBase
    {
        private int _amount;

        public Debt(int amount)
        {
            Amount = amount;
        }

        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                SetProperty(ref _amount, value);
            }
        }
    }
}
