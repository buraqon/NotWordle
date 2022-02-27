using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{

    public static PlayfabManager Instance;  

    [SerializeField] private GameObject rowPrefab;

    [SerializeField] private Transform rowsParent;

    [SerializeField] private GameObject nameWindow;
    
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
            // CustomId = ID,
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successful login account create");

        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;

        if(name == null)
            nameWindow.SetActive(true);
        else
        {
            GameManager.Instance.SetUserInfo("Name", name);
            UIManager.Instance.RefreshLogInState(true);
            UIManager.Instance.SetName(name);
        }
            
        
        // SaveUserData(GameManager.Instance.GetUserInfo("Name"));
    }

    public void SubmitNicknameButton()
    {
        if(nameWindow.GetComponentInChildren<InputField>().text.Length <= 15)
        {
            var request  = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = nameWindow.GetComponentInChildren<InputField>().text
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        }
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated name");
        
        GameManager.Instance.SetUserInfo("Name", result.DisplayName);

        UIManager.Instance.RefreshLogInState(true);
        UIManager.Instance.SetName(name);

        nameWindow.SetActive(false);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error creating account");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboardFive(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate 
                {
                    StatisticName = "DialyChallenge",
                    Value = -score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    public void SendLeaderboardRush(int score)
    {
        if(score == 0)
            return;
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate 
                {
                    StatisticName = "RushChallenge",
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

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "DialyChallenge",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

    public void GetLeaderBoardRush()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "RushChallenge",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGetRush, OnError);
    }

    void OnLeaderBoardGet(GetLeaderboardResult result)
    {
        foreach(Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach(var item in  result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = (-item.StatValue).ToString();
        }
    }
    void OnLeaderBoardGetRush(GetLeaderboardResult result)
    {
        foreach(Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach(var item in  result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = (item.StatValue).ToString();
        }
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
