namespace StockApp.Application.Common.Exceptions;

public class BusinessException(string message) : Exception(message)
{
}