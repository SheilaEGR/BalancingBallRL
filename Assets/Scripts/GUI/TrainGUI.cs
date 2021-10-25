using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGUI : MonoBehaviour
{
    public LabelValue EpisodeNum;
    public LabelValue Reward;
    public LabelValue KP;
    public LabelValue KI;
    public LabelValue KD;

    private void Start() 
    {
        EpisodeNum.SetLabel("Episode");
        Reward.SetLabel("Reward");
        KP.SetLabel("kp");
        KI.SetLabel("ki");
        KD.SetLabel("kd");
    }

    private void Update() 
    {
        EpisodeNum.SetValue(Globals.episodeNum);
        Reward.SetValue(Globals.episodeReward);
        KP.SetValue(Globals.kp);
        KI.SetValue(Globals.ki);
        KD.SetValue(Globals.kd);
    }
}
