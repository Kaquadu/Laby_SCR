using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Collections;

namespace Bank
{
    //######################### LAMPORT ALG #################################

    public class Bakery
    {
        // internal vars
        private List<bool> _choosing;
        private List<int> _number;
        private int _maxUsers;
        private int _maxValue;

        /// <summary>
        /// Creates a new Bakery instance.
        /// </summary>
        /// <param name="maxUsers">The maximum number of customers to support.</param>
        public Bakery(int maxUsers)
        {
            _choosing = new List<bool>();
            _number = new List<int>();
            _maxUsers = maxUsers;

            // make sure collections are ready
            initializeCollections(maxUsers);
        }

        /// <summary>
        /// Acquires the exclusive lock for the specified user.
        /// </summary>
        public void Lock(int id)
        {
            // The 'doorway' is where we wait in line
            // to get a ticket for the bakery. 
            _choosing[id] = true;
            _number[id] = ++_maxValue;
            _choosing[id] = false;

            // This is the rest of the waiting line
            for (var j = 0; j < _maxUsers; j++)
            {
                while (_choosing[j]) { Thread.Sleep(1); }

                while ((_number[j] != 0) && (bakeryCompare(j, id))) { Thread.Sleep(1); }
            }
        }

        /// <summary>
        /// Releases the critical section.
        /// </summary>
        public void Unlock(int id)
        {
            _number[id] = 0;
        }

        /// <summary>
        /// Performns a comparison against tokens in the bakery wait line.
        /// </summary>
        private bool bakeryCompare(int j, int i)
        {
            return (_number[j] < _number[i]) ||
                ((_number[j] == _number[i]) && (j < i));
        }

        /// <summary>
        /// Ensures that the shared variables are correctly initialized.
        /// </summary>
        private void initializeCollections(int size)
        {
            for (var j = 0; j < size; j++)
            {
                _number.Add(0);
                _choosing.Add(false);
            }
        }
    }
}
