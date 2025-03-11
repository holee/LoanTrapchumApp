namespace Applications.Responses.ViewModels.Customers;

public sealed class CustomerLoanRequest(string firstName, string lastName,
    string idCard, DateTime birthDate, string phone, 
    string address, int circle)
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string IdCard { get; } = idCard;
    public DateTime BirthDate { get; } = birthDate;
    public string Phone { get; } = phone;
    public string Address { get; } = address;
    public int Circle { get; } = circle;
}
