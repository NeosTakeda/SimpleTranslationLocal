using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.AppCommon {
    class AppUtil {
        private readonly static List<string> _dataFiles = new List<string>();

        public static string convertToFileName(string data) {
            if (0 == _dataFiles.Count) {
                for (var i = (int)'a'; i <= (int)'z'; i++) {
                    _dataFiles.Add(((char)i).ToString());
                    for (var j = (int)'a'; j <= (int)'z'; j++) {
                        _dataFiles.Add(((char)i).ToString() + ((char)j).ToString());
                    }
                }
            }

            var result = data.Trim().ToLower();
            var nm = "";
            if (1 == result.Length) {
            } else {
                result = result.Substring(0, 2);
            }
            if (!_dataFiles.Contains(result)) {
                result = "!!";
            }
            return result;
        }
    }
}
