using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia_Client
{
    class Protocol
    {
        Dictionary<string, string> errorStr;

        public Protocol() {
            errorStr = new Dictionary<string, string>();
            errorStr.Add("1020", "success");
            errorStr.Add("1021", "Wrong Details.");
            errorStr.Add("1022", "User is already connected.");
            errorStr.Add("1040", "success");
            errorStr.Add("1041", "Pass illegal");
            errorStr.Add("1042", "Username is already exists");
            errorStr.Add("1043", "Username is illegal");
            errorStr.Add("1044", "Other");
            errorStr.Add("1100", "success");
            errorStr.Add("1101", "Room is full");
            errorStr.Add("1102", "Room does not exist or other reason");
            errorStr.Add("1120", "success");
        }

        public string getCodeErrorMsg(string code) {
            string result = "";
            try {
                errorStr.TryGetValue(code, out result);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            return result;
        }
    }
}
