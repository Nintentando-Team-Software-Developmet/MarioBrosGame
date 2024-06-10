using System.Collections.Generic;
using System;
using System.IO;
namespace SuperMarioBros.Source.Managers;

public class HighScoreManager
{
    private string _highScorePath = "resources/HighScore";

    /*
     * GetHighScore returns the int value of the highScore saved on resources
     */
    public int GetHighScore()
    {
        int integer = -1;
        string[] lines = File.ReadAllLines(_highScorePath);

        if (int.TryParse(lines[0], out int number))
        {
            integer = number;
        }
        return integer;
    }
    /*
     * UpdateHighScore updates the HighScore if the new one is greater than that one
     */
    public void UpdateHighScore(int newScore)
    {
        string[] lines = File.ReadAllLines(_highScorePath);
        if (int.TryParse(lines[0], out int oldScore))
        {
            if (newScore > oldScore)
            {
                lines[0] = $"{newScore}";
                File.WriteAllLines(_highScorePath, lines);
            }
        }
    }
}
