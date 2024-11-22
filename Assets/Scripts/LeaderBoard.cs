using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    private string publicLeaderBKey = "dfdc1e9e1a92ac921e6dcb2b824a7963920b819321061a4fc7a562de24b6da10";

    [SerializeField] 
    private List<TextMeshProUGUI> names;

    [SerializeField] 
    private List<TextMeshProUGUI> scores;

    public void GetLeaderboard(){
        LeaderboardCreator.GetLeaderboard(publicLeaderBKey, ((msg) =>
        {
            for(int i = 0; i < names.Count; i++){
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetBoard(string username, int score){
        LeaderboardCreator.UploadNewEntry(publicLeaderBKey, username, score, ((msg) => 
        {
            GetLeaderboard();
        }));
    }
}
