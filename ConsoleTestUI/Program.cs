using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using System;

namespace ConsoleTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ITeamService teams = new TeamManager(new EfTeamDal());

            IMatchService match = new MatchManager(new EfMatchDal());

            var data = match.GetMatchDetails();

            foreach (var d in data)
            {
                Console.WriteLine(d.HomeTeamName + "\t"+d.AwayTeamName);
            }


            foreach (var team in teams.GetAll())
            {
                //Console.WriteLine(team.TeamName);
            }

            //Random rand = new Random();

            //for (int i = 0; i < 50; i++)
            //{
            //    Console.Write(rand.Next(8)+" ");
            //}

            Console.ReadLine();
        }
    }
}
