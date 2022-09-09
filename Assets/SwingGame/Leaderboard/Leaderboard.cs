using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class Leaderboard
{
    [Serializable]
    public struct ScorePlayer
    {
        [SerializeField] public string Name;
        [SerializeField] public ulong Score;
        [SerializeField] public string Horodatage;

        public ScorePlayer(string name, ulong score, string horodatage)
        {
            this.Name = name;
            this.Score = score;
            this.Horodatage = horodatage;
        }
    }
    [Serializable]
    public struct ListScorePlayer
    {
        [SerializeField] public ScorePlayer[] data;
    }

    private static string LEADERBOARD_URL = "http://www.flareden.fr:8000/";
    private static Leaderboard _leaderboard = null;

    private ILeaderboardChange callback;

    public static Leaderboard GetInstance()
    {
        if (_leaderboard == null)
        {
            _leaderboard = new Leaderboard();
        }
        return _leaderboard;
    }

    public async void SendScore(ScorePlayer scorePlayer)
    {
        Dictionary<string, string> values = new Dictionary<string, string>{
            { "Name", scorePlayer.Name },
            { "Score", scorePlayer.Score.ToString() },
            { "Horodatage", scorePlayer.Horodatage}
        };
        FormUrlEncodedContent content = new FormUrlEncodedContent(values);

        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.PostAsync(LEADERBOARD_URL + "AddScore/", content);
        string strResponse = await response.Content.ReadAsStringAsync();
        httpClient.Dispose();

        ListScorePlayer res = JsonUtility.FromJson<ListScorePlayer>(strResponse);
        callback.OnTop10Receive(res.data);
    }

    public async void FetchAroundScore(ScorePlayer scorePlayer)
    {
        Dictionary<string, string> values = new Dictionary<string, string>{
            { "Name", scorePlayer.Name },
            { "Score", scorePlayer.Score.ToString() },
            { "Horodatage", scorePlayer.Horodatage}
        };
        FormUrlEncodedContent content = new FormUrlEncodedContent(values);

        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.PostAsync(LEADERBOARD_URL + "GetLeaderboardAround/", content);
        string strResponse = await response.Content.ReadAsStringAsync();
        httpClient.Dispose();

        List<ScorePlayer> res = JsonUtility.FromJson<List<ScorePlayer>>(strResponse);
    }


    public async void FetchTop10()
    {
        HttpClient httpClient = new HttpClient();
        string strResponse = await httpClient.GetStringAsync(LEADERBOARD_URL + "Top10/");
        httpClient.Dispose();
        ListScorePlayer res = JsonUtility.FromJson<ListScorePlayer>(strResponse);

        callback.OnTop10Receive(res.data);
    }

    public void SetListener(ILeaderboardChange leaderboardChange)
    {
        this.callback = leaderboardChange;
    }

    public static void GenerateGameobjectTab(GameObject canvasAnchor, ScorePlayer[] list)
    {
        if (canvasAnchor.transform.childCount > 0)
        {
            GameObject.Destroy(canvasAnchor.transform.GetChild(0).gameObject);
        }
        GameObject container = GameObject.Instantiate(Resources.Load("Prefabs/Leaderboard/ListScore", typeof(GameObject))) as GameObject;
        container.transform.SetParent(canvasAnchor.transform, false);
        container.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        //container.GetComponent<RectTransform>().sizeDelta = Vector2.zero;


        for (int i = 0; i < list.Length; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/Leaderboard/PlayerScore", typeof(GameObject))) as GameObject;
            obj.transform.SetPositionAndRotation(new Vector3(0, -50 * i, 0), Quaternion.identity);
            obj.transform.SetParent(container.transform.Find("Body").transform, false);
            obj.transform.Find("rank_value").GetComponent<TMPro.TextMeshProUGUI>().text = (i + 1).ToString();
            obj.transform.Find("name_value").GetComponent<TMPro.TextMeshProUGUI>().text = list[i].Name;
            obj.transform.Find("score_value").GetComponent<TMPro.TextMeshProUGUI>().text = list[i].Score.ToString();

        }
        canvasAnchor.GetComponent<RectTransform>().sizeDelta = new Vector2(650, 50 * (list.Length + 1));
    }

}
