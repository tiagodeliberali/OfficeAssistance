var assistance = new Assistance(new ConsoleLogger(false), true);

Console.WriteLine("Olá! Eu sou a Cecília, a assistente virtual da dra. Nicoly. Como posso te ajudar? \n");

var input = Console.ReadLine();

while (input != "sair")
{
    var result = await assistance.Chat(input);
    Console.WriteLine($"\n{result}\n");
    input = Console.ReadLine();
}