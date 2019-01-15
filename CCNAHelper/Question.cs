using System.Collections.Generic;

namespace CCNAHelper
{
    public class Question
    {
        public string Body;
        public string[] Answers;

        public Question(string Body, string[] Answers)
        {
            this.Body = Body;
            this.Answers = Answers;
        }
    }
}
