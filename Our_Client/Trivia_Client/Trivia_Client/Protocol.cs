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
