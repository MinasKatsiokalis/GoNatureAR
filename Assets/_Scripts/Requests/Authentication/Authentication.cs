using RestSharp;

namespace GoNatureAR.Requests
{
    public class OAuth2Request : Request
    {
        public OAuth2Request(Pilot pilot) : base(PrepareUrl(pilot), Method.POST)
        {
            AddHeaders(pilot);
        }

        private void AddHeaders(Pilot pilot)
        {
            string pilotName = pilot.ToString().Replace("_", "-");
            string configKey = pilotName.ToUpper().Replace("-", "_") + "_QUANTUMLEAP_OAUTH2";
            string configValue = EnvFileManager.Instance.envVariables[configKey];

            RestRequest.AddHeader("Accept", "application/json");
            RestRequest.AddHeader("Authorization", "Basic " + configValue);
            RestRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            RestRequest.AddParameter("grant_type", "client_credentials");
        }

        private static string PrepareUrl(Pilot pilot)
        {
            string domain = ".varcities.eu";
            string prefix = "https://idm.";
            string pilotName = pilot.ToString().Replace("_", "-");
            string oauth2 = "/oauth2/token";
            string url = $"{prefix}{pilotName}{domain}{oauth2}";

            if (pilot == Pilot.gzira)
            {
                domain = "213.165.170.189:3443";
                prefix = "https://";
                url = $"{prefix}{domain}{oauth2}";
            }
            return url;
        }
    }
}