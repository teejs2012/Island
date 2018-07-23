using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameData {

    public List<string> TriggeredObjects = new List<string>();
    public List<int> ActiveKeys = new List<int>();
    public List<int> UsedKeys = new List<int>();

    public override string ToString() {
        StringBuilder result = new StringBuilder();
        result.Append("Triggered Objects: ");
        foreach(var t in TriggeredObjects)
        {
            result.Append(t);
            result.Append(",");
        }

        result.Append("; ActiveKeys: ");

        foreach(var k in ActiveKeys)
        {
            result.Append(k);
            result.Append(",");
        }

        result.Append("; UsedKeys: ");

        foreach (var k in UsedKeys)
        {
            result.Append(k);
            result.Append(",");
        }

        return result.ToString();
    }
}
