using System.Collections.Generic;

namespace SimpleTranslationLocal.AppCommon {
    class AppUtil {

        #region Declaration
        private readonly static string DataKeys = "abcdefghijklmnopqrstuvwxyz";
        private readonly static List<string> _dataFiles = new List<string>();
        #endregion

        #region Internal Method
        /// <summary>
        /// 検索用語を検索対象となるファイル名に変換
        /// </summary>
        /// <param name="data">検索キーワード</param>
        /// <returns>ファイル名(親ディレクトリ絵を含む場合もあり)</returns>
        public static string ConvertToFileName(string data) {
            if (0 == _dataFiles.Count) {
                GetKeyList();
            }

            var fileName = data.Trim().ToLower();
            var result = fileName;
            if (3 <= result.Length) {
                result = $@"{result.Substring(0, 2)}\{result.Substring(2, 1)}";
            }

            if (!_dataFiles.Contains(result.Replace(@"\",""))) {
                if (3 <= fileName.Length) {
                    if (_dataFiles.Contains(fileName.Substring(0,2))) {
                        result = $@"{fileName.Substring(0, 2)}\!!";
                    } else {
                        result = "!!";
                    }
                } else {
                    result = "!!";
                }
            } else if (2 == result.Length) {
                result = $@"{result}\{result}";
            }
            return result;
        }

        /// <summary>
        /// ファイル検索用のリストを作成
        /// </summary>
        /// <returns></returns>
        public static List<string> GetKeyList() {
            if (0 < _dataFiles.Count) {
                return _dataFiles;
            }

            string s1;
            string s2;
            for (var i = 0; i < DataKeys.Length; i++) {
                s1 = DataKeys.Substring(i, 1);
                _dataFiles.Add(s1);
                for (var j = 0; j < DataKeys.Length; j++) {
                    s2 = s1 + DataKeys.Substring(j, 1);
                    _dataFiles.Add(s2);
                    for (var k = 0; k < DataKeys.Length; k++) {
                        _dataFiles.Add(s2 + DataKeys.Substring(k, 1));
                    }
                }
            }
            return _dataFiles;
        }
        #endregion
    }
}
