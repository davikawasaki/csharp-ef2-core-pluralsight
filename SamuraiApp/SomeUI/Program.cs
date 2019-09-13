using System;
using System.Linq;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace SomeUI
{
    class Program
    {
        private static SamuraiContext _ctx = new SamuraiContext();
        static void Main(string[] args)
        {
            //InsertSamurai();
            //InsertMultipleSamurais();
            //SimpleSamuraiQuery();
            //MoreQueries();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //AddMoreSamurais();
            //DeleteMany();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Julie" };
            _ctx.Samurais.Add(samurai);  // The context is now tracking the samurai object
            _ctx.SaveChanges();
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "Julie" };
            var samuraiSammy = new Samurai { Name = "Sampson" };
            _ctx.Samurais.AddRange(samurai, samuraiSammy);  // The context is now tracking the samurai object
            _ctx.SaveChanges();
        }

        private static void InsertMultipleDifferentObjects()
        {
            var samurai = new Samurai { Name = "Oda Nobunaga" };
            var battle = new Battle
            {
                Name = "Battle of Nagashino",
                StartDate = new DateTime(1575, 06, 16),
                EndDate = new DateTime(1575, 06, 28)
            };
            _ctx.AddRange(samurai, battle);
            _ctx.SaveChanges();
        }

        private static void SimpleSamuraiQuery()
        {
            var samurais = _ctx.Samurais.ToList();
        }

        private static void MoreQueries()
        {
            var name = "Sampson";
            var samurais = _ctx.Samurais.Where(s => s.Name == name).ToList();  // With a variable in the exp, EF Core parameterize the query, setting the name as a @__name_0 parameter
            var samurais2 = _ctx.Samurais.FirstOrDefault(s => s.Name == name);  // Is transformed into a SELECT TOP(1) SQL query
            var samurais3 = _ctx.Samurais.Find(2);  // Not LINQ, but a DbSet method (if it's already on memory, EF Core returns the tracking memory object instead of wasting time going to the DB)
            var samurais4 = _ctx.Samurais.Where(s => EF.Functions.Like(s.Name, "J%"));  // Filtering partial text using LINQ
            var samurais5 = _ctx.Samurais.Where(s => s.Name.Contains("abc"));  // Filtering partial text using LINQ (this version will be transformed into a SQL LIKE (%abc%) SQL query
            var lastSampson = _ctx.Samurais.OrderBy(s => s.Id).LastOrDefault(s => s.Name == name);  // Ideal performance for last LINQ method is with order by, because it constructs the SQL with a descending sort natively and not in memory
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _ctx.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _ctx.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _ctx.Samurais.ToList();
            samurais.ForEach(s => s.Name += "San");
            _ctx.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _ctx.Samurais.FirstOrDefault();
            samurai.Name += "Hiro";
            _ctx.Samurais.Add(new Samurai { Name = "Kikuchiyo" });
            _ctx.SaveChanges();
        }

        private static void InsertBattle()
        {
            _ctx.Battles.Add(new Battle
            {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });
            _ctx.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _ctx.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var _newCtx = new SamuraiContext())
            {
                _newCtx.Battles.Update(battle);
                _newCtx.SaveChanges();
            }
        }

        private static void AddMoreSamurais()
        {
            _ctx.AddRange(
                new Samurai { Name = "Kambei Shimada" },
                new Samurai { Name = "Shichirōji" },
                new Samurai { Name = "Katsushirō Okamoto" },
                new Samurai { Name = "Heihachi Hayashida" },
                new Samurai { Name = "Kyūzō" },
                new Samurai { Name = "Gorōbei Katayama" }
            );
            _ctx.SaveChanges();
        }

        private static void DeleteWhileTracked()
        {
            var samurai = _ctx.Samurais.FirstOrDefault(s => s.Name == "Kambei Shimada");
            _ctx.Samurais.Remove(samurai);
            _ctx.SaveChanges();
        }

        private static void DeleteMany()
        {
            var samurais = _ctx.Samurais.Where(s => s.Name.Contains("ō"));
            _ctx.Samurais.RemoveRange(samurais);
            // alternate: _ctx.RemoveRange(samurais);
            _ctx.SaveChanges();
        }

        private static void DeleteWhileNotTracked()
        {
            var samurai = _ctx.Samurais.FirstOrDefault(s => s.Name == "Heihachi Hayashida");
            using (var _newCtx = new SamuraiContext())
            {
                _newCtx.Samurais.Remove(samurai);
                _newCtx.SaveChanges();
            }
        }

        private static void DeleteUsingId(int samuraiId)
        {
            var samurai = _ctx.Samurais.Find(samuraiId);
            _ctx.Remove(samurai);
            _ctx.SaveChanges();
        }
    }
}
