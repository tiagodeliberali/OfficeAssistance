using System.Collections.Concurrent;
using System.Reflection;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;

using OfficeAssistance.Core;
using OfficeAssistance.Core.Skills;

public class Assistance
{
    private readonly bool debugInfo;
    private readonly IKernel kernel;
    private readonly IDictionary<string, ISKFunction> directorySkills;
    private readonly IDictionary<string, ISKFunction> nativeSkills;
    private readonly List<string> chatHistory = new();

    private readonly User user = new User();
    string appointmentDate = string.Empty;

    public Assistance(ILogger logger, bool debugInfo)
    {
        this.debugInfo = debugInfo;
        this.kernel = Kernel.Builder.WithLogger(logger).Build();

        // Alternative using OpenAI
        kernel.Config.AddOpenAITextCompletionService(
            "text-davinci-003",                                         // OpenAI Model name
            "<KEY>"       // OpenAI API Key
        );
        // crate a variable with current directory path
        var currentAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        this.directorySkills = kernel.ImportSemanticSkillFromDirectory(Path.Combine(currentAssemblyPath, "Skills"), "AssistanceSkills");
        this.nativeSkills = kernel.ImportSkill(new NativeSkills());

        PrintDebugInfo();
    }

    private void PrintDebugInfo()
    {
        if (!this.debugInfo) return;

        Console.WriteLine("DEBUG: Loaded skills:");
        Console.WriteLine("===\n");

        FunctionsView functions = kernel.Skills.GetFunctionsView();
        ConcurrentDictionary<string, List<FunctionView>> nativeFunctions = functions.NativeFunctions;
        ConcurrentDictionary<string, List<FunctionView>> semanticFunctions = functions.SemanticFunctions;

        foreach (KeyValuePair<string, List<FunctionView>> skill in nativeFunctions)
        {
            Console.WriteLine("Native skill: " + skill.Key);

            foreach (var item in skill.Value)
            {
                Console.WriteLine($" - {item.Name}");
            }
        }

        foreach (KeyValuePair<string, List<FunctionView>> skill in semanticFunctions)
        {
            Console.WriteLine("Semantic skill: " + skill.Key);

            foreach (var item in skill.Value)
            {
                Console.WriteLine($" - {item.Name}");
            }
        }
        Console.WriteLine("===\n");
    }

    public async Task<string> Chat(string input)
    {
        var intention = $"{await kernel.RunAsync(input, directorySkills["DetectionSkill"])}".Trim();

        this.chatHistory.Add($"user: {input}");

        if (this.debugInfo) Console.WriteLine($"DEBUG: Detected intention: {intention}");

        var result = intention switch
        {
            "ScheduleAppointment" => await this.ScheduleAppointment(this.PrepareInput()),
            "SupplyMissingData" => await this.ScheduleAppointment(this.PrepareInput()),
            _ => await this.GeneralInformation(this.PrepareInput()),
        };

        chatHistory.Add($"bot: {result}");

        return result;
    }

    private async Task<string> GeneralInformation(string input)
    {
        return (await kernel.RunAsync(input, directorySkills["GeneralInformation"])).ToString().Trim();
    }

    private string PrepareInput()
    {
        return string.Join("\n", chatHistory);
    }

    private async Task<string> ScheduleAppointment(string input)
    {
        if (!string.IsNullOrEmpty(user.MissingData()))
        {
            var userSuppliedDataString = (await kernel.RunAsync(input, directorySkills["RecognizeUser"])).ToString().Trim();
            var json = System.Text.Json.JsonSerializer.Deserialize<UserSuppliedData>(userSuppliedDataString)!;

            if (string.IsNullOrEmpty(user.Name)) user.Name = json.name;
            if (string.IsNullOrEmpty(user.Email)) user.Email = json.email;
            if (string.IsNullOrEmpty(user.PhoneNumber)) user.PhoneNumber = json.phone;

            if (!string.IsNullOrEmpty(user.MissingData())) return json.requestUserMissingInformation;
        }

        var result = (await kernel.RunAsync(input, directorySkills["ScheduleAppointment"])).ToString().Trim().Split(";");

        appointmentDate = result[0];

        return result[1];
    }
}

