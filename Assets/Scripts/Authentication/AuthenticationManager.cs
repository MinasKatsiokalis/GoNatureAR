using RestSharp;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class AuthenticationManager 
{
    private static string accessToken;
    private static int expiresIn;
    public static string GetAccessToekn() => accessToken;
    public static int GetExpiresIn() => expiresIn;

    public static async Task<string> GetAccessToken(string pilot)
    {
        string domain = ".varcities.eu";
        string hostIdmUrl = "https://idm." + pilot.Replace("_", "-") + domain;

        string configKey = pilot.ToUpper().Replace("-", "__") + "_QUANTUMLEAP_OAUTH2";
        string configValue = EnvFileManager.Instance.envVariables[configKey];
        RestClient client = new RestClient(hostIdmUrl);
        RestRequest request = new RestRequest("/oauth2/token", Method.POST);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", "Basic " + configValue);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("grant_type", "client_credentials");

        IRestResponse response = await client.ExecuteAsync(request);
        Debug.Log(response.Content);

        JObject data = JObject.Parse(response.Content);
        string accessToken = data["access_token"].ToString();
        string expiresIn = data["expires_in"].ToString();

        return accessToken;
    }

    public static async void GetData(string token, string pilot)
    {
        string url = "https://varcities-api." + pilot.Replace("_", "-") + ".varcities.eu/v2/entities";
        var client = new RestClient(url);
        client.Timeout = -1;
        client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        var request = new RestRequest(Method.GET);
        request.AddHeader("Fiware-Service", "openiot");
        request.AddHeader("X-Auth-Token", token);
        request.AddHeader("Fiware-ServicePath", "/");

        IRestResponse response = await client.ExecuteAsync(request);

        Debug.Log(response.Content);
    }

    public static async void GetSensorData(string token, string pilot, string sensor)
    {
        string url = "https://varcities-api." + pilot.Replace("_", "-") + ".varcities.eu/v2/entities/" + sensor + "?lastN=1";
        var client = new RestClient(url);
        client.Timeout = -1;
        client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        var request = new RestRequest(Method.GET);
        request.AddHeader("Fiware-Service", "openiot");
        request.AddHeader("X-Auth-Token", token);
        request.AddHeader("Fiware-ServicePath", "/");

        IRestResponse response = await client.ExecuteAsync(request);

        Debug.Log(response.Content);
    }
}

public enum Pilot
{
    chania,
    dundalk,
    castelfranco_veneto,
    leuven,
    gzira,
    skelleftea,
    novo_mesto
}
