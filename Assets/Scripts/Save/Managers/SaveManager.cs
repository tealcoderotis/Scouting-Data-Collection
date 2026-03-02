using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager
{
    public readonly static string BasePath = Application.persistentDataPath;
    public readonly static string AppSettingsPath = Path.Combine(BasePath, "App_Settings.txt");
    public readonly static string EventInfoPath = Path.Combine(BasePath, "Event_Info.txt");
    public readonly static string UserSettingsPath = Path.Combine(BasePath, "User_Settings");
    public readonly static string ScoutingPresets = Path.Combine(BasePath, "Scouting_Presets");
    public readonly static string SavedMatches = Path.Combine(BasePath, "Saved_Matches");
    public readonly static string SavedMatchCycles = Path.Combine(BasePath, "Saved_Cycles");
    public readonly static string PreMatchSettings = Path.Combine(BasePath, "Pre_Match");
    public static string EventSaveString(string eventKey) => $"EVENT_{eventKey}.csv";


    public static bool ValidateDirectory(string directoryPath)
    {
        bool exists = Directory.Exists(directoryPath);
        if (!exists) Directory.CreateDirectory(directoryPath);
        return exists;
    }

    public static void SaveAppSettings()
    {
        File.WriteAllText(AppSettingsPath, JsonUtility.ToJson(AppSettingsSaveManager.Instance.CurrentAppSettings));
    }
    public static AppSettings GetAppSettings()
    {
        if (File.Exists(AppSettingsPath)) return JsonUtility.FromJson<AppSettings>(File.ReadAllText(AppSettingsPath));
        else return new();
    }

    public static void SaveUserSettings()
    {
        ValidateDirectory(UserSettingsPath);
        File.WriteAllText(Path.Combine(UserSettingsPath, $"TODO: GET CURRENT USER_settings.txt"), "TODO: CURRENT_USER_SETTINGS_DATA");
    }

    [System.Serializable]
    public class PresetData
    {
        public string identifier;
        public string json;

        public PresetData(ScoutingObject scoutingObject)
        {
            identifier = scoutingObject.ObjectTypeIdentifier;
            json = JsonUtility.ToJson(scoutingObject.GetSettings());
        }
    }
    public static string[] GetSavePresets()
    {
        if (!ValidateDirectory(ScoutingPresets)) return new string[] { };
        return Directory.GetFiles(ScoutingPresets);
    }
    public static void SaveScoutingPreset(string presetName, List<PresetData> presetObjects)
    {
        ValidateDirectory(ScoutingPresets);
        File.WriteAllText(Path.Combine(ScoutingPresets, presetName), JsonUtility.ToJson(new ArrayWrapper<PresetData>(presetObjects.ToArray())));
    }
    public static PresetData[] LoadScoutingPreset(string presetName)
    {
        string path = Path.Combine(ScoutingPresets, presetName);
        if (!File.Exists(path)) return new PresetData[] { };
        return JsonUtility.FromJson<ArrayWrapper<PresetData>>(File.ReadAllText(path)).array;
    }
    public static void SavePreMatchSettings(PrematchSaveManager.PrematchSaveData preMatchData)
    {
        ValidateDirectory(PreMatchSettings);
        File.WriteAllText(Path.Combine(PreMatchSettings, "pre_match.json"), JsonUtility.ToJson(preMatchData));
    }

    public static PrematchSaveManager.PrematchSaveData? LoadPreMatchSettings()
    {
        if (File.Exists(Path.Combine(PreMatchSettings, "pre_match.json")))
        {
            return JsonUtility.FromJson<PrematchSaveManager.PrematchSaveData>(File.ReadAllText(Path.Combine(PreMatchSettings, "pre_match.json")));
        }
        else
        {
            return null;
        }
    }

    public static void LoadEventData()
    {
        if (File.Exists(EventInfoPath)) ScoutingCore.EventData = JsonUtility.FromJson<ScoutingCore.CoreEventData>(File.ReadAllText(EventInfoPath));
        //if (ScoutingCore.CurrentEvent.end_date == "" || ScoutingCore.CurrentEvent.EndDate < System.DateTime.Now.Date) ScoutingCore.EventData = new();
    }
    public static void SaveEventData()
    {
        File.WriteAllText(EventInfoPath, JsonUtility.ToJson(ScoutingCore.EventData));
    }


    public static void SaveMatchData(MatchData data)
    {
        ValidateDirectory(SavedMatches);
        string competitionPath = Path.Combine(SavedMatches, EventSaveString(data.EventKey));
        MatchData.ArbritraryData[] content = data.GetFullData();
        StringBuilder sb = new();
        if (!File.Exists(competitionPath))
        {
            for (int i = 0; i < content.Length; i++)
                sb.Append($"{content[i].name}{(i < content.Length - 1 ? ',' : "")}");
            sb.Append('\n');
            for (int i = 0; i < content.Length; i++)
                sb.Append($"{MatchData.ParseTypeForCSV(content[i].type.Name)}{(i < content.Length - 1 ? ',' : "")}");
        }
        else sb.Append(File.ReadAllText(competitionPath));
        sb.Append('\n');
        for (int i = 0; i < content.Length; i++)
            sb.Append($"\"{content[i].value}\"{(i < content.Length - 1 ? ',' : "")}");
        File.WriteAllText(competitionPath, sb.ToString());
    }

    public static void SaveCycleData(MatchData data)
    {
        ValidateDirectory(SavedMatchCycles);
        string competitionPath = Path.Combine(SavedMatchCycles, EventSaveString(data.EventKey));
        MatchData.ArbritraryData[] content = data.GetFullData();
        StringBuilder sb = new();
        if (!File.Exists(competitionPath))
        {
            for (int i = 0; i < content.Length; i++)
                sb.Append($"{content[i].name}{(i < content.Length - 1 ? ',' : "")}");
            sb.Append('\n');
            for (int i = 0; i < content.Length; i++)
                sb.Append($"{MatchData.ParseTypeForCSV(content[i].type.Name)}{(i < content.Length - 1 ? ',' : "")}");
        }
        else sb.Append(File.ReadAllText(competitionPath));
        sb.Append('\n');
        for (int i = 0; i < content.Length; i++)
            sb.Append($"\"{content[i].value}\"{(i < content.Length - 1 ? ',' : "")}");
        File.WriteAllText(competitionPath, sb.ToString());
    }



    [System.Serializable]
    public class ArrayWrapper<ListType>
    {
        public ListType[] array;
        public ArrayWrapper(ListType[] array)
        {
            this.array = array;
        }
    }
}