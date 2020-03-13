using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using TheDebtBook.ViewModels;
using TheDebtBook.Views;

namespace TheDebtBook.ViewModels
{
    public class PersonViewModel : BindableBase
    {
        private Person person;

        public Person Person
        {
            get { return person; }
            set
            {
                SetProperty(ref person, value);
            }
        }

        public PersonViewModel(Person p)
        {
            person = p;

        }

        ICommand _AddToDebtCommand;
        public ICommand AddToDebtCommand
        {
            get
            {
                return _AddToDebtCommand ?? (_AddToDebtCommand = new DelegateCommand(() =>
                {
                    Person.Add();
                }));
            }
        }
    }
}
