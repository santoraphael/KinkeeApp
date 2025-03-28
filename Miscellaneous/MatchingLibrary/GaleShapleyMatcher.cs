using System;
using System.Collections.Generic;

namespace Miscellaneous.MatchingLibrary
{
    public static class GaleShapleyMatcher<T, U> where T : IComparable<T> where U : IComparable<U>
    {
        public static SortedDictionary<T, U> Match(HashSet<SuitorEntity<T, U>> optimalEntities, HashSet<AcceptorEntity<U, T>> pessimalEntities)
        {
            Dictionary<AcceptorEntity<U, T>, SuitorEntity<T, U>> matching = new Dictionary<AcceptorEntity<U, T>, SuitorEntity<T, U>>();
            Stack<SuitorEntity<T, U>> processStack = new Stack<SuitorEntity<T, U>>(optimalEntities);
            while (processStack.Count > 0)
            {
                // Match the optimal entity to its first preference
                //If its first preference is not previously matched then automatically match and deque
                SuitorEntity<T, U> suitorEntity = processStack.Pop();
                SuitorEntity<T, U> matchedEntity;
                AcceptorEntity<U, T> acceptorEntity = suitorEntity.Preferences.Dequeue();
                if (!matching.TryGetValue(acceptorEntity, out matchedEntity))
                {
                    matching.Add(acceptorEntity, suitorEntity);
                }
                else//Else
                {
                    foreach (SuitorEntity<T, U> entity in acceptorEntity.Preferences)
                    {
                        if (matchedEntity == entity)
                        {
                            //The Pessimal Entity makes a choice: If it already has a better preference then it keeps it
                            processStack.Push(suitorEntity);
                            break;
                        }
                        if (suitorEntity != entity) continue;
                        matching[acceptorEntity] = suitorEntity;
                        processStack.Push(matchedEntity);
                        break;
                    }

                }

            }
            SortedDictionary<T, U> result = new SortedDictionary<T, U>();
            foreach (KeyValuePair<AcceptorEntity<U, T>, SuitorEntity<T, U>> keyValuePair in matching)
            {
                result.Add(keyValuePair.Value.Entity, keyValuePair.Key.Entity);
            }
            return result;
        }
    }
}

