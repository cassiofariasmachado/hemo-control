using HemoControl.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HemoControl.Database
{

    public static class DbInitializer
    {

        public static void Initialize(HemoControlContext context)
        {
            if (!context.Database.GetPendingMigrations().Any()
                && context.Database.GetMigrations().Any())
            {
                if (!context.Users.Any())
                {
                    AddUsers(context);
                }
            }
        }

        private static void AddUsers(HemoControlContext context)
        {

            var users = new User[] {
                new User ("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", 78),
                new User ("Lavinia", "Carvalho Ferreira", "laviniacferreira@gmail.com", 80),
                new User ("Bruno", "Dias Pinto", "brunodiasp@yahoo.com", 67),
                new User ("Otávio", "Carvalho Goncalves", "otaviocg@hotmail.com", 87),
                new User ("Victor", "Alves Barros", "victorab@gmail.com", 58),
                new User ("Giovana", "Correia Barros", "giocb@bol.com.br", 23)
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }

    }

}