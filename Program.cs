using System;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json ;


class Client
{
    private readonly HttpClient httpClient;
    private readonly string serverUrl;

    public global::System.String ServerUrl => serverUrl;

    public global::System.String ServerUrl1 => serverUrl;

    public Client(string serverUrl) //Client connects to Server
    {
        this.serverUrl = serverUrl;
        this.httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(200)
        };
    }

    //POST request Method
    public async Task SendPostRequestAsync(string endpoint, byte[] payload)
    {
        try
        {
            string url = $"http://localhost:3000/";
            HttpContent content = new ByteArrayContent(payload);
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("POST request was successful. Resource created.");
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                HandleErrorResponse(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    //GET request Method
    public async Task<byte[]> SendGetRequestAsync(string endpoint, int callType, int resendSeq)
    {
        try
        {
            string url = $"http://localhost:3000/";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                HandleErrorResponse(error);
                return null;
            }
            else
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                byte[] jsonDataBytes = Encoding.UTF8.GetBytes(jsonResponse);
                return jsonDataBytes;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        } 
        
    }


    // Method to handle missing packets
    public void HandleMissingPackets()
    {
        // Implement logic to handle missing packets
    }

    // Method to handle error responses
    public void HandleErrorResponse(string error)
    {
        Console.WriteLine(error);
    }
}

public class ResponsePayload
{
    public string? Symbol { get; set; } 
    public char BuySellIndicator { get; set; }
    public int Quantity { get; set; } 
    public int Price { get; set; } 
    public int PacketSequence { get; set; } 
}

public class RequestPayload
{
    public int callType { get; set; }
    public int resendSeq { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Client Application");

        string serverUrl = "http://localhost:3000/";
        Client client = new Client(serverUrl);

        // Call SendGetRequestAsync to perform the GET request
        string endpoint = "http://localhost:3000/"; // Replace with the actual endpoint
        int callType = 1; // Replace with the actual value
        int resendSeq = 0; // Replace with the actual value
        
        byte[] responseJSON = await client.SendGetRequestAsync(endpoint, callType, resendSeq);

        if (responseJSON != null)
        {
           string jsonResponse = Encoding.UTF8.GetString(responseJSON);

            // Deserialize the JSON string into an instance of ResponseData
            ResponsePayload response = JsonConvert.DeserializeObject<ResponsePayload>(jsonResponse);
            Console.WriteLine(response);
        }
        
        
        Console.ReadKey();
    }

}