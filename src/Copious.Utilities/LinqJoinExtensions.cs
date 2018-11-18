using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Copious.Utilities {
    // https://www.codeproject.com/Articles/488643/LinQ-Extended-Joins
    // https://www.codeproject.com/Articles/1181451/Visual-Studio-Collection-Visualizers

    public static class LinqJoinExtensions {
        public static IEnumerable<TResult> LeftJoin<TSource, TInner, TKey, TResult> (this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        where TSource : class
        where TInner : class {
            if (pk == null) throw new ArgumentNullException (nameof (pk));
            if (fk == null) throw new ArgumentNullException (nameof (fk));
            if (result == null) throw new ArgumentNullException (nameof (result));

            return from s in source
            join i in inner
            on pk (s) equals fk (i) into joinData
            from left in joinData.DefaultIfEmpty ()
            select result (s, left);
        }

        public static IQueryable<TResult> LeftJoin<TSource, TInner, TKey, TResult> (this IQueryable<TSource> source,
            IQueryable<TInner> inner,
            Expression<Func<TSource, TKey>> pk,
            Expression<Func<TInner, TKey>> fk,
            Expression<Func<TSource, TInner, TResult>> result)
        where TSource : class
        where TInner : class => source.Join (inner, pk, fk, result).DefaultIfEmpty ().AsQueryable ();

        public static IEnumerable<TResult> RightJoin<TSource, TInner, TKey, TResult> (this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        where TSource : class
        where TInner : class {
            if (pk == null) throw new ArgumentNullException (nameof (pk));
            if (fk == null) throw new ArgumentNullException (nameof (fk));
            if (result == null) throw new ArgumentNullException (nameof (result));

            return from i in inner
            join s in source
            on fk (i) equals pk (s) into joinData
            from right in joinData.DefaultIfEmpty ()
            select result (right, i);
        }

        public static IQueryable<TResult> RightJoin<TSource, TInner, TKey, TResult> (this IQueryable<TSource> source,
            IQueryable<TInner> inner,
            Expression<Func<TSource, TKey>> pk,
            Expression<Func<TInner, TKey>> fk,
            Expression<Func<TInner, TSource, TResult>> result)
        where TSource : class
        where TInner : class => inner.Join (source, fk, pk, result).DefaultIfEmpty ().AsQueryable ();

        public static IEnumerable<TResult> FullOuterJoinJoin<TSource, TInner, TKey, TResult> (this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        where TSource : class
        where TInner : class {
            if (pk == null) throw new ArgumentNullException (nameof (pk));
            if (fk == null) throw new ArgumentNullException (nameof (fk));
            if (result == null) throw new ArgumentNullException (nameof (result));

            var left = source.LeftJoin (inner, pk, fk, result).ToList ();
            var right = source.RightJoin (inner, pk, fk, result).ToList ();

            return left.Union (right);
        }

        public static IEnumerable<TResult> LeftExcludingJoin<TSource, TInner, TKey, TResult> (this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        where TSource : class
        where TInner : class {
            if (pk == null) throw new ArgumentNullException (nameof (pk));
            if (fk == null) throw new ArgumentNullException (nameof (fk));
            if (result == null) throw new ArgumentNullException (nameof (result));

            return from s in source
            join i in inner
            on pk (s) equals fk (i) into joinData
            from left in joinData.DefaultIfEmpty ()
            where left == null
            select result (s, left);
        }

        public static IEnumerable<TResult> RightExcludingJoin<TSource, TInner, TKey, TResult> (this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        where TSource : class
        where TInner : class {
            if (pk == null) throw new ArgumentNullException (nameof (pk));
            if (fk == null) throw new ArgumentNullException (nameof (fk));
            if (result == null) throw new ArgumentNullException (nameof (result));

            return from i in inner
            join s in source
            on fk (i) equals pk (s) into joinData
            from right in joinData.DefaultIfEmpty ()
            where right == null
            select result (right, i);
        }

        public static IEnumerable<TResult> FulltExcludingJoin<TSource, TInner, TKey, TResult> (this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        where TSource : class
        where TInner : class {
            if (pk == null) throw new ArgumentNullException (nameof (pk));
            if (fk == null) throw new ArgumentNullException (nameof (fk));
            if (result == null) throw new ArgumentNullException (nameof (result));

            var left = source.LeftExcludingJoin (inner, pk, fk, result).ToList ();
            var right = source.RightExcludingJoin (inner, pk, fk, result).ToList ();

            return left.Union (right);
        }
    }
}