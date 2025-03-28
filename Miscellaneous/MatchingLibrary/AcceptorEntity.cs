using System;
using System.Collections.Generic;

namespace Miscellaneous.MatchingLibrary
{
    public class AcceptorEntity<T, U> where T : IComparable<T> where U : IComparable<U>
    {
        #region Constructors

        public AcceptorEntity(T entity)
        {
            Entity = entity;
            Preferences = new Queue<SuitorEntity<U, T>>();
        }

        #endregion

        #region Properties

        public T Entity { get; private set; }

        public Queue<SuitorEntity<U, T>> Preferences { get; private set; }

        #endregion
    }
}
