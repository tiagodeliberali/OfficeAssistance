namespace OfficeAssistance.Core;

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public User(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public User()
    {
    }

    public override string ToString()
    {
        string data = string.Empty;

        if (!string.IsNullOrEmpty(Name))
        {
            data += $"The user name is {Name};";
        }
        if (!string.IsNullOrEmpty(Email))
        {
            data += $"The user email is {Email};";
        }
        if (!string.IsNullOrEmpty(PhoneNumber))
        {
            data += $"The user phone number is {PhoneNumber};";
        }

        return data;
    }

    public string MissingData()
    {
        string missingData = string.Empty;

        if (string.IsNullOrEmpty(Name))
        {
            missingData += "name;";
        }
        if (string.IsNullOrEmpty(Email))
        {
            missingData += "email;";
        }
        if (string.IsNullOrEmpty(PhoneNumber))
        {
            missingData += "phone number;";
        }

        return missingData;
    }
}
