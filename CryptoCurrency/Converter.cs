using System;
using System.Collections.Generic;

namespace CryptoCurrency
{
    public class Converter : IConverter
    {
        private readonly IDictionary<string, double> _cryptoCurrencyDictionary = new Dictionary<string, double>();

        /// <summary>
        ///     Angiver prisen for en enhed af en kryptovaluta. Prisen angives i dollars.
        ///     Hvis der tidligere er angivet en værdi for samme kryptovaluta,
        ///     bliver den gamle værdi overskrevet af den nye værdi
        /// </summary>
        /// <param name="currencyName">Navnet på den kryptovaluta der angives</param>
        /// <param name="price">Prisen på en enhed af valutaen målt i dollars. Prisen kan ikke være negativ</param>
        public void SetPricePerUnit(string currencyName, double price)
        {
            if (currencyName.Length <= 0)
                throw new ArgumentException(ConverterMessages.UNIT_NAME_MESSAGE);

            if (price <= 0)
                throw new ArgumentException(ConverterMessages.UNIT_PRICE_MESSAGE);

            if (_cryptoCurrencyDictionary.ContainsKey(currencyName))
                _cryptoCurrencyDictionary[currencyName] = price;
            else
                _cryptoCurrencyDictionary.Add(currencyName, price);
        }

        /// <summary>
        ///     Konverterer fra en kryptovaluta til en anden.
        ///     Hvis en af de angivne valutaer ikke findes, kaster funktionen en ArgumentException
        /// </summary>
        /// <param name="fromCurrencyName">Navnet på den valuta, der konverterers fra</param>
        /// <param name="toCurrencyName">Navnet på den valuta, der konverteres til</param>
        /// <param name="amount">Beløbet angivet i valutaen angivet i fromCurrencyName</param>
        /// <returns>Værdien af beløbet i toCurrencyName</returns>
        public double Convert(string fromCurrencyName, string toCurrencyName, double amount)
        {
            var fromCurrencyFound = _cryptoCurrencyDictionary.TryGetValue(fromCurrencyName, out var fromCurrencyPrice);
            var toCurrencyFound = _cryptoCurrencyDictionary.TryGetValue(toCurrencyName, out var toCurrencyPrice);

            if (fromCurrencyName.Length == 0 || !fromCurrencyFound)
                throw new ArgumentException(ConverterMessages.FROM_CURRENCY_NAME_MESSAGE);

            if (toCurrencyName.Length == 0 || !toCurrencyFound)
                throw new ArgumentException(ConverterMessages.TO_CURRENCY_NAME_MESSAGE);

            if (amount <= 0)
                throw new ArgumentException(ConverterMessages.AMOUNT_MESSAGE);

            var value = fromCurrencyPrice / toCurrencyPrice * amount;
            return value;
        }
    }
}