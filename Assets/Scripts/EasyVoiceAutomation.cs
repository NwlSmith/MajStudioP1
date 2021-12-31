using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyVoiceAutomation : MonoBehaviour
{

    private RoommateManager _roommateManager;
    [SerializeField] private Card[] allCards;
    private StrToAudioClipDictionary _dictionary;

    private Dictionary<string, AudioClip> loadedResourcesDictionary = new Dictionary<string, AudioClip>();

    void Start()
    {
        _roommateManager = FindObjectOfType<RoommateManager>();
        _dictionary = FindObjectOfType<StrToAudioClipDictionary>();

        //GenerateCSV();
        
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Voice/");
        
        Debug.Log("Resources found:");

        foreach (var clip in audioClips)
        {
            Debug.Log("Found " + clip.name);
            loadedResourcesDictionary.Add(clip.name, clip);
        }

        LoadClips();


        // Generate text files

        
        
        
    }
    
    // take roommatemanager, all cards
    // From disk, get all voice lines,
    // associate lines with cards as above,
    // store lines and audio clips in dictionary
    private void LoadClips()
    {

        LoadRoommateMonologue("\"yo dude, I have some weird shit I'm gonna tell ya later.\"", 0);
        
        List<string> roommateMonologue = _roommateManager.GetMonologueQueue();

        for (int i = 0; i < roommateMonologue.Count; i++)
        {
            LoadRoommateMonologue(roommateMonologue[i], i + 1); // + 1 bc the first one is "yo dude, I have some weird shit I'm gonna tell ya later."
        }
        
        foreach (var card in allCards)
        {
            LoadCards(card);
        }
        
    }

    private void LoadRoommateMonologue(string line, int index)
    {
        LoadFile("Roommate Monologue " + index, line);
    }
    
    private void LoadCards(Card card)
    {
        LoadFile(card.name + " info", card.infoText);
        LoadFile(card.name + " d1 roommate", card.d1RoommateResponse);
        LoadFile(card.name + " d2 roommate", card.d2RoommateResponse);
    }

    private void LoadFile(string fileName, string line)
    {
        string filePath = "Voice/" + fileName + ".wav";
        Debug.Log($"trying to correctly associate {fileName}...");

        if (!loadedResourcesDictionary.ContainsKey(fileName))
        {
            Debug.LogWarning("Loaded resources does not contain " + fileName);
            return;
        }
        AudioClip ac = loadedResourcesDictionary[fileName];
        
        _dictionary.AddEntry(line, ac);
    }

    // take roommatemanager, all cards
    // generate CSV for all roommate monologue, all cards, and all card responses
    // make file name from type of text (monologue, card, card response etc), and which one it is in order
    // save that text to file.
    private void GenerateCSV()
    {
        string str = "\"ID\",\"Group\",\"Status\",\"Speaker\",\"Text\",\"File name\"\n";
        
        // Roommate
        
        str += FormatRoommateString("\"yo dude, I have some weird shit I'm gonna tell ya later.\"", 0);

        List<string> roommateMonologue = _roommateManager.GetMonologueQueue();

        for (int i = 0; i < roommateMonologue.Count; i++)
        {
            str += FormatRoommateString(roommateMonologue[i], i + 1); // + 1 bc the first one is "yo dude, I have some weird shit I'm gonna tell ya later."
        }
        
        // cards

        foreach (var card in allCards)
        {
            str += FormatCardStrings(card);
        }

        System.IO.File.WriteAllText(Application.dataPath + "AllDialogue.csv", str);
        Debug.Log($"Successfully wrote to {Application.dataPath + "AllDialogue.csv"}");
    }

    private string FormatRoommateString(string roommateString, int index)
    {
        return ConstructCSV("Microsoft Zira Desktop", roommateString, "Roommate Monologue " + index.ToString());
    }

    private string FormatCardStrings(Card card)
    {
        string str = "";
        str += ConstructCSV("Microsoft David Desktop", card.infoText, card.name + " info");
        str += ConstructCSV("Microsoft Zira Desktop", card.d1RoommateResponse, card.name + " d1 roommate");
        str += ConstructCSV("Microsoft Zira Desktop", card.d2RoommateResponse, card.name + " d2 roommate");
        return str;
    }

    private string ConstructCSV(string voice, string dialogue, string filename)
    {
        return "1,\"\",0,\"" + voice + "\"," + dialogue + ",\"" + filename + "\"\n";
    }
    
}
