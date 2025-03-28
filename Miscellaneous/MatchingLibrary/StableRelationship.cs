using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mongo.Models;

namespace Miscellaneous.MatchingLibrary
{
    public class StableRelationship
    {
        public void Suitor(UserModel suitor, UserModel acceptor)
        {
            Person personSuitor = (Person)suitor;
            SuitorEntity<Person, Person> suitorOne = new SuitorEntity<Person, Person>(personSuitor);
            HashSet<SuitorEntity<Person, Person>> suitorEntities = new HashSet<SuitorEntity<Person, Person>>();
            suitorEntities.Add(suitorOne);


            Person personAcceptor = (Person)acceptor;
            AcceptorEntity<Person, Person> acceptorOne = new AcceptorEntity<Person, Person>(personAcceptor);
            HashSet<AcceptorEntity<Person, Person>> acceptorEntities = new HashSet<AcceptorEntity<Person, Person>>();
            acceptorEntities.Add(acceptorOne);


            suitorOne.Preferences.Enqueue(acceptorOne);
            acceptorOne.Preferences.Enqueue(suitorOne);


            foreach (SuitorEntity<Person, Person> suitorEntity in suitorEntities)
            {
                foreach (AcceptorEntity<Person, Person> acceptorEntity in suitorEntity.Preferences)
                {
                    Console.WriteLine("Acceptor : " + acceptorEntity.Entity.Name);
                }
            }

            foreach (AcceptorEntity<Person, Person> acceptorEntity in acceptorEntities)
            {
                foreach (SuitorEntity<Person, Person> suitorEntity in acceptorEntity.Preferences)
                {
                    Console.WriteLine("Acceptor : " + suitorEntity.Entity.Name);
                }
            }


            SortedDictionary<Person, Person> matching = GaleShapleyMatcher<Person, Person>.Match(suitorEntities, acceptorEntities);
            foreach (KeyValuePair<Person, Person> keyValuePair in matching)
            {
                Console.WriteLine(keyValuePair.Key.Name + " : " + keyValuePair.Value.Name);
            }
        }
    }


    public class Person : UserModel, IComparable<UserModel>
    {
        public int CompareTo(UserModel other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
