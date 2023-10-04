internal class Program
{
    private static async Task CallBackend(string ipAddress, int durationInMinutes, int healthStartFailInMinutes, int healthFailDurationInMinutes, string backend)
    {
        Console.WriteLine($"{durationInMinutes} minutes total with {healthFailDurationInMinutes} minutes unhealthy after {healthStartFailInMinutes} minutes (Backend {backend})");

        var durationInSeconds = durationInMinutes * 60;
        var healtfailStartInSeconds = healthStartFailInMinutes * 60;
        var healthfailDurationInSeconds = healthFailDurationInMinutes * 60;
        var url = $"http://{ipAddress}/timeout?durationInSeconds={durationInSeconds}&healthfailStartInSeconds={healtfailStartInSeconds}&healthfailDurationInSeconds={healthfailDurationInSeconds}";

        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Invoking {url}");
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(durationInSeconds + 10);
            var response = await client.GetAsync(url);
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Result: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Exception: {ex.Message}");
        }
    }

    private static async Task Main(string[] args)
    {
        var ipAddress = "20.166.137.178";
        var backend = "Linux";
        await CallBackend(ipAddress, 4, 1, 2, backend);
        await CallBackend(ipAddress, 8, 2, 4, backend);
        await CallBackend(ipAddress, 12, 3, 6, backend);
        await CallBackend(ipAddress, 16, 4, 8, backend);

        ipAddress = "20.166.138.22";
        backend = "Windows";
        await CallBackend(ipAddress, 4, 1, 2, backend);
        await CallBackend(ipAddress, 8, 2, 4, backend);
        await CallBackend(ipAddress, 12, 3, 6, backend);
        await CallBackend(ipAddress, 16, 4, 8, backend);
    }
}