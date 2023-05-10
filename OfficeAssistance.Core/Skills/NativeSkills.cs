namespace OfficeAssistance.Core.Skills;

using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

public class NativeSkills
{
    static Dictionary<string, User> Users = new()
        {
            { "(11) 96067-1234", new User("Tiago Deliberali", "tiago@test.com", "(11) 96067-1234")},
            { "(11) 98409-1234", new User("Glaucilene Santos", "glau@test.com", "(11) 98409-1234")},
        };

    [SKFunction("Get more information about a person talking to the bot")]
    [SKFunctionContextParameter(Name = "phoneNumber", Description = "phone number used by the person talking to the bot")]
    public string GetInfoAboutThePerson(SKContext context)
    {
        return Users[context["phoneNumber"]].ToString();
    }

    [SKFunction("Include a new profile in the list of people the bot talked to")]
    [SKFunctionContextParameter(Name = "name", Description = "Mame you use")]
    [SKFunctionContextParameter(Name = "email", Description = "The email you wanto to use to communicate with the bot")]
    [SKFunctionContextParameter(Name = "phoneNumber", Description = "phone number you use")]
    public void AddNewProfile(SKContext context)
    {
        Users.Add(context["phoneNumber"], new User(context["name"], context["email"], context["phoneNumber"]));
    }
}
