using System;
using System.Linq;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            //InsertNewPkFkGraph();
            //InsertNewPkFkGraphMultipleChildren();
            //AddChildToExistingObjectWhileTracked();
            //AddChildToExistingObjectWhileNotTracked(1);
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
            // alternate: call a stored procedure!
            // _ctx.Database.ExecuteSqlCommand("exec DeleteById {0}", samuraiId);
        }

        private static void InsertNewPkFkGraph()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "I've come to save you"}
                }
            };
            _ctx.Samurais.Add(samurai);
            _ctx.SaveChanges();
        }

        private static void InsertNewPkFkGraphMultipleChildren()
        {
            var samurai = new Samurai
            {
                Name = "Kyūzō",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Watch out for my sharp sword! "},
                    new Quote { Text = "I told you to watch out for the sharp sword! Oh well! "}
                }
            };
            _ctx.Samurais.Add(samurai);
            _ctx.SaveChanges();
        }

        private static void AddChildToExistingObjectWhileTracked()
        {
            var samurai = _ctx.Samurais.First();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _ctx.SaveChanges();
        }

        private static void AddChildToExistingObjectWhileNotTracked(int samuraiId)
        {
            var quote = new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?",
                SamuraiId = samuraiId
            };
            using (var _newCtx = new SamuraiContext())
            {
                _newCtx.Quotes.Add(quote);
                _newCtx.SaveChanges();
            }
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            // Return rich objects graphs, always loading the entire set of related objects. The execution is done with 2 queries:
            // 1. Gather a filtered list of samurais to avoid duplicates
            // 2. Gather the quotes from the recovered and filtered list of samurais, doing an inner join
            var samuraiWithQuotes = _ctx.Samurais.Include(s => s.Quotes).ToList();
            // Filtered samurais example
            var samuraiWithQuotes2 = _ctx.Samurais
                .Where(s => s.Name.Contains("Julie"))
                .Include(s => s.Quotes).ToList();
            // Filtered samurais example with grandchildren leveraging the usage of .ThenInclude
            //var samuraiWithQuotes3 = _ctx.Samurais
            //    .Where(s => s.Name.Contains("Julie"))
            //    .Include(s => s.Quotes)
            //    .ThenInclude(q => q.Translations)
            //    .ToList();
            // Multiple includes are allowed as well
            var samuraiWithQuotes4 = _ctx.Samurais
                .Where(s => s.Name.Contains("Julie"))
                .Include(s => s.Quotes)
                .Include(s => s.SecretIdentity)
                .FirstOrDefault();
        }

        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }

        private static List<dynamic> ProjectSomeProperties()
        {
            // Returning multiple properties passing an anonymous type
            var someProperties = _ctx.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            // Passing a defined type
            var someProperties2 = _ctx.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
            return someProperties.ToList<dynamic>();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            var someProperties = _ctx.Samurais.Select(s => new { s.Id, s.Name, s.Quotes }).ToList();  // Complete type search return
            var someProperties2 = _ctx.Samurais.Select(s => new { s.Id, s.Name, s.Quotes.Count }).ToList();
            var somePropertiesWithSomeQuotes = _ctx.Samurais
                .Select(s => new { s.Id, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) })  // Filter inner properties is possible as well
                .ToList();
            // Filter related objects with full types: enabling compositions in .NET
            //var samuraisWithHappyQuotes = _ctx.Samurais
            //    .Select(s => new
            //    {
            //        Samurai = s,
            //        Quotes = s.Quotes.Where(q => q.Text.Contains("happy")).ToList()
            //    })
            //    .ToList();
            // Unfortunately, EF Core projections don't connect graphs on this version:
            // - Projected collection navigations don't get tracked if the collection is composed (https://github.com/aspnet/EntityFrameworkCore/issues/8999)
            // Therefore, two separate queries are needed to accomplish such a thing, and EF Core will attach the related data connections automatically.
            var samurais = _ctx.Samurais.ToList();
            var happyQuotes = _ctx.Quotes.Where(q => q.Text.Contains("happy")).ToList();
        }

        private static void FilteringWithRelatedData()
        {
            // Creates a WHERE EXISTS query to filter objects using related data WHERE LINQ method query
            var samurais = _ctx.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                .ToList();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _ctx.Samurais.Include(s => s.Quotes).FirstOrDefault();
            //samurai.Quotes[0].Text += " Did you hear that?";  // UPDATE SQL Query
            _ctx.Quotes.Remove(samurai.Quotes[2]);  // REMOVE SQL Query
            _ctx.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _ctx.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text += " Did you hear that?";
            using (var _newCtx = new SamuraiContext())
            {
                // DbSet native method, which keeps track of all graph related entities
                //_newCtx.Quotes.Update(quote);
                _newCtx.Entry(quote).State = EntityState.Modified;
                _newCtx.SaveChanges();
            }
        }
    }
}
