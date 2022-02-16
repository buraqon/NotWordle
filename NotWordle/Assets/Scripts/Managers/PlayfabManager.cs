using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{

    public static PlayfabManager Instance;  
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of a singleton in the scene");
            Destroy(this);
        }
    }

    public void Login(string ID)
    {
        var request = new LoginWithCustomIDRequest
        {
            // CustomId = SystemInfo.deviceUniqueIdentifier,
            CustomId = ID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucess, OnError);
    }

    void OnSucess(LoginResult result)
    {
        Debug.Log("Successful login account create");

        SaveUserData(GameManager.Instance.GetUserInfo("Name"));
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error creating account");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate 
                {
                    StatisticName = "DialyChallenge",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfuly sent leaderboard result");
    }

    void SaveUserData(string name)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Name", name}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("User name saved");
    }

    public void LoadUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieve, OnError);
    }

    void OnDataRecieve(GetUserDataResult result)
    {
        if(result.Data != null && result.Data.ContainsKey("Name"))
        {
            Debug.Log("User data is complete");
        }
        Debug.Log("User data not complete!");
    }
}
