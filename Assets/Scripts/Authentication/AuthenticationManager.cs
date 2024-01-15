using RestSharp;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public static class AuthenticationManager 
{
    private static string accessToken;
    private static int expiresIn;
    public static string GetAccessToekn() => accessToken;
    public static int GetExpiresIn() => expiresIn;

    public static async Task RequestAccessToken(Pilot pilot, Action<string> onSuccess, Action<string> onFailure)
    {
        string domain = ".varcities.eu";
        string prefix = "https://idm.";
        string pilotName = pilot.ToString().Replace("_", "-");
        string url = $"{prefix}{pilotName}{domain}";

        if (pilot == Pilot.gzira)
        {
            domain = "213.165.170.189:3443";
            prefix = "https://";
            url = $"{prefix}{domain}";
        }

        string configKey = pilotName.ToUpper().Replace("-", "_") + "_QUANTUMLEAP_OAUTH2";
        string configValue = EnvFileManager.Instance.envVariables[configKey];
        RestClient client = new RestClient(url);
        client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;


        RestRequest request = new RestRequest("/oauth2/token", Method.POST);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", "Basic " + configValue);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("grant_type", "client_credentials");

        IRestResponse response = await client.ExecuteAsync(request);

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            onFailure?.Invoke("Error in authentication process with server.");
            return;
        }

        //Debug.Log(response.Content);
        JObject data = JObject.Parse(response.Content);
        accessToken = data["access_token"].ToString();
        expiresIn = (int)data["expires_in"];

        onSuccess?.Invoke(accessToken);
    }
}

[Serializable]
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