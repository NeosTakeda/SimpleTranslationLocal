using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.AppCommon {
    class RegExUtil {

        #region Declaration
        private readonly Regex _regEx;

        private readonly List<string> GroupKeys = new List<string>() { "k1", "k2", "k3" };
        private readonly Dictionary<string, string> _groupValues = new Dictionary<string, string>();
        #endregion

        #region Public Property
        /// <summary>
        /// マッチング結果の妥当性
        /// </summary>
        public bool IsValid { private set; get; } = false;

        /// <summary>
        /// 正規表現にマッチした値
        /// </summary>
        public string Value { private set; get; } = "";

        /// <summary>
        /// グループの値
        /// </summary>
        public string GroupValue(string key) => this._groupValues[key];

        /// <summary>
        /// マッチングした箇所を除いた文字列
        /// </summary>
        public string Remain { private set; get; } = "";
        #endregion

        #region Constructor
        public RegExUtil(string pattern) {
            this._regEx = new Regex(pattern);

            //var regEx = new Regex(@"\?<(?<key>.+?)>");
            //var match = regEx.Match(pattern);
            //if (match.Success) {
            //    this._groupKey = match.Groups["key"].Value;
            //}
        }
        #endregion

        #region Pubilc Method
        /// <summary>
        /// グループの値を取得。
        /// グループの値とマッチング結果を削除した文字列の残りはプロパティを参照
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Match(string data) {
            var match = this._regEx.Match(data);
            this._groupValues.Clear();
            this.Value = Null2Empty(match.Value);
            this.Remain = this._regEx.Replace(data, "").Trim();
            if (match.Success) {
                foreach (var key in GroupKeys) {
                    this._groupValues.Add(key, Null2Empty(match.Groups[key].Value));
                }
                return true;
            } else {
                return false;
            }

        }
        #endregion


        #region Private Method
        /// <summary>
        /// 文字列の空判定
        /// </summary>
        /// <param name="val">検査する値</param>
        /// <returns>true:空(null or 空文字)、false:それ以外</returns>
        private bool IsEmpty(string val) {
            if (null == val) {
                return true;
            }
            return (0 == val.Trim().Length);
        }

        /// <summary>
        /// null、もしくは空白のみの文字列を空文字列に変換
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string Null2Empty(string val) {
            return IsEmpty(val) ? "" : val.Trim();
        }
        #endregion
    }
}
