using System;

namespace ATMSimulator
{
    public interface IATMState
    {
        void InsertCard(ATM atm);
        void EnterPin(ATM atm, string pin);
        void WithdrawCash(ATM atm, decimal amount);
    }

    public class WaitingForCardState : IATMState
    {
        public void InsertCard(ATM atm)
        {
            atm.SetState(atm.WaitingForPinState);
            Console.WriteLine("Karta włożona. Proszę podać PIN.");
        }

        public void EnterPin(ATM atm, string pin)
        {
            Console.WriteLine("Najpierw włóż kartę.");
        }

        public void WithdrawCash(ATM atm, decimal amount)
        {
            Console.WriteLine("Najpierw włóż kartę i podaj PIN.");
        }
    }

    public class WaitingForPinState : IATMState
    {
        private const string CorrectPin = "1234";

        public void InsertCard(ATM atm)
        {
            Console.WriteLine("Karta już jest włożona.");
        }

        public void EnterPin(ATM atm, string pin)
        {
            if (pin == CorrectPin)
            {
                atm.SetState(atm.ReadyForOperationState);
                Console.WriteLine("PIN poprawny. Możesz dokonać operacji.");
            }
            else
            {
                atm.SetState(atm.WaitingForCardState);
                Console.WriteLine("Niepoprawny PIN. Karta zwrócona.");
            }
        }

        public void WithdrawCash(ATM atm, decimal amount)
        {
            Console.WriteLine("Podaj najpierw poprawny PIN.");
        }
    }

    public class ReadyForOperationState : IATMState
    {
        private decimal _balance = 1000m;

        public void InsertCard(ATM atm)
        {
            Console.WriteLine("Karta już jest w użyciu.");
        }

        public void EnterPin(ATM atm, string pin)
        {
            Console.WriteLine("PIN już został wprowadzony.");
        }

        public void WithdrawCash(ATM atm, decimal amount)
        {
            if (amount <= _balance)
            {
                _balance -= amount;
                Console.WriteLine($"Wypłacono {amount} PLN. Pozostało {_balance} PLN.");
            }
            else
            {
                Console.WriteLine("Brak wystarczających środków.");
            }
            atm.SetState(atm.WaitingForCardState);
            Console.WriteLine("Karta zwrócona. Dziękujemy!");
        }
    }

    public class ATM
    {
        public IATMState WaitingForCardState { get; }
        public IATMState WaitingForPinState { get; }
        public IATMState ReadyForOperationState { get; }

        private IATMState _currentState;

        public ATM()
        {
            WaitingForCardState = new WaitingForCardState();
            WaitingForPinState = new WaitingForPinState();
            ReadyForOperationState = new ReadyForOperationState();
            _currentState = WaitingForCardState;
        }

        public void SetState(IATMState state)
        {
            _currentState = state;
        }

        public void InsertCard()
        {
            _currentState.InsertCard(this);
        }

        public void EnterPin(string pin)
        {
            _currentState.EnterPin(this, pin);
        }

        public void WithdrawCash(decimal amount)
        {
            _currentState.WithdrawCash(this, amount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ATM atm = new ATM();

            atm.InsertCard();
            atm.EnterPin("1234");
            atm.WithdrawCash(100m);
            atm.InsertCard();
            atm.WithdrawCash(200m);
        }
    }
}
