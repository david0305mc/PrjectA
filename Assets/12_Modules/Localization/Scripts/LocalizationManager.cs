using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Linq;

public class LocalizationManager : MonoBehaviour
{
    protected string Language { get; private set; }
    public static LocalizationManager Instance { get; private set; }

    [Serializable] public class StringStringDictionaly : SerializableDictionary<string, string> { }
    [Serializable] public class LanguageDictionary : SerializableDictionary<string, StringStringDictionary> { }
    //
    //https://docs.google.com/spreadsheets/d/1UWi_gtSYDSi0D_hMT4uLwcY_rQfIx0WmT5DKR0LEHBg/export?format=tsv
    //https://docs.google.com/spreadsheets/d/1LeDydivi55yGxns9u_PP59C7PLwcKqdAiL2EnCQ_Imk/export?format=tsv
    [SerializeField] private string langURL = "https://docs.google.com/spreadsheets/d/1UWi_gtSYDSi0D_hMT4uLwcY_rQfIx0WmT5DKR0LEHBg/export?format=tsv";
    //[SerializeField] private string langURL = "https://docs.google.com/spreadsheets/d/1LeDydivi55yGxns9u_PP59C7PLwcKqdAiL2EnCQ_Imk/export?format=tsv";
    [SerializeField] private LanguageDictionary Langs;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        Language = Application.systemLanguage.ToString();
    }

    public void GetLocalizationText()
    {
        Langs.Clear();
        GetLanguage().Forget();
    }

    private async UniTask GetLanguage()
    {
        UnityWebRequest request = UnityWebRequest.Get(langURL);
        var op = await request.SendWebRequest();
        SetLanguageList(op.downloadHandler.text);
    }
    private void SetLanguageList(string text)
    {
        string[] row = text.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;
        string[,] Sentence = new string[rowSize, columnSize];

        Enumerable.Range(0, rowSize).ToList().ForEach(i =>
        {
            string[] column = row[i].Split('\t');
            Enumerable.Range(0, columnSize).ToList().ForEach(j =>
            {
                Sentence[i, j] = column[j];
            });
        });

        Langs = new LanguageDictionary();
        Enumerable.Range(1, rowSize - 1).ToList().ForEach(i =>
        {
            Langs.Add(Sentence[i, 0].TrimEnd(), new StringStringDictionary());

            Enumerable.Range(0, columnSize).ToList().ForEach(j =>
            {
                Langs[Sentence[i, 0].TrimEnd()].Add(Sentence[0, j].TrimEnd(), Sentence[i, j]);
            });

        });

    }

    public string GetText(string key)
    {
        if (Langs.ContainsKey(key))
        {
            if (Langs[key].ContainsKey(Language))
            {
                return Langs[key][Language];
            }
            else
            {
                if (Langs[key].ContainsKey("English"))
                {
                    return Langs[key]["English"];
                }
            }
        }
        return key;
    }





}
