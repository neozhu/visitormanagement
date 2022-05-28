using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Application.Services.MessageService;
public class SMSMessageService
{
    private const string host = "https://dfsns.market.alicloudapi.com";
    private const string path = "/data/send_sms";
    private const string method = "POST";
    private const string appcode = "e37a9c2841974dbbad7e517f1d36940e";
    private readonly ILogger<SMSMessageService> _logger;

    public  SMSMessageService(ILogger<SMSMessageService> logger)
    {
        _logger = logger;
    }
    public async Task Send(string to, string[] args,string templ= "TPL_0000")
    {
        try
        {
            var url = host + path;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "APPCODE " + appcode);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>("content", string.Join(',', args)));
                nvc.Add(new KeyValuePair<string, string>("phone_number", to));
                nvc.Add(new KeyValuePair<string, string>("template_id", templ));
                var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };
                var res = await client.SendAsync(req);
                var content = await res.Content.ReadAsStringAsync();
                _logger.LogInformation($"Send to {to}:{string.Join(',', args)}, result:{content}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Send to {to}:{string.Join(',', args)}");
        }

    }

}
