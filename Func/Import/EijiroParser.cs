using SimpleTranslationLocal.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.Func.Import {
    class EijiroParser : IDictionaryParser {

        #region Constructor
        public EijiroParser(string file) : base(file) {
        }
        #endregion

        #region Public Method

        public override long GetCount() {
            throw new NotImplementedException();
        }

        public override WordData Read() {
            throw new NotImplementedException();
        }
        #endregion

    }
}
