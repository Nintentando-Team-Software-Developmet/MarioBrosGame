using System.Collections.Generic;
using System;
using System.IO;
namespace SuperMarioBros.Source.Managers;

public class HighScoreManager
{
    private string _highScorePath = "resources/HighScores";

    public int GetHighScoreAtLevel(int level)
    {
        int integer = -1;
        if (IsValidLevel(level))
        {
            Console.WriteLine($"el highsScore es: {integer}");
            return integer;
        }
        string[] lines = File.ReadAllLines(_highScorePath);

        foreach (string line in lines)
        {
            if (level == 0)
            {
                break;
            }
            if (int.TryParse(line, out int number))
            {
                level--;
                integer = number;
            }


        }
        Console.WriteLine($"el highsScore es: {integer}");
        return integer;
    }
    public void UpdateHighScoreAtLevel(int level, int newScore)
    {
        if (IsValidLevel(level))
        {
            Console.WriteLine($"se rechazó el nivel{level}");
            return;
        }
        string[] lines = File.ReadAllLines(_highScorePath);
        if (int.TryParse(lines[level - 1], out int oldScore))
        {
            if (newScore > oldScore)
            {
                lines[level - 1] = $"{newScore}";
                File.WriteAllLines(_highScorePath, lines);
                Console.WriteLine($"el nuevo highsScore es: {newScore}");
            }
        }
    }

    private bool IsValidLevel(int level)
    {
        if (level < 0)
        {
            return false;
        }
        string[] levelsHighScores = File.ReadAllLines(_highScorePath);

        if (level >= levelsHighScores.Length)
        {
            return false;
        }
        return true;
    }
}
