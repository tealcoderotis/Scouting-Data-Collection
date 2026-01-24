using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ScoutingCore;

public class APIData
{
    [Serializable]
    public class WrapperClass<T>
    {
        public T Data;

        public static implicit operator T(WrapperClass<T> wrapperClass) => wrapperClass.Data;
    }

    [Serializable]
    public class SimpleEvent
    {
        public string key;
        public string name;
        public string event_code;
        public int event_type;
        public District district;
        public string city;
        public string state_prov;
        public string country;
        public string start_date;
        public DateTime StartDate => DateTime.ParseExact(start_date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public string end_date;
        public DateTime EndDate => DateTime.ParseExact(end_date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        public int year;


        [Serializable]
        public class District
        {
            public string key;
            public string display_name;
            public string abbreviation;
            public int year;
        }


        public static IEnumerator<WrapperClass<SimpleEvent[]>> GetWithTeamInYear(string team_key, int year) => APICaller.GetInformation<WrapperClass<SimpleEvent[]>>($"/team/{team_key}/events/{year}/simple");
    }
    [Serializable]
    public class SimpleMatch
    {
        public string key;
        public string comp_level;
        public CompLevels CompLevel
        {
            get
            {
                _CompLevel ??= (CompLevels)Enum.Parse(typeof(CompLevels), comp_level);
                return (CompLevels)_CompLevel;
            }
        }
        private CompLevels? _CompLevel;
        public int set_number;
        public int match_number;
        public Alliances alliances;
        public string winning_alliance;
        public string event_key;
        public int time;
        public int predicted_time;
        public int actual_time;


        [Serializable]
        public class Alliances
        {
            public Alliance red;
            public Alliance blue;

            public string GetTeamKey(int index)
            {
                return (index < 3 ? blue : red).team_keys[index % 3];
            }
        }
        [Serializable]
        public class Alliance
        {
            public int score;
            public string[] dq_team_keys;
            public string[] surrogate_team_keys;
            public string[] team_keys;
        }


        public static IEnumerator<WrapperClass<SimpleMatch[]>> GetFromEvent(string event_key) => APICaller.GetInformation<WrapperClass<SimpleMatch[]>>($"/event/{event_key}/matches/simple");
        public static string GetMatchKey(CompLevels comp_level, int set_number, int match_number)
        {
            if (comp_level == CompLevels.qm) return $"{CurrentEvent.key}_qm{match_number}";
            else return $"{CurrentEvent.key}_{comp_level}{set_number}m{match_number}";
        }
        public static IEnumerator<SimpleMatch> Get(CompLevels comp_level, int set_number, int match_number) => Get(GetMatchKey(comp_level, set_number, match_number));
        public static IEnumerator<SimpleMatch> Get(string match_key) => APICaller.GetInformation<SimpleMatch>($"/match/{match_key}/simple");
    }
    [Serializable]
    public class SimpleTeam
    {
        public string key;
        public int team_number;
        public string name;
        public string nickname;
        public string city;
        public string state_prov;
        public string country;

        public static IEnumerator<SimpleTeam> Get(string team_key) => APICaller.GetInformation<SimpleTeam>($"/team/{team_key}/simple");
    }

    public static IEnumerator SetToCurrentEvent(MonoBehaviour src, string overrideKey)
    {
        CurrentEvent = null;
        string scouter_team_key = "frc4450";
        DateTime currentTime = DateTime.Now.Date;

        IEnumerator iterator = SimpleEvent.GetWithTeamInYear(scouter_team_key, currentTime.Year);
        yield return src.StartCoroutine(iterator);
        if (iterator.Current == null) yield break;
        SimpleEvent[] events = (WrapperClass<SimpleEvent[]>)iterator.Current;

        SimpleEvent overrideEvent = null;
        for (int i = events.Length - 1; i >= 0; i--)
        {
            if (overrideKey == events[i].key)
            {
                overrideEvent = events[i];
                break;
            }

            if (events[i].StartDate < currentTime)
            {
                if (events[i].EndDate > currentTime)
                {
                    IsTesting = false;
                    CurrentEvent = events[i];
                }
                else if (i == events.Length - 1)
                {
                    IsTesting = true;
                    CurrentEvent = events[i];
                }

                if (overrideKey != "") continue;
                else break;
            }
        }
        if (overrideEvent != null) CurrentEvent = overrideEvent;
        // so that we can test on last known event if no future known events exist (assuming they insert at 0)
        if (CurrentEvent == null && events.Length > 0) CurrentEvent = events[0];


        if (CurrentEvent == null) CurrentEventMatches = null;
        else
        {
            iterator = SimpleMatch.GetFromEvent(CurrentEvent.key);
            yield return src.StartCoroutine(iterator);
            if (iterator.Current == null) yield break;
            List<SimpleMatch> forSort = ((WrapperClass<SimpleMatch[]>)iterator.Current).Data.ToList();
            forSort.Sort(matchSorter);
            CurrentEventMatches = forSort.ToArray();
        }
    }
}