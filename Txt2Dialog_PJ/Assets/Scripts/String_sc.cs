using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class String_sc : MonoBehaviour
{
    public string[] strArrayTemp;
    // public string[] strArrayTranformed;

    public Tester_sc gameManager;
    public TextAsset txt;


    //"內文&指令","角色名"

    //暫定角色名為國棟跟統神

    private void Awake()
    {
        if (txt != null)
        {
            strArrayTemp = txt.text.Split('\n');
        }
    }

    public void ReadAndTransform(string[] txt)
    {
        gameManager.fileUploadingCanvas.SetActive(false);
        
        if (txt == null)
        {
            return;
        }

        if (txt.Length == 1 || txt[0] == "")
        {
            return;
        }


        // var temp = Add2DialogList(TxtTransform(strArrayTemp));
        var temp = Add2DialogList(TxtTransform(txt));

        // print(strArrayTemp[0].TrimEnd().TrimEnd(']')); //讀取文件時每一段換行都留有空白?先消除空白才能消除中括號

        temp.Add(new DialogData("/speed:0//close/", "", gameManager.EnddingDialogue, true));

        ActionAdd(temp, new[] {1, 2}, new UnityAction[] {() => print("添加action1"), () => print("添加action2")});

        ActionAdd(temp, 2, () => print("第二句再多加一個action3"));
        
        ActionAdd(temp,temp.Count,()=>gameManager.fileUploadingCanvas.SetActive(true));

        gameManager.DialogueShow(temp);

        // DialogManager.Show(temp);
        // for (int i = 0; i < strArrayTemp.Length; i++)
        // {
        //     print(strArrayTemp[i]);
        // }
    }

    private void ActionAdd(List<DialogData> temp, int[] iDialogAction, UnityAction[] actions)
    {
        int itemp = -1;

        foreach (int i in iDialogAction)
        {
            if (i > temp.Count)
            {
                print("錯誤，指定的位置超出對話總數");
                return;
            }
        }

        foreach (int i in iDialogAction)
        {
            itemp++;
            temp[i - 1].Callback = actions[itemp];
        }
    }

    private void ActionAdd(List<DialogData> temp, int iDialogAction, UnityAction action)
    {
        if (iDialogAction > temp.Count)
        {
            print("錯誤，指定的位置超出對話總數");
            return;
        }

        temp[iDialogAction - 1].Callback = action;
    }


    string[,] TxtTransform(string[] strArray)
    {
        print("strArray.Length:" + strArray.Length);
        if (strArray.Length == 1 && strArray[0] == "")
        {
            return null;
        }

        #region 先確定有幾個對話框

        int index = -1;

        for (int i = 0; i < strArray.Length; i++)
        {
            if (CheckSpeaker(strArray[i]) == E_Character.Padko.ToString()
                || CheckSpeaker(strArray[i]) == E_Character.Me.ToString())
            {
                print(CheckSpeaker(strArray[i]) + "是統神或國棟");
                index++;
            }
        }

        var temp = index + 1;
        print("共有幾個對話框:" + temp);

        string[,] str = new string[index + 1, 2]; //對話內容,說話者

        #endregion

        index = -1; //回收再利用

        #region 對話跟說話者都分類好了

        for (int i = 0; i < strArray.Length; i++)
        {
            if (CheckSpeaker(strArray[i]) == E_Character.Padko.ToString()
                || CheckSpeaker(strArray[i]) == E_Character.Me.ToString())
            {
                index++;
                str[index, 1] = CheckSpeaker(strArray[i]); //記住說話者
                print($"第{index}個對話的說話者:" + strArray[i]);
            }

            else if (CheckSpeaker(strArray[i]) != E_Character.Padko.ToString()
                     && CheckSpeaker(strArray[i]) != E_Character.Me.ToString())
            {
                print(strArray[i]);
                str[index, 0] += strArray[i]; //一段台詞
                print($"第{index}個對話的台詞:" + strArray[i]);
            }
        }

        SpeakerNameTransform(str);

        #endregion

        return str;
    }

    private string CheckSpeaker(string str)
    {
        string temp = str;
        return temp.Trim('[').TrimEnd().TrimEnd(']');
    }

    private List<DialogData> Add2DialogList(string[,] str)
    {
        if (str.Length == 0)
        {
            return null;
        }

        List<DialogData> dialog = new List<DialogData>();
        for (int i = 0; i < str.GetLength(0); i++)
        {
            // print(str[i, 0] + '\n' + str[i, 1]);
            dialog.Add(Dialogue_Speaker(str[i, 0], str[i, 1]));
        }

        return dialog;
    }

    public void SpeakerNameTransform(string[,] s)
    {
        for (int i = 0; i < s.GetLength(0); i++)
        {
            if (s[i, 1] == E_Character.Me.ToString())
            {
                s[i, 1] = E_Character.Me.ToString();
            }

            if (s[i, 1] == E_Character.Padko.ToString())
            {
                s[i, 1] = E_Character.Padko.ToString();
            }
        }
    }

    public DialogData Dialogue_Speaker(string dialog, string speaker)
    {
        return new DialogData(dialog, speaker);
    }

    public class DialogDataTool
    {
        public string color; // ↓預設格式，每次增加請複製一份↓
        public string size; // 
        public string emote; // 
        public string click; // 
        public string wait; // 
        public string speed; // 
        public string speaker; // 
        public string dialog; // 
        public string newline; // 
        public string close; // 
    }
}

public enum E_Character
{
    Padko,
    Me
}