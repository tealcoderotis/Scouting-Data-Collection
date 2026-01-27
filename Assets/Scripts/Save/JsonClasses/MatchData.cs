using System.Collections.Generic;

public class MatchData
{
    public string deviceIdentifier = UnityEngine.SystemInfo.deviceName;
    public int scoutingTeam = 4450;
    public string scoutingUser = "UNKNOWN_USER";
    public int scoutedTeam = -1;
    public System.DateTime timeSaved = System.DateTime.Now;

    public string EventKey
    {
        get
        {
            if (ScoutingCore.CurrentEvent == null) return "UNKNOWN_COMPETITION";
            else if (ScoutingCore.CurrentEvent.key == "") return "WEIRD_KEY";
            else return ScoutingCore.CurrentEvent.key;
        }
    }

    public ScoutingCore.CompLevels compLevel;
    public int setNumber;
    public int matchNumber;

    public string PrimaryKey => $"{compLevel}_{setNumber}_{matchNumber}_{deviceIdentifier}_{EventKey}_{System.DateTime.Now.Year}";

    public List<ArbritraryData> uniqueData = new();
    public ArbritraryData[] GetFullData()
    {
        ArbritraryData[] data = new ArbritraryData[uniqueData.Count + 9];
        data[0] = new("primary_key", typeof(string), PrimaryKey);
        data[1] = new("team_number", typeof(int), scoutedTeam.ToString());
        data[2] = new("comp_level", typeof(string), matchNumber.ToString());
        data[3] = new("set_number", typeof(int), matchNumber.ToString());
        data[4] = new("match_number", typeof(int), matchNumber.ToString());
        data[5] = new("competition", typeof(string), ScoutingCore.CurrentEvent.key);
        data[6] = new("timestamp", typeof(System.DateTime), timeSaved.ToString("yyyy-MM-dd HH:mm:ss"));
        data[7] = new("scouter_name", typeof(string), scoutingUser);
        data[8] = new("scouting_team", typeof(int), scoutingTeam.ToString());
        for (int i = 0; i < uniqueData.Count; i++)
            data[9 + i] = uniqueData[i];
        return data;
    }

    public static string ParseTypeForCSV(System.Type type) => ParseTypeForCSV(type.Name);
    public static string ParseTypeForCSV(string typeName)
    {
        return typeName switch
        {
            nameof(System.Int32) => "smallint unsigned",
            nameof(System.Boolean) => "tinyint(1)",
            nameof(System.String) => "varchar(45)",
            nameof(System.DateTime) => "timestamp",
            _ => throw new System.Exception($"{typeName} is invalid (has no conversion)")
        };
    }

    
    public struct ArbritraryData
    {
        public string name;
        public System.Type type;
        public string value;

        public ArbritraryData(string name, System.Type type, string value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }
    }
}
