using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class SaveData : MonoBehaviour
{


    string SaveDataURL = "http://localhost/MagicKitchen/Scripts/saveData.php";

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState == GameManager.gameStates.GameOver)
        {
            LoginToDB();
        }
    }

    public void LoadTheSaved()
    {

    }

    public void LoginToDB()
    {
        StartCoroutine(SaveUserData(PlayerStats.instance.username, PlayerStats.instance.satisfiedClients, PlayerStats.instance.satPercentage, PlayerStats.instance.lifeStamps[0], PlayerStats.instance.lifeStamps[1], PlayerStats.instance.lifeStamps[2], PlayerStats.instance.levelType));
    }

    //function that checks user info with db and logs you in
    public IEnumerator SaveUserData(string username, string satisfiedClients, string satPercentage, string firstLife, string middleLife, string lastLife, string levelType)
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();

        wwwForm.Add(new MultipartFormDataSection("usernamePost", username));
        wwwForm.Add(new MultipartFormDataSection("satisfiedClientsPost", satisfiedClients));
        wwwForm.Add(new MultipartFormDataSection("satPercentPost", satPercentage));
        wwwForm.Add(new MultipartFormDataSection("firstLifePost", firstLife));
        wwwForm.Add(new MultipartFormDataSection("middleLifePost", middleLife));
        wwwForm.Add(new MultipartFormDataSection("lastLifePost", lastLife));
        wwwForm.Add(new MultipartFormDataSection("levelTypePost", levelType));

        UnityWebRequest www = UnityWebRequest.Post(SaveDataURL, wwwForm);

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }

        yield return www.SendWebRequest();

        string cmsg = www.downloadHandler.text; //update text message
        Debug.Log("The update text is: " + www.downloadHandler.text);
        if (cmsg != "Updated.")
        {
            Debug.Log("something went wrong");
        }
    }
}
