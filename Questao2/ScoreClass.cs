using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Questao2
{
    public class ScoreClass
    {
        private const string SCORE_API = "https://jsonmock.hackerrank.com/api/football_matches";

        public class Pagination
        {
            public int Page { get; set; }
            public int Per_page { get; set; }
            public int Total { get; set; }
            public int Total_pages { get; set; }
            public Match[] Data { get; set; }
        }
        public class Match
        {
            public string Competition { get; set; }
            public int Year { get; set; }
            public string Round { get; set; }
            public string Team1 { get; set; }
            public string Team2 { get; set; }
            public int Team1goals { get; set; }
            public int Team2goals { get; set; }
        }

        public async Task<int> GetTeamScore(string team, int year)
        {
            int totalGoals = 0;

            List<Match> allMatches = await GetAllMatchesForTeam(team, year);

            allMatches.ForEach(match =>
            {
                if (match.Team1 == team)
                {
                    totalGoals += match.Team1goals;
                }

                if (match.Team2 == team)
                {
                    totalGoals += match.Team2goals;
                }

            });


            return totalGoals;
        }

        public async Task<List<Match>> GetAllMatchesForTeam(string team, int year)
        {
            var allMatches = new List<Match>();
            var currentPage = 1;
            var currentTeam = "team1"; // Start with team1

            try
            {
                while (true)
                {
                    // Get matches for the current page and team
                    Pagination matchesData = await GetApiScore(team, year, currentPage, currentTeam);

                    if (matchesData == null)
                    {
                        // No data found for the team/year
                        return allMatches;
                    }

                    // Add the matches to the list
                    allMatches.AddRange(matchesData.Data);

                    // Check if we've reached the last page
                    if (currentPage == matchesData.Total_pages)
                    {
                        // If we've reached the last page and we've processed both teams, we're done
                        if (currentTeam == "team2")
                        {
                            break;
                        }

                        // Switch to the other team and reset page to 1
                        currentTeam = "team2";
                        currentPage = 1;
                    }
                    else
                    {
                        // Move to the next page
                        currentPage++;
                    }
                }

                return allMatches;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return allMatches;
        }

        public async Task<Pagination> GetApiScore(string team, int year, int page, string teamParameter)
        {
            string url = $"{SCORE_API}?year={year}&{teamParameter}={team}&page={page}";

            using (var client = new HttpClient())
            {
                try
                {
                    // Make the API call and get the response
                    var response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a C# object
                        return JsonConvert.DeserializeObject<Pagination>(jsonResponse);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error fetching data from API: {e.Message}");
                }
            }

            return null; // Return null on error
        }

    }


}
