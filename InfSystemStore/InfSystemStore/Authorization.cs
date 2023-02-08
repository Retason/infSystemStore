using System.Collections.Generic;

namespace InfSystemStore
{
    class Authorization
    {
        //Поиск пользователя с таким же логином и паролем
        public User Autorize(string login, string password) =>
            DataStorage.Users.Find(x => x.Login == login && x.Password == password);
    }
}
  