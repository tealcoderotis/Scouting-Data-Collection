using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static APIData;

public static class APICaller
{
    private const string BASE_URL = "https://www.thebluealliance.com/api/v3";
    private const string AUTH_KEY_NAME = "X-TBA-Auth-Key";
    private static string APIKey = "RPSLHyQYK30GsvlJ17W8LLBVtPwQCkxgsjSyLgdVLmQTbyPNVmAG3zcrUNhv7ND0";
    public static void SetAPIKey(string APIKey) => APICaller.APIKey = APIKey;


    public static IEnumerator<T> GetInformation<T>(string link)
    {
        Task<T> request = AsyncGetInformation<T>(link);
        while (!request.IsCompleted) yield return default;
        yield return request.Result;
    }
    public static async Task<T> AsyncGetInformation<T>(string link)
    {
        return JsonUtility.FromJson<T>(await AsyncRequestFromTBA(link, typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(WrapperClass<>)));
    }
    public static async Task<string> AsyncRequestFromTBA(string link, bool wrapString = false)
    {
        using UnityWebRequest request = UnityWebRequest.Get($"{BASE_URL}{link}");
        request.SetRequestHeader(AUTH_KEY_NAME, APIKey);
        request.SendWebRequest();
        while (!request.isDone) await Task.Delay(50);

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
            return null;
        }
        else if (wrapString) return WrapJsonString(request.downloadHandler.text);
        else return request.downloadHandler.text;
    }

    private static string WrapJsonString(string json) => $"{{\"Data\": {json}}}";
}
