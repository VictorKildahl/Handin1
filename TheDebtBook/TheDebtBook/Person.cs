﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace TheDebtBook.ViewModels
{
    public class Person : BindableBase
    {
        private string name;
        private string debt;

        public Person()
        {

        }

        public Person(string aName, string aDebt)
        {
            name = aName;
            debt = aDebt;
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

        public string Debt
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

    }
}



   
