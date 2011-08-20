using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PetaPoco
{
    public static class PetaPocoRelationExtensions
    {
        public static List<T> FetchOneToMany<T, T1>(this Database db, Func<T, object> key, Sql Sql)
        {
            return db.FetchOneToMany<T, T1>(key, Sql.SQL, Sql.Arguments);
        }

        public static List<T> FetchManyToOne<T, T1>(this Database db, Func<T, object> key, Sql Sql)
        {
            return db.FetchManyToOne<T, T1>(key, Sql.SQL, Sql.Arguments);
        }

        public static List<T> FetchManyToOne<T, T1, T2>(this Database db, Func<T, object> key, Sql Sql)
        {
            return db.FetchManyToOne<T, T1, T2>(key, Sql.SQL, Sql.Arguments);
        }

        public static List<T> FetchManyToOne<T, T1, T2, T3>(this Database db, Func<T, object> key, Sql Sql)
        {
            return db.FetchManyToOne<T, T1, T2, T3>(key, Sql.SQL, Sql.Arguments);
        }

        public static List<T> FetchOneToMany<T, T1>(this Database db, Func<T, object> key, string sql, params object[] args)
        {
            var relator = new Relator();
            return db.Fetch<T, T1, T>((a, b) => relator.OneToMany(a, b, key), sql, args);
        }

        public static List<T> FetchManyToOne<T, T1>(this Database db, Func<T, object> key, string sql, params object[] args)
        {
            var relator = new Relator();
            return db.Fetch<T, T1, T>((a, b) => relator.ManyToOne(a, b, key), sql, args);
        }

        public static List<T> FetchManyToOne<T, T1, T2>(this Database db, Func<T, object> key, string sql, params object[] args)
        {
            var relator = new Relator();
            return db.Fetch<T, T1, T2, T>((a, b, c) => relator.ManyToOne(a, b, c, key), sql, args);
        }

        public static List<T> FetchManyToOne<T, T1, T2, T3>(this Database db, Func<T, object> key, string sql, params object[] args)
        {
            var relator = new Relator();
            return db.Fetch<T, T1, T2, T3, T>((a, b, c, d) => relator.ManyToOne(a, b, c, d, key), sql, args);
        }
    }

    public class Relator
    {
        private Dictionary<string, object> existingmanytoone = new Dictionary<string, object>();
        private List<string> properties = new List<string>();
        private PropertyInfo property1, property2, property3;
        
        public T ManyToOne<T, TSub1>(T main, TSub1 sub, Func<T, object> idFunc)
        {
            property1 = GetProperty<T, TSub1>(property1);
            sub = GetSub(main, sub, idFunc);
            property1.SetValue(main, sub, null);

            return main;
        }

        public T ManyToOne<T, TSub1, TSub2>(T main, TSub1 sub1, TSub2 sub2, Func<T, object> idFunc)
        {
            property1 = GetProperty<T, TSub1>(property1);
            property2 = GetProperty<T, TSub2>(property2);

            sub1 = GetSub(main, sub1, idFunc);
            sub2 = GetSub(main, sub2, idFunc);

            property1.SetValue(main, sub1, null);
            property2.SetValue(main, sub2, null);

            return main;
        }

        public T ManyToOne<T, TSub1, TSub2, TSub3>(T main, TSub1 sub1, TSub2 sub2, TSub3 sub3, Func<T, object> idFunc)
        {
            property1 = GetProperty<T, TSub1>(property1);
            property2 = GetProperty<T, TSub2>(property2);
            property3 = GetProperty<T, TSub3>(property3);

            sub1 = GetSub(main, sub1, idFunc);
            sub2 = GetSub(main, sub2, idFunc);
            sub3 = GetSub(main, sub3, idFunc);

            property1.SetValue(main, sub1, null);
            property2.SetValue(main, sub2, null);
            property3.SetValue(main, sub3, null);

            return main;
        }

        private PropertyInfo GetProperty<T, TSub>(PropertyInfo property)
        {
            if (property == null)
            {
                property = typeof (T).GetProperties()
                    .Where(x => typeof (TSub) == x.PropertyType && !properties.Contains(x.Name))
                    .FirstOrDefault();

                if (property == null)
                    ThrowPropertyNotFoundException<T, TSub>();

                properties.Add(property.Name);
            }

            return property;
        }

        private TSub GetSub<T, TSub>(T main, TSub sub, Func<T, object> idFunc)
        {
            object existing;
            if (existingmanytoone.TryGetValue(idFunc(main) + typeof (TSub).Name, out existing))
                sub = (TSub) existing;
            else
                existingmanytoone.Add(idFunc(main) + typeof(TSub).Name, sub);
            return sub;
        }

        private object onetomanycurrent;
        public T OneToMany<T, TSub>(T main, TSub sub, Func<T, object> idFunc)
        {
            if (main == null)
                return (T)onetomanycurrent;

            if (property1 == null)
            {
                property1 = typeof(T).GetProperties().Where(x => typeof(ICollection<TSub>).IsAssignableFrom(x.PropertyType)).FirstOrDefault();
                if (property1 == null)
                    ThrowPropertyNotFoundException<T, ICollection<TSub>>();
            }

            if (onetomanycurrent != null && idFunc((T)onetomanycurrent).Equals(idFunc(main)))
            {
                ((ICollection<TSub>)property1.GetValue((T)onetomanycurrent, null)).Add(sub);
                return default(T);
            }

            var prev = (T)onetomanycurrent;
            onetomanycurrent = main;

            property1.SetValue((T)onetomanycurrent, new List<TSub> { sub }, null);

            return prev;
        }

        private static void ThrowPropertyNotFoundException<T, TSub1>()
        {
            throw new Exception(string.Format("No Property of type {0} found on object of type: {1}", typeof(TSub1).Name, typeof(T).Name));
        }
    }
}