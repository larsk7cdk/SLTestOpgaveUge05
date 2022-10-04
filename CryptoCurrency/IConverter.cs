namespace CryptoCurrency
{
    public interface IConverter
    {
        void SetPricePerUnit(string currencyName, double price);
        double Convert(string fromCurrencyName, string toCurrencyName, double amount);
    }
}