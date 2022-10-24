
using System;
using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Doublsb.Dialog;
using UnityEngine;
using UnityEngine.UI;

public class FileBrowserUpdate : MonoBehaviour
{
    public Tester_sc gameManager;
    public String_sc txtReader;
    string[] txt = null;
    string sDialog;
    public Text textPreview;
    public Button btnShowDialog;
    public Button btnLoadTxt;

    private void Start()
    {
        btnShowDialog.onClick.AddListener(ShowEffect);
        btnLoadTxt.onClick.AddListener(OpenFileBrowser);
    }

    void ShowEffect()
    {
        if (txt.Length==1&&txt[0]=="")
        {
            return;
        }
        txtReader.ReadAndTransform(txt);
    }
    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();
        bp.filter = "text files (*.txt) | *.txt";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
            StartCoroutine(CoLoadTxt(path));

        });
    }

    IEnumerator CoLoadTxt(string path)
    {
        sDialog = File.ReadAllText(path);
        textPreview.text = sDialog;
        txt = sDialog.Split('\n');
        yield return null;
    }
}
