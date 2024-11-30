using Bot_De_Telegram;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////  CREO LA "BASE DE DATOS"  /////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Usuario usuario1 = new Usuario("admin1", "contraseñaadmin1");
CuentaPesos cajapesos1 = new CuentaPesos(usuario1, "pesos");
CuentaPesos cajadolares1 = new CuentaPesos(usuario1, "dolares");

Productos productopesos1 = new Productos("tata", "pan bimbo", 1, 80, "$");
Productos productopesos2 = new Productos("revoltijo", "leche", 2, 40, "$");
Productos productopesos3 = new Productos("mercado libre", "llave numero 13", 3, 180, "$");

Productos productodolares1 = new Productos("amazon", "dron", 1, 230, "USD");
Productos productodolares2 = new Productos("sony", "play sation 5", 2, 750, "USD");
Productos productodolares3 = new Productos("steam", "black ops 6", 3, 70, "USD");

cajapesos1.AgregarProducto(productopesos1);
cajapesos1.AgregarProducto(productopesos2);
cajapesos1.AgregarProducto(productopesos3);

cajadolares1.AgregarProducto(productodolares1);
cajadolares1.AgregarProducto(productodolares2);
cajadolares1.AgregarProducto(productodolares3);

Usuario usuario2 = new Usuario("admin2", "contraseñaadmin2");
CuentaPesos cajapesos2 = new CuentaPesos(usuario2, "pesos");
CuentaPesos cajadolares2 = new CuentaPesos(usuario2, "dolares");

Productos productopesos4 = new Productos("banifox", "memoria ram", 1, 4500, "$");
Productos productopesos5 = new Productos("tratoria", "milanesa napolitana", 2, 650, "$");
Productos productopesos6 = new Productos("divino", "almohada", 3, 1500, "$");

Productos productodolares4 = new Productos("temu", "cubiertos", 1, 5, "USD");
Productos productodolares5 = new Productos("riot", "valorant coins", 2, 10, "USD");
Productos productodolares6 = new Productos("duolingo", "membresia anual", 3, 50, "USD");

cajapesos2.AgregarProducto(productopesos4);
cajapesos2.AgregarProducto(productopesos5);
cajapesos2.AgregarProducto(productopesos6);

cajadolares2.AgregarProducto(productodolares4);
cajadolares2.AgregarProducto(productodolares5);
cajadolares2.AgregarProducto(productodolares6);

 Dictionary<long, Usuario> usuariosActivos = new();


// Token del bot
var botClient = new TelegramBotClient("7761937353:AAHNmXU2ont-JH1RzvNyHESUyckcEamve7Y");

// Fecha
int year;
int month;
int day;
int hour;
int minute;
int second;

// Mensajes e informacion del usuario
long chatId = 0;
string messageText;
int messageId;
string firstName;
string lastName;
long id;
Message sentMessage;

////////////////////////////////////////////

// Leer la fecha, hora y guardar variables
year = int.Parse(DateTime.UtcNow.Year.ToString());
month = int.Parse(DateTime.UtcNow.Month.ToString());
day = int.Parse(DateTime.UtcNow.Day.ToString());
hour = int.Parse(DateTime.UtcNow.Hour.ToString());
minute = int.Parse(DateTime.UtcNow.Minute.ToString());
second = int.Parse(DateTime.UtcNow.Second.ToString());
Console.WriteLine("Data: " +year + "/" + month + "/" + day);
Console.WriteLine("Time: " +hour + "/" + minute + "/" + second);

// Token de cancelacion
using var cts = new CancellationTokenSource();

// Bot empieza a recibir
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};
botClient.StartReceiving(
    HandleUpdateAsync,HandleErrorAsync,receiverOptions,cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();


// Mensaje de bienvenida
Console.WriteLine($"\nHola! soy {me.Username} y soy tu bot!");

// Envia una cancelacion, detiene el bot y cierra la consola
Console.ReadKey();
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
        return;

    // Procesar texto del mensaje
    chatId = update.Message.Chat.Id;
    messageText = update.Message.Text.Trim().ToLower(); // Sanitizar entrada
    firstName = update.Message.From.FirstName;
    lastName = update.Message.From.LastName;

    // Comando para iniciar sesión
    if (messageText.StartsWith("/usuario"))
    {
        if (messageText.Contains(usuario1.Nombre.ToLower()) && messageText.Contains(usuario1.Contraseña))
        {
            usuariosActivos[chatId] = usuario1;
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Ingreso correcto como {usuario1.Nombre}. Puede usar el comando /caja para ver sus cuentas.",
                cancellationToken: cancellationToken);
        }
        else if (messageText.Contains(usuario2.Nombre.ToLower()) && messageText.Contains(usuario2.Contraseña))
        {
            usuariosActivos[chatId] = usuario2;
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Ingreso correcto como {usuario2.Nombre}. Puede usar el comando /caja para ver sus cuentas.",
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Usuario o contraseña incorrectos. Por favor, intente nuevamente.",
                cancellationToken: cancellationToken);
        }
        return;
    }

    // Comando para ver las cajas
    if (messageText.StartsWith("/caja"))
    {
        if (!usuariosActivos.ContainsKey(chatId))
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Debe iniciar sesión con /usuario antes de consultar las cajas.",
                cancellationToken: cancellationToken);
            return;
        }

        Usuario usuarioActivo = usuariosActivos[chatId];

        if (messageText.Contains("pesos"))
        {
            string productosPesos = usuarioActivo == usuario1 ? cajapesos1.MostrarProductos() : cajapesos2.MostrarProductos();
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Últimos movimientos en la caja de ahorros en pesos:\n{productosPesos}",
                cancellationToken: cancellationToken);
        }
        else if (messageText.Contains("dolares"))
        {
            string productosDolares = usuarioActivo == usuario1 ? cajadolares1.MostrarProductos() : cajadolares2.MostrarProductos();
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Últimos movimientos en la caja de ahorros en dólares:\n{productosDolares}",
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Debe especificar 'pesos' o 'dolares' después del comando /caja.",
                cancellationToken: cancellationToken);
        }
    }

    // Comando para volver atrás
    if (messageText == "/volver")
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Volvió al menú principal. Puede usar /caja para consultar cuentas o /cambiar_usuario para iniciar sesión con otro usuario.",
            cancellationToken: cancellationToken);
    }

    // Comando para cambiar de usuario
    if (messageText == "/cambiar_usuario")
    {
        if (usuariosActivos.ContainsKey(chatId))
        {
            usuariosActivos.Remove(chatId);
        }

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Ha cerrado sesión. Por favor, use /usuario 'nombre' 'contraseña' para iniciar sesión con otro usuario.",
            cancellationToken: cancellationToken);
    }

    // Mensaje de ayuda
    if (messageText == "help" || messageText == "ayuda")
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"Hola {firstName} {lastName}, utilice el comando:\n/usuario 'nombre de usuario' 'contraseña' para iniciar sesión.\n/caja pesos o dolares para consultar sus cuentas.",
            cancellationToken: cancellationToken);
    }
}


Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}", _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

