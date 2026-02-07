using System.Collections.Generic;
using UnityEngine;

public class KeyboardMahjongSystem : MonoBehaviour
{
    Dictionary<char, char[]> columns = new()
    {
        { 'q', new[]{ 'q','Q','a','A','z','Z' } },
        { 'a', new[]{ 'q','Q','a','A','z','Z' } },
        { 'z', new[]{ 'q','Q','a','A','z','Z' } },

        { 'w', new[]{ 'w','W','s','S','x','X' } },
        { 's', new[]{ 'w','W','s','S','x','X' } },
        { 'x', new[]{ 'w','W','s','S','x','X' } },

        { 'e', new[]{ 'e','E','d','D','c','C' } },
        { 'd', new[]{ 'e','E','d','D','c','C' } },
        { 'c', new[]{ 'e','E','d','D','c','C' } },

        { 'r', new[]{ 'r','R','f','F','v','V' } },
        { 'f', new[]{ 'r','R','f','F','v','V' } },
        { 'v', new[]{ 'r','R','f','F','v','V' } },

        { 't', new[]{ 't','T','g','G','b','B' } },
        { 'g', new[]{ 't','T','g','G','b','B' } },
        { 'b', new[]{ 't','T','g','G','b','B' } },

        { 'y', new[]{ 'y','Y','h','H','n','N' } },
        { 'h', new[]{ 'y','Y','h','H','n','N' } },
        { 'n', new[]{ 'y','Y','h','H','n','N' } },
    };

    Dictionary<char, int> index = new();

    void Awake()
    {
        foreach (var k in columns.Keys)
            index[k] = 0;
    }

    public char Resolve(char key)
    {
        if (!columns.ContainsKey(key))
            return '\0';

        index[key] = (index[key] + 1) % columns[key].Length;
        return columns[key][index[key]];
    }
}
