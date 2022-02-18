using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordGameDict
{
    // In C# using a HashSet is an O(1) operation. It's a dictionary without the keys!
    private HashSet<string> words = new HashSet<string>();
    private List<string> wordsArray = new List<string>();

    private TextAsset targetText;

    private TextAsset referenceText;
    public WordGameDict()
    {
        InitTargetDic("ospd");
        InitReferenceDic("refText");
    }

    public WordGameDict(string filename)
    {
        InitTargetDic(filename);
        InitReferenceDic(filename);
    }

    protected void InitTargetDic(string filename)
    {
        targetText = (TextAsset)Resources.Load(filename, typeof(TextAsset));
        var text = targetText.text;

        foreach (string s in text.Split('\n'))
        {
            wordsArray.Add(s);
            // words.Add(s);
        }
    }
    protected void InitReferenceDic(string filename)
    {
        referenceText = (TextAsset)Resources.Load(filename, typeof(TextAsset));
        var text = referenceText.text;

        foreach (string s in text.Split('\n'))
        {
            // wordsArray.Add(s);
            words.Add(s);
        }
    }

    public bool CheckWord(string word, int minLength)
    {
        if (word.Length < minLength)
        {
            return false;
        }
        
        return (words.Contains(word));
    }

    public string GetRandomWord(int length)
    {
        int limit = 100;
        for(int i=0;i<limit; i++)
        {
            int random = Random.Range(0, wordsArray.Count);
            string word = wordsArray[random];

            if(word.Length == length)
                return word.ToUpper();
        }
        return null;
    }
}