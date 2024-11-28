using Homework_13;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class CardsManager
    {
        public List<CreditCard> cards = new List<CreditCard>();
        public void AddCard(string holder)
        {
            CreditCard newCard = new CreditCard(holder);
            newCard.PrintCardDetails();
            cards.Add(newCard);
        }
    }


}
