using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.Recommendation
{
    public class UserData : IEnumerable<User>
    {
        public IReadOnlyList<string> Films { get; }

        private readonly Dictionary<string, User> users;

        public IReadOnlyDictionary<string, User> Users => users;

        public UserData(IReadOnlyList<string> films)
        {
            Films = films;
            users = new Dictionary<string, User>();
        }

        public UserData(params string[] films) : this(films.ToList())
        {
        }

        public void Add(User u)
        {
            users.Add(u.Name, u);
        }

        public IEnumerable<User> NearestNeibours(string name, Func<User, User, double> sim)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var u = users[name];
            return users.Values.Where(us => us != u).OrderByDescending(us => sim(u, us));
        }

        public IEnumerable<User> ENearestNeighbours(string name)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var u = users[name];
            return users.Values.Where(us => us != u).OrderByDescending(us => u.ESimilarity(us));
        }

        public IEnumerable<User> PNearestNeighbours(string name)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var u = users[name];
            return users.Values.Where(us => us != u).OrderByDescending(us => u.PCorrelation(us));
        }

        public IEnumerable<User> NearestNeighbours(string name, Func<User, User, double> sim, string film)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var u = users[name];
            return users.Values.Where(us => us != u && us.Ratings.ContainsKey(film)).OrderByDescending(us => sim(u, us));
        }

        public IEnumerable<User> PNearestNeighbours(string name, string film)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var u = users[name];
            return users.Values.Where(us => us != u && us.Ratings.ContainsKey(film)).OrderByDescending(us => u.PCorrelation(us));
        }

        public double PredictRating1(string name, string film, Func<User, User, string, double> sim)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var result = 0d;
            var target = users[name];
            var validUsers = users.Where(u => u.Value != target && u.Value.Ratings.ContainsKey(film)).Select(u => u.Value);
            foreach (var u in validUsers)
                result += u.Ratings[film];
            return result / validUsers.Count();
        }

        public double PredictRating2(string name, string film, Func<User, User, string, double> sim)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var result = 0d;
            var totalCorrelation = 0d;
            var target = users[name];
            var validUsers = users.Where(u => u.Value != target && u.Value.Ratings.ContainsKey(film)).Select(u => u.Value);
            foreach (var u in validUsers)
            {
                result += u.Ratings[film] * sim(u, target, film);
                totalCorrelation += sim(u, target, film);
            }
            return result / totalCorrelation;
        }

        public double PredictRating3(string name, string film, Func<User, User, string, double> sim)
        {
            if (!users.ContainsKey(name))
                throw new ArgumentException("Name does not exist.");
            var result = 0d;
            var totalCorrelation = 0d;
            var target = users[name];
            var validUsers = users.Where(u => u.Value != target && u.Value.Ratings.ContainsKey(film)).Select(u => u.Value);
            foreach (var u in validUsers)
            {
                result += (u.Ratings[film] - u.Mean(film)) * sim(u, target, film);
                totalCorrelation += sim(u, target, film);
            }
            return target.Mean(film) + result / totalCorrelation;
        }

        public IEnumerator<User> GetEnumerator()
        {
            return users.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
