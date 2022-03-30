using System;
using System.IO;

namespace App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            uint size = 10;
            Users users = new Users(size);

            string path = "11.txt";
            users.SaveToFile(path);

            User[] users2 = Users.LoadFromFile(path);

            if (users != null)
            {
                DateTime now = DateTime.Now;

                int imin = 0;
                int imax = 0;

                for (int i = 1; i < size; i++)
                {
                    if (now.Subtract(users2[i].Birthday) < now.Subtract(users2[imin].Birthday))
                        imin = i;

                    if (now.Subtract(users2[i].Birthday) > now.Subtract(users2[imax].Birthday))
                        imax = i;
                }

                string path2 = "22.txt";

                File.AppendAllText(path2, "The youngest: ");
                users2[imin].AddToFile(path2);

                File.AppendAllText(path2, "The oldest: ");
                users2[imax].AddToFile(path2);
            }

        }

        class User
        {
            private string _name;
            private string _secName;
            private string _surname;

            public User(string name, string secName, string surname, DateTime birthday = new DateTime())
            {
                this.Name = name;
                this.SecondName = secName;
                this.Surname = surname;
                this.Birthday = birthday;
            }

            public override string ToString()
            {
                return String.Format("{0} {1} {2}, {3}", this.Name, this.SecondName, this.Surname, this.Birthday.ToShortDateString());
            }

            public void AddToFile(string path)
            {
                File.AppendAllText(path, this.ToString() + "\n");
            }

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = _isAllAlpha(value) ? value : "";
                }
            }

            public string SecondName
            {
                get { return _secName; }
                set
                {
                    _secName = _isAllAlpha(value) ? value : "";
                }
            }
            public string Surname
            {
                get { return _surname; }
                set
                {
                    _surname = _isAllAlpha(value) ? value : "";
                }
            }

            public DateTime Birthday { get; set; }

            private static bool _isAllAlpha(string text)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (!Char.IsLetter(text[i]))
                        return false;
                }
                return true;
            }
        }

        class Users
        {
            public uint Size { get; private set; }
            private User[] users;
            public Users(uint size)
            {
                this.Size = size;
                this.users = new User[size];
                this.Randomize();
            }

            private void Randomize()
            {
                Random random = new Random();

                for (int i = 0; i < this.Size; i++)
                {
                    users[i] = new User("name" + (char)('A' + i), "secondname" + (char)('A' + i), "surname" + (char)('A' + i));
                    users[i].Birthday = new DateTime(random.Next(1950, 2022), random.Next(1, 12), random.Next(1, 31));
                }
            }

            public void SaveToFile(string path)
            {
                File.WriteAllText(path, "");
                for (int i = 0; i < this.Size; i++)
                {
                    File.AppendAllText(path, this.users[i].ToString() + "\n");
                }
            }

            public static User[] LoadFromFile(string path)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        DateTime now = DateTime.Now;
                        string[] lines = File.ReadAllLines(path);

                        User[] users = new User[lines.Length];

                        for (uint i = 0; i < lines.Length; i++)
                        {
                            int idx = lines[i].IndexOf(',');

                            string[] tmp = lines[i].Substring(0, idx).Split(" ");
                            users[i] = new User(tmp[0], tmp[1], tmp[2]);

                            tmp = lines[i].Substring(idx + 1).Split(".");

                            users[i].Birthday = new DateTime(int.Parse(tmp[2]), int.Parse(tmp[1]), int.Parse(tmp[0]));
                        }
                        return users;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
