using Newtonsoft.Json;
using Questao2;
using System;
using System.Text.RegularExpressions;
using static Questao2.ScoreClass;

public class Program
{


    public static async Task Main()
    {

        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014

    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        ScoreClass score = new ScoreClass();

        var totalGoals = await score.GetTeamScore(team, year);

        return totalGoals;
    }


}