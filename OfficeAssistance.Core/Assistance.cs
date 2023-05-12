using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text.Json;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

using OfficeAssistance.Core;
using OfficeAssistance.Core.Models;
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
            "<key>"                                                     // OpenAI API Key
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

        if (this.debugInfo) Console.WriteLine($"DEBUG: Detected intention: {intention}");

        var result = intention switch
        {
            "ScheduleAppointment" => await this.ScheduleAppointment(input),
            "SupplyMissingData" => await this.ScheduleAppointment(input),
            _ => await this.GeneralInformation(input),
        };

        chatHistory.Add($"bot: {result}");

        return result;
    }

    private async Task<string> GeneralInformation(string input)
    {
        return (await kernel.RunAsync(input, directorySkills["GeneralInformation"])).ToString().Trim();
    }

    private async Task<string> ScheduleAppointment(string input)
    {
        if (!string.IsNullOrEmpty(user.MissingData()))
        {
            var userSuppliedDataString = (await kernel.RunAsync(input, directorySkills["RecognizeUser"])).ToString().Trim();
            if (this.debugInfo) Console.WriteLine($"DEBUG: User retrieved data: {userSuppliedDataString}");

            var json = JsonSerializer.Deserialize<UserSuppliedData>(userSuppliedDataString)!;

            if (string.IsNullOrEmpty(user.Name)) user.Name = json.name;
            if (string.IsNullOrEmpty(user.Email)) user.Email = json.email;
            if (string.IsNullOrEmpty(user.PhoneNumber)) user.PhoneNumber = json.phone;

            if (!string.IsNullOrEmpty(user.MissingData())) return json.requestUserMissingInformation;
        }

        var variables = new ContextVariables(input);
        variables.Set("availableSlots", this.RetrieveSchedule());
        variables.Set("todayDate", DateTime.Today.ToLongDateString());

        var scheduleString = (await kernel.RunAsync(variables, directorySkills["ScheduleAppointment"])).ToString().Trim();
        var scheduleJson = JsonSerializer.Deserialize<ScheduleData>(scheduleString)!;

        if (scheduleJson.userAcceptedDate)
        {
            if (this.debugInfo) Console.WriteLine($"DEBUG: User accepted date: {scheduleJson.userAcceptedDate}");
        }

        return scheduleJson.botResponse;
    }

    private string RetrieveSchedule()
    {
        var availavleDays = new List<DateTime>()
        {
            DateTime.Parse("2023-05-12 10:00"),
            DateTime.Parse("2023-05-12 14:00"),
            DateTime.Parse("2023-05-12 15:00"),
            DateTime.Parse("2023-05-13 10:00"),
            DateTime.Parse("2023-05-13 18:00"),
            DateTime.Parse("2023-05-15 17:00"),
            DateTime.Parse("2023-05-15 19:00"),
            DateTime.Parse("2023-05-15 20:00"),
            DateTime.Parse("2023-05-15 21:00"),
            DateTime.Parse("2023-05-16 10:00"),
            DateTime.Parse("2023-05-16 14:00"),
            DateTime.Parse("2023-05-17 16:00"),
            DateTime.Parse("2023-05-17 17:00"),
            DateTime.Parse("2023-05-17 18:00"),
            DateTime.Parse("2023-05-17 20:00"),
            DateTime.Parse("2023-05-18 10:00"),
            DateTime.Parse("2023-05-19 09:00"),
            DateTime.Parse("2023-05-19 11:00"),
            DateTime.Parse("2023-05-19 15:00"),
        };

        return string.Join("\n", availavleDays.Select(x => $"Available session starting {x.ToLongDateString()} at {x.ToLongTimeString()}"));
    }
}

