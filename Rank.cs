using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;

namespace _2048
{
    [Serializable]
    public class Rank
    {
        public int length = 9;
        public int[] scores;
        public Rank()
        {
            scores = new int[length];
        }
        public void add_score(int score)
        {
            for (int i = 0; i < length; i++)
            {
                if (scores[i] < score)
                {
                    change_rank(i);
                    scores[i] = score;
                    break;
                }
            }
        }
        public void change_rank(int index)
        {
            for (int i = length - 2; i >= index; i--)
            {
                scores[i + 1] = scores[i];
            }
        }
    }
}