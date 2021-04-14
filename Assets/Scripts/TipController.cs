using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

class TipController:MonoBehaviour
{
    [SerializeField]
    Text TipText;

    static string[] BuildTips =
{
        "A good track has fast straights as well as challenging curves",
        "Keep your curves a constant radius. A curve that tightens is hard to drive round",
        "Keep your cones evenly spaced. If you make a long straight, the curve at the end will distort",
        "If you move a cone very slightly, the track section redraws itself and it costs nothing",
        "Use the dirt road if you want to want to raise the terrain. Use the air road if you want to make a jump",
        "To make a jump, use the air road. Make sure the landing section slopes downward because the car will tip forward",
        "If you accidentally pile dirt on top of your track you can move one of the cones and it clears the road",
        "If your track build goes horribly wrong, load a different track without saving your changes",
    };
    static string[] DriveTips =
    {
        "Road Hog - when you hit a car and overtake it for 3 seconds \n Road Rage - when you hit a car and force it off the track",
        "All your tracks are stored as .rip files in 'Device/Android/data/com.TheMeanFiddler.RoadRipper/Files'. You can share them with friends, back them up, rename them",
        "You can save a replay and watch it later. It's stored as an .rcd file in 'Device/Android/data/com.TheMeanFiddler.RoadRipper/Files'.",
        "Have fun watching your replays. Find your favourite moments and watch them again and again from different camera angles. Snip and save them as new replays.",
        "Steer out of a roll or you might end up on your roof"
    };
    static string[] FWDTips =
    {
        "If your front wheel drive cars understeers, lift off the gas or apply the brake to shift the weight onto the front wheels",
        "Front wheel drive - always understeers when you power round a corner. Use the brake to help you steer.",
        "Front wheel drive - to drift round corners you need to 'pendulum turn' - also called a 'Scandinavian flick'. It takes a lot of practice."
    };
    static string[] RWDTips =
    {
        "If your rear wheel drive car oversteers, keep on the gas and apply a bit of reverse steering lock",
        "Rear wheel drive - when approaching a corner, flick the wheel to start drifting and then straighten up. Keep your foot on the gas.",
    };

    void Start()
    {
        if (PlayerManager.Type == "BuilderPlayer")
            TipText.text = BuildTips[Random.Range(0, BuildTips.Length)];
        if (PlayerManager.Type == "CarPlayer")
        {
            if (Random.value > 0.5f)
                TipText.text = DriveTips[Random.Range(0, DriveTips.Length)];
            else
            {
                if (Main.Instance.Vehicle == "Car")
                    TipText.text = FWDTips[Random.Range(0, FWDTips.Length)];
                else
                    TipText.text = RWDTips[Random.Range(0, RWDTips.Length)];
            }
        }
    }



}

