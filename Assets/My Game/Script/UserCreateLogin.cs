using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UserCreateLogin : MonoBehaviour {

	EventSystem system;
	public static string playerUsername; //player's username
	private string playerPsw; //player's password

	public string username;
	public string password;

	public InputField usernameRInput;
	public InputField passwordRInput;

	public InputField loginName;
	public InputField loginPassword;

	public GameObject login;
	public GameObject create;
	public GameObject mainMenu;

	string CreateUserURL = "http://games.csd.auth.gr/MagicKitchen/Scripts/insertuser.php";
	string LoginURL = "http://games.csd.auth.gr/MagicKitchen/Scripts/check.php";
	string LogoutURL = "http://games.csd.auth.gr/MagicKitchen/Scripts/disconnect.php";

	public string[] userInfo; //array of user account info

	// Use this for initialization
	void Start () 
	{
		system = EventSystem.current;
		LoadTheSaved();
	}
	
	// Update is called once per frame
	void Update () {
		//lets you use tab to navigate between sellectable objects
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

			if (next != null)
			{
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null)
					inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
		}
	}

	public void LoadTheSaved()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            loginName.text = PlayerPrefs.GetString("Username");
            loginPassword.text = PlayerPrefs.GetString("Password");
			LoginToDB();
        }

    }

	//function that starts the login coroutine
	public void LoginToDB() 
	{
		StartCoroutine (LoginUser(loginName.text, loginPassword.text));
	}

	//function that checks user info with db and logs you in
	public IEnumerator LoginUser(string logname, string logpass) 
	{
		if (logname != "" && logpass != "")
        {
			//Debug.Log("The username is:" + logname);
			//login form
			List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
			wwwForm.Add(new MultipartFormDataSection("usernamePost", logname));
			wwwForm.Add(new MultipartFormDataSection("passwordPost", logpass));

			UnityWebRequest www = UnityWebRequest.Post(LoginURL, wwwForm);
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.LogError(www.error);
			}

			yield return www.SendWebRequest();
			string key = www.downloadHandler.text;

			if (!key.Contains("Success"))
            {
				Debug.Log(www.downloadHandler.text);
			}

			//Debug.Log("The key is" + key);
			if (key.Contains("Success"))
			{
				fetchUserData(); //get data from db and load them in game variables
				//Save player's info
				SaveInfo(logname, logpass);
			}
        }    
	}

	public void fetchUserData() 
	{
		string link = "http://games.csd.auth.gr/MagicKitchen/Scripts/getUser.php"; //url to get user data
		StartCoroutine (getUserData (link));

	}

	//function that gets the user data from db
	public IEnumerator getUserData(string link)
	{
		UnityWebRequest www = UnityWebRequest.Get(link);

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.LogError(www.error);
		}

		yield return www.SendWebRequest();

		string userDataString = www.downloadHandler.text;
		userInfo = userDataString.Split(new[] { ".|." }, System.StringSplitOptions.None);
		login.SetActive(false);
		mainMenu.SetActive(true);
	}

	//Save Player's username and psw 
    public void SaveInfo(string saveUsername, string savePsw)
    {
        PlayerPrefs.SetString("Username", saveUsername);
        PlayerPrefs.SetString("Password", savePsw);
       // Debug.Log("Save: The saved username is: " + PlayerPrefs.GetString("Username"));
    }

	//function that fetches the data and creates a user
    public void CreateUserForm() {
		username = usernameRInput.text;
		password = passwordRInput.text;

		StartCoroutine (UsertoDB(username, password));
	}

	//function that links with php files to send queries to db
	private IEnumerator UsertoDB(string username, string password) 
	{

		List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
		//if none inputfield is empty
		if (username != "" &&  password != "" ) {
				
			wwwForm.Add(new MultipartFormDataSection("usernamePost", username));
			wwwForm.Add(new MultipartFormDataSection("passwordPost", password));
					
			UnityWebRequest www = UnityWebRequest.Post(CreateUserURL, wwwForm);

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.LogError(www.error);
			}

			yield return www.SendWebRequest();

			string cmsg = www.downloadHandler.text; //update text message
			//Debug.Log("The update text is: " + www.downloadHandler.text);

			if (cmsg == "User created!") { 
				//clear all fields and text message
				clear (usernameRInput);
				clear (passwordRInput);

				//go to login
				login.SetActive (true);
				create.SetActive (false);
			} 
			else 
			{
				//stay in creation panel
				create.SetActive (true);
			}
		}
	}

	//function that clears the given input field
	public void clear(InputField inputfield) 
	{
		inputfield.Select();
		inputfield.text = "";
	}

	//function that starts the login coroutine
	public void Logout() {
		StartCoroutine (LogoutUser());
		// login.SetActive(true);
	}

	//function that checks user info with db and logs you in
	public IEnumerator LogoutUser() {
		UnityWebRequest www = UnityWebRequest.Get(LogoutURL);

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.LogError(www.error);
		}

		yield return www.SendWebRequest();

		string key  = www.downloadHandler.text; // get the first 13 characters
		if (key == "Destroyed") {
			// clear (loginName);
			// clear (loginPassword);

			//Delete user's data
			DeleteTheSaved();

			//go to login
			//SceneManager.LoadScene("Menu");
			mainMenu.SetActive(false);
			create.SetActive (false);
			login.SetActive (true);
		}
	}

	//Delete players credentials
    public void DeleteTheSaved()
    {
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
    }


}

