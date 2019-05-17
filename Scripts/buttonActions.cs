using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonActions : MonoBehaviour
{
    private bool instructionOn = false;
    private bool musicOn = true;
    public Button startButton;

    void Start()
    {
        Button startBtn = startButton.GetComponent<Button>();
        startBtn.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        switch (startButton.name)
        {
            case "Start":
                Debug.Log("start pressed");

                //start game, change scene to game scene
                break;

            case "Rules":
                Debug.Log("rules pressed");

                Image instructionP = GameObject.Find("Instruction").GetComponent<Image>();
                Image instructionI = GameObject.Find("InstructionImage").GetComponent<Image>();

                if (instructionOn)  // turn off
                {
                    instructionP.color = UnityEngine.Color.clear;
                    instructionI.color = UnityEngine.Color.clear;

                    instructionOn = !instructionOn;
                }
                else
                {   // turn on
                    Color32 newCol1 = new Color32(166, 63, 63, 160);
                    Color32 newCol2 = new Color32(240, 250, 235, 255);

                    instructionP.color = newCol1;
                    instructionI.color = newCol2;
                    
                    instructionOn = !instructionOn;
                }

                break;
            case "Music":
                Debug.Log("music pressed");

                if (musicOn)
                {
                    // turn off music
                }else
                {
                    // turn on music
                }

                break;
            case "Exit":
                Debug.Log("quit pressed");
                Application.Quit();
                break;
        }
    }
}












