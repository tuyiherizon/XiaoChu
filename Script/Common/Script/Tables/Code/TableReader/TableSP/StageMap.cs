using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tables
{
    public class StageMapRecord
    {
        public int _Width = 0;
        public int _Height = 0;
        public Dictionary<string, string> _MapDefaults = new Dictionary<string, string>();

        public string GetMapPos(int x, int y)
        {
            string key = x + "," + y;
            if (_MapDefaults.ContainsKey(key))
            {
                return _MapDefaults[key];
            }

            return "";
        }

        public static StageMapRecord ReadStageMap(string path)
        {
            StageMapRecord record = new StageMapRecord();
            var tableAsset = ResourceManager.GetTable(path);
            string[] splits = tableAsset.Split('\n');
            foreach (var splitStr in splits)
            {
                string[] keyValue = splitStr.Split('=');
                if (keyValue[0].Equals("Size"))
                {
                    string[] sizeStrs = keyValue[1].Split(',');
                    record._Width = int.Parse(sizeStrs[0]);
                    record._Height = int.Parse(sizeStrs[1]);
                }
                else
                {
                    record._MapDefaults.Add(keyValue[0].Trim('\n', '\r', ' '), keyValue[1].Trim('\n', '\r', ' '));
                }
                 
            }

            return record;
        }

    }

    public class StageMap
    {
        
    }
}