using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Linq;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    [Header("Menus")]
    public CanvasRenderer baseMenu;
    public CanvasRenderer mainMenu;
    public CanvasRenderer nameMenu;

    [Header("UI")]
    public GridLayoutGroup GameUI;
    public Text messageText;
    public InputField emailInput;
    public InputField pswdInput;
    public InputField nameInput;

    [Header("Walker")]
    public TerrainWalker terrainWalker;

    private User user;
    private Text[] gameText;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        baseMenu.gameObject.SetActive(true);
        nameMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        user = gameObject.AddComponent<User>();
        user.rider = terrainWalker.GetComponentInChildren<Rotator>();
    }

    private void Update() {
        if (user.isWalking){
            gameText[1].text = $"You have walked: {string.Format("{0:0.000}", user.data.miles)} miles";
            //Debug.Log(user.data.miles);
        }
    }

    #region GameLogic
    void InitGameScreen(){
        gameText = GameUI.GetComponentsInChildren<Text>();
        gameText[0].text = $"Hello {user.data.name}";
        StartGame();
        baseMenu.gameObject.SetActive(false); 
        terrainWalker.gameObject.SetActive(true);  
    }

    void StartGame(){
        user.StartWalking();
        StartCoroutine(ServerUpdate());
    }

    IEnumerator ServerUpdate(){
        while (true){
            SaveUserData();
            yield return new WaitForSecondsRealtime(2);
        }
    }

    #endregion

    #region Buttons

    public void RegisterButton(){
        var req = new RegisterPlayFabUserRequest{
            Email = emailInput.text,
            Password = pswdInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(req, OnRegSuccess, OnError);
    }
    public void LoginButton(){
        var req = new LoginWithEmailAddressRequest {
            Email = emailInput.text,
            Password = pswdInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(req, OnLoginSuccess, OnError);
    }

    public void SaveDataButton(){
        instance.user.data.name = nameInput.text;
        PlayfabManager.SaveUserData();
        InitGameScreen();
    }

    #endregion

    #region  Results

    void OnError(PlayFabError error){
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

    void OnRegSuccess(RegisterPlayFabUserResult res){
        messageText.text = "Registered and logged in";
        mainMenu.gameObject.SetActive(false);
        nameMenu.gameObject.SetActive(true);
    }
    void OnLoginSuccess(LoginResult result){
        messageText.text = "Login Success";
        GetUserData();     
    }

    void OnDataSend(UpdateUserDataResult res){
        Debug.Log("User Data update success");
        baseMenu.gameObject.SetActive(false);
    }

    void OnUserDataRecieved(GetUserDataResult res){
        if (res != null && res.Data.ContainsKey("StoredData")){
            // desrialize data into user
            user.data = JsonUtility.FromJson<UserStoredData>(res.Data["StoredData"].Value);
            InitGameScreen();
        }
    }

    #endregion

    #region Requests

    public static void SaveUserData(){
        var req = new UpdateUserDataRequest{
            Data = new Dictionary<string, string>{
                // serialize user data
                {"StoredData", JsonUtility.ToJson(instance.user.data)}
            }
        };
        PlayFabClientAPI.UpdateUserData(req, instance.OnDataSend, instance.OnError);
    }

    public static void GetUserData(){
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), instance.OnUserDataRecieved, instance.OnError);
    }

    #endregion
}
