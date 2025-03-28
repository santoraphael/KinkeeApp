using System;
using System.Collections.Generic;

namespace Miscellaneous.MatchingLibrary
{
    public class SuitorEntity<T, U> where T : IComparable<T> where U : IComparable<U>
    {
        #region Constructors

        public SuitorEntity(T entity)
        {
            Entity = entity;
            Preferences = new Queue<AcceptorEntity<U, T>>();
        }

        #endregion

        #region Properties

        public T Entity { get; private set; }

        public Queue<AcceptorEntity<U, T>> Preferences { get; private set; }

        #endregion

    }
}
