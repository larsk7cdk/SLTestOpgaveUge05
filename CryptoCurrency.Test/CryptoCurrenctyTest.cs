using FluentAssertions;

namespace CryptoCurrency.Test;

public class CryptoCurrenctyTest
{
    [Theory]
    [InlineData("Bitcoin", 20000.5)]
    [InlineData("B", 20000.5)]
    [InlineData("Bitcoin", 0.1)]
    public void Tilfoej_krypto_valuta_med_navn_og_pris(string currencyName, double price)
    {
        IConverter sut = new Converter();

        var exception = Record.Exception(() => sut.SetPricePerUnit(currencyName, price));

        exception.Should().BeNull();
    }

    [Theory]
    [InlineData("", 20000.5, ConverterMessages.UNIT_NAME_MESSAGE)]
    [InlineData("", -0.1, ConverterMessages.UNIT_NAME_MESSAGE)]
    [InlineData("Bitcoin", 0, ConverterMessages.UNIT_PRICE_MESSAGE)]
    [InlineData("Bitcoin", -0.1, ConverterMessages.UNIT_PRICE_MESSAGE)]
    public void Tilfoej_krypto_valuta_med_ugyldig_navn_og_eller_ugyldig_pris(string currencyName, double price, string message)
    {
        IConverter sut = new Converter();

        var exception = Record.Exception(() => sut.SetPricePerUnit(currencyName, price));

        exception.Should().BeOfType(typeof(ArgumentException));
        exception?.Message.Should().Be(message);
    }

    [Theory]
    [InlineData("Bitcoin", "Litecoin", 50, 1000)]
    public void Omregn_krypto_valuta_med_beloeb(string fromCurrencyName, string toCurrencyName, double amount, double expected)
    {
        IConverter sut = new Converter();
        sut.SetPricePerUnit("Bitcoin", 100);
        sut.SetPricePerUnit("Litecoin", 5);

        var actual = sut.Convert(fromCurrencyName, toCurrencyName, amount);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "", 0, ConverterMessages.FROM_CURRENCY_NAME_MESSAGE)]
    [InlineData("", "Litecoin", 10000, ConverterMessages.FROM_CURRENCY_NAME_MESSAGE)]
    [InlineData("Bitcoin", "", 10000, ConverterMessages.TO_CURRENCY_NAME_MESSAGE)]
    [InlineData("Bitcoin", "Litecoin", 0, ConverterMessages.AMOUNT_MESSAGE)]
    public void Omregn_krypto_valuta_med_ugyldige_navn_og_eller_ugyldig_beloeb(
        string fromCurrencyName, string toCurrencyName, double amount, string message)
    {
        IConverter sut = new Converter();
        sut.SetPricePerUnit("Bitcoin", 100);
        sut.SetPricePerUnit("Litecoin", 5);

        var exception = Record.Exception(() => sut.Convert(fromCurrencyName, toCurrencyName, amount));

        exception.Should().BeOfType(typeof(ArgumentException));
        exception?.Message.Should().Be(message);
    }

    [Fact]
    public void Opdater_krypto_valuta_pris_og_omregn()
    {
        IConverter sut = new Converter();
        sut.SetPricePerUnit("Bitcoin", 100);
        sut.SetPricePerUnit("Litecoin", 5);
        
        sut.SetPricePerUnit("Litecoin", 2.5);
        var actual = sut.Convert("Bitcoin", "Litecoin", 50);

        actual.Should().Be(2000);
    }
}