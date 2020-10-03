using HemoControl.Entities;
using System.Linq;

namespace HemoControl.Database
{
    public static class DbInitializer
    {
        public static void Initialize(HemoControlContext context)
        {
            if (!context.Users.Any())
            {
                AddUsers(context);
            }
        }

        private static void AddUsers(HemoControlContext context)
        {
            var users = new User[] {
                new User ("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", "cassiofariasmachado", "12345678"),
                new User ("Lavinia", "Carvalho Ferreira", "laviniacferreira@gmail.com", "laviniacferreira", "12345678"),
                new User ("Bruno", "Dias Pinto", "brunodiasp@yahoo.com", "brunodiasp", "12345678"),
                new User ("Otávio", "Carvalho Goncalves", "otaviocg@hotmail.com", "otaviocg", "12345678"),
                new User ("Victor", "Alves Barros", "victorab@gmail.com", "victorab", "12345678"),
                new User ("Giovana", "Correia Barros", "giocb@bol.com.br", "giocb", "12345678")
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }

    }

}